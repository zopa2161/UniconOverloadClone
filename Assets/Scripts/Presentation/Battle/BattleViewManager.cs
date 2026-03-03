using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Data.Battle.BattleLogs;
using Core.Data.Character;
using Core.Enums;
using Core.Interfaces;
using Logic.Battle;
using Presentation.Battle.Characters;
using Presentation.UI;
using UnityEngine;

namespace Presentation.Battle
{
    public class BattleViewManager : MonoBehaviour
    {
        [SerializeField] private GameObject _characterPrefab;
        [SerializeField] private Transform[] _friendlySpawn;
        [SerializeField] private Transform[] _enemySpawn;

        [SerializeField] private Transform _midCenter;

        [SerializeField] private BattleUIManager _uiManager;

        // 🌟 핵심: 영혼(Logic Data)과 육체(View)를 연결해두는 명부(Cache)
        private readonly Dictionary<CharacterInstance, CharacterView> _viewCache = new();


        private BattleManager _battleManager;

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

                view.Initialize(character, spawnPoint: _friendlySpawn[f]);

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

                view.Initialize(character, true, _enemySpawn[e]);

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
                if (logEvent is SkillDeclareLog skillDeclareLog)
                {
                    var actorView = GetViewFromInstance(skillDeclareLog.Actor);
                    
           
                    yield return new WaitForSeconds(1.0f);
                }
                else if (logEvent is ApplyEffectLog effectLog)
                {
                    var actorView = GetViewFromInstance(effectLog.Actor);
                    if (effectLog.isConsume)
                    {
                        Debug.Log("consuming");
                        if (effectLog.Skill.Type == SkillType.Active)
                            actorView.ConsumeVisualAP(effectLog.Skill.ConsumingPoint); // AP 1 소모 (기획에 따라 소모량 조절)
                        else if (effectLog.Skill.Type == SkillType.Passive) actorView.ConsumeVisualPP(effectLog.Skill.ConsumingPoint); // PP 1 소모
                    }

                    var targetViews = new List<CharacterView>();
                    if (_uiManager != null) _uiManager.ShowSkillName(effectLog.Skill.CodeName); // 스킬 이름 데이터 접근
                    foreach (var target in effectLog.Targets)
                    {
                        var t = GetViewFromInstance(target);
                        if (t != null) targetViews.Add(t);
                    }

                    var effect = ((ApplyEffectLog)logEvent).Effect;
                    // EffectType에 따라 배우들의 연기를 지시합니다.
                    switch (effect.Type) // (이름이 Type이라면 effectLog.Type으로 수정)
                    {
                        ///
                        /// 지금은 단순히 타입에 따라서 분기를 하지만 나중 가서는 이펙트 내부의 함수를 이용해서
                        /// 값을 정하는 방식으로 해야함.
                        /// 이런식으로 어택/ 힐 을 뭉뚱거리는건 말도 안되지;
                        case EffectType.Attack:
                            if (actorView != null) actorView.PlayAttackAnimation();
                            for (var t = 0; t < targetViews.Count; t++)
                            {
                                if (targetViews[t] != null) targetViews[t].PlayHitEffect();
                                targetViews[t].TakeVisualHP(-(int)effectLog.Effections[t]);
                            }

                            break;

                        case EffectType.Heal:
                            if (actorView != null) actorView.PlayCastingAnimation(); // 힐 시전 모션
                            for (var t = 0; t < targetViews.Count; t++)
                            {
                                if (targetViews[t] != null) targetViews[t].PlayHealEffect();
                                targetViews[t].TakeVisualHP((int)effectLog.Effections[t]);
                            }

                            break;

                        // 필요에 따라 Buff, Debuff 등 추가...
                    }
                }
                // ApplyEffectLog가 아닌 다른 일반 로그(예: 턴 시작, 이동 등)일 경우
                else if (logEvent is BeforeHitLog beforeHitLog)
                {
                    var actorView = GetViewFromInstance(beforeHitLog.Actor);
    
                    // actorView가 존재하고, 근거리 공격일 때만 실행 (중첩 if문 병합)
                    if (actorView != null && beforeHitLog.effet.RangeType == EffectRangeType.Melee)
                    {
                        var targetView = GetViewFromInstance(beforeHitLog.Targets[0]);
        
                        // 1. Transform 전체가 아닌 Vector3 '위치 값'만 빼서 복사본을 만듭니다.
                        Vector3 targetFrontPosition = targetView.transform.position;
        
                        // 2. 복사된 Vector3 값의 x좌표를 팩션에 따라 수정합니다.
                        if (beforeHitLog.Targets[0].Faction == CharacterFaction.Friendly)
                            targetFrontPosition.x -= 0.5f;
                        else
                            targetFrontPosition.x += 0.5f;
            
                        // 3. 계산이 끝난 새로운 좌표(Vector3)로 이동시킵니다.
                        actorView.MoveToTarget(targetFrontPosition);
                        actorView.isInitialPosition = false;
                    }
                }
                else if (logEvent is AfterHitLog afterHitLog)
                {
                    // var actorView = GetViewFromInstance(afterHitLog.Actor);
                    // if (actorView != null)
                    // {
                    //     actorView.ReturnToSpawnPoint();
                    //     if (_uiManager != null) _uiManager.OffSkillName();
                    // }
                }
                else if (logEvent is CharacterDeathLog deathLog)
                {
                    var actorView = GetViewFromInstance(deathLog.Actor);
                    if (actorView != null)
                    {
                        actorView.CharacterDeath();
                        Debug.Log("Character Death");
                    }
                }
                else if (logEvent is ActiveSkillEndLog activeSkillEndLog)
                {
                    foreach (var view in _viewCache.Values)
                    {
                        if(!view.isInitialPosition) view.ReturnToSpawnPoint();
                    }
                }

                // 3. 핵심: 다음 로그로 넘어가기 전에 1초 대기! 
                yield return new WaitForSeconds(0.5f);
            }

            Debug.Log("🏁 [연출 종료] 모든 전투 연출이 끝났습니다!");
        }
    }
}