using System.Collections;
using System.Collections.Generic;
using Core.Data.Battle.BattleLogs;
using Core.Data.Character;
using Core.Enums;
using Core.Interfaces;
using Logic.Battle;
using Presentation.Battle.Characters;
using UnityEngine;

namespace Presentation.Battle
{
    public class BattleViewManager : MonoBehaviour
    {
        [SerializeField] private GameObject _characterPrefab;
        [SerializeField] private Transform[] _friendlySpawn;
        [SerializeField] private Transform[] _enemySpawn;

        private BattleManager _battleManager;

        // 🌟 핵심: 영혼(Logic Data)과 육체(View)를 연결해두는 명부(Cache)
        private Dictionary<CharacterInstance, CharacterView> _viewCache =
            new Dictionary<CharacterInstance, CharacterView>();

        public void Initialize(BattleManager battleManager)
        {
            _battleManager = battleManager;

            // 전투 재시작 등을 대비해 명부를 한 번 깨끗하게 비워줍니다.
            _viewCache.Clear();

            SpawnCharacters(battleManager.BattleContext);
        }

        private void SpawnCharacters(IBattleContext battleContext)
        {
            var friendlyBoard = battleContext.friendlyBoard;
            var enemyBoard = battleContext.enemyBoard;

            // 🟦 아군 스폰
            for (var f = 0; f < 6; f++)
            {
                var character = friendlyBoard.characters[f];
                if (character == null) continue;

                var viewObj = Instantiate(_characterPrefab, _friendlySpawn[f]);
                var view = viewObj.GetComponent<CharacterView>();

                view.Initialize(character);

                var depthGroup = f % 3;
                view.SetSortingOrder(10 - depthGroup);

                // 🌟 명부에 등록! (아군)
                _viewCache.Add(character, view);
            }

            // 🟥 적군 스폰
            for (var e = 0; e < 6; e++)
            {
                var character = enemyBoard.characters[e];
                if (character == null) continue;

                var viewObj = Instantiate(_characterPrefab, _enemySpawn[e]);
                var view = viewObj.GetComponent<CharacterView>();

                view.Initialize(character, true);

                var depthGroup = e % 3;
                view.SetSortingOrder(10 - depthGroup);

                // 🌟 명부에 등록! (적군)
                _viewCache.Add(character, view);
            }
        }

        // 🌟 헬퍼 함수: 나중에 대본(Log)을 읽을 때 특정 캐릭터의 View를 즉시 찾아줍니다.
        public CharacterView GetViewFromInstance(CharacterInstance instance)
        {
            if (_viewCache.TryGetValue(instance, out var view)) return view;

            //Debug.LogWarning($"[BattleViewManager] 앗! 명부에 없는 캐릭터입니다: {instance.Data.CharacterName}");
            return null;
        }
        
        
        public IEnumerator PlayBattleRoutine(List<BattleLogEvent> logs)
        {
            Debug.Log("🎬 [연출 시작] 대본 리딩을 시작합니다!");

            foreach (var logEvent in logs)
            {
                Debug.Log($"[▶ 재생중] {logEvent.log}");

                // 🌟 패턴 매칭: logEvent가 ApplyEffectLog라면, effectLog라는 변수로 즉시 캐스팅해서 사용합니다.
                if (logEvent is ApplyEffectLog effectLog)
                {
                    var actorView = GetViewFromInstance(effectLog.Actor);
                    var targetView = GetViewFromInstance(effectLog.Target);
                
                    var effect = ((ApplyEffectLog)logEvent).Effect;
                    // EffectType에 따라 배우들의 연기를 지시합니다.
                    switch (effect.Type) // (이름이 Type이라면 effectLog.Type으로 수정)
                    {
                        case EffectType.Attack:
                            if (actorView != null) actorView.PlayAttackAnimation();
                            if (targetView != null) targetView.PlayHitEffect(); // 맞는 연출 추가!
                            break;
                    
                        case EffectType.Heal:
                            if (actorView != null) actorView.PlaySimpleActionEffect(); // 힐 시전 모션
                            //if (targetView != null) targetView.PlayHealEffect(); // 초록색 반짝임
                            break;
                    
                        // 필요에 따라 Buff, Debuff 등 추가...
                    }
                }
                // ApplyEffectLog가 아닌 다른 일반 로그(예: 턴 시작, 이동 등)일 경우
                else 
                {
                    if (logEvent.Actor != null)
                    {
                        var actorView = GetViewFromInstance(logEvent.Actor);
                        if (actorView != null) actorView.PlaySimpleActionEffect();
                    }
                }

                // 3. 핵심: 다음 로그로 넘어가기 전에 1초 대기! 
                yield return new WaitForSeconds(1.0f); 
            }

            Debug.Log("🏁 [연출 종료] 모든 전투 연출이 끝났습니다!");
        }
    }
}

