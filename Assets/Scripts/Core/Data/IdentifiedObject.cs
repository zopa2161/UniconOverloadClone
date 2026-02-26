using UnityEngine;

namespace Core
{
    public class IdentifiedObject : ScriptableObject
    {
        [SerializeField] private int id;

        [SerializeField] private Sprite icon;

        [SerializeField] private string codeName;

        [SerializeField] private string displayName;

        public int ID => id;
        public Sprite Icon => icon;
        public string CodeName => codeName;
        public string DisplayName => displayName;
    }
}