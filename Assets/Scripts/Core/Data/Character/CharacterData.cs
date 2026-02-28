using System.Collections.Generic;
using Core.Data.Skills;
using Core.Data.Stats;
using UnityEngine;

namespace Core.Data.Character
{
    [CreateAssetMenu(fileName = "New Character", menuName = "Character/New Character")]
    public class CharacterData : IdentifiedObject
    {
        [SerializeField] private GameObject _visualPrefab;

        [SerializeField] private List<Skill> _activeSkillData;

        [SerializeField] private List<Skill> _passiveSkillData;

        [SerializeField] private List<StatInstance> _statData;

        public GameObject VisualPrefab => _visualPrefab;
        public List<StatInstance> StatData => _statData;

        public List<Skill> ActiveSkillData => _activeSkillData;
        public List<Skill> PassiveSkillData => _passiveSkillData;
    }
}