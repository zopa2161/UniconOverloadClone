using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Core
{
    public partial class IODatabase : ScriptableObject
    {
        [SerializeField] private List<IdentifiedObject> datas = new();

        public IReadOnlyList<IdentifiedObject> Datas => datas;
        public int Count => datas.Count;

        public IdentifiedObject this[int index] => datas[index];

        private void SetID(IdentifiedObject target, int id)
        {
            var field = typeof(IdentifiedObject).GetField("id", BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(target, id);

#if UNITY_EDITOR
            EditorUtility.SetDirty(target);
#endif
        }

        private void ReorderDatas()
        {
            var field = typeof(IdentifiedObject).GetField("id", BindingFlags.NonPublic | BindingFlags.Instance);
            for (var i = 0; i < datas.Count; i++)
            {
                field.SetValue(datas[i], i);
#if UNITY_EDITOR
                EditorUtility.SetDirty(datas[i]);
#endif
            }
        }

        public void Add(IdentifiedObject newData)
        {
            datas.Add(newData);
            SetID(newData, datas.Count - 1);
        }

        public void Remove(IdentifiedObject data)
        {
            datas.Remove(data);
            ReorderDatas();
        }

        public IdentifiedObject Select(int id)
        {
            return datas[id];
        }

        public T Select<T>(int id) where T : IdentifiedObject
        {
            return Select(id) as T;
        }

        public IdentifiedObject Select(string codeName)
        {
            return datas.Find(item => item.CodeName == codeName);
        }

        public T Select<T>(string codeName) where T : IdentifiedObject
        {
            return Select(codeName) as T;
        }

        public IdentifiedObject Select(Func<IdentifiedObject, bool> predicate)
        {
            return datas.FirstOrDefault(predicate);
        }

        public T Select<T>(Func<T, bool> predicate) where T : IdentifiedObject
        {
            return datas.FirstOrDefault(x => predicate(x as T)) as T;
        }

        public IdentifiedObject[] Selects(Func<IdentifiedObject, bool> predicate)
        {
            return datas.Where(predicate).ToArray();
        }

        public T[] Selects<T>(Func<T, bool> predicate) where T : IdentifiedObject
        {
            return datas.Where(x => predicate(x as T)).Cast<T>().ToArray();
        }

        public T[] GetDatas<T>()
        {
            return datas.Cast<T>().ToArray();
        }

        // Data�� CodeName�� �������� ������������ ������
        public void SortByCodeName()
        {
            datas.Sort((x, y) => x.CodeName.CompareTo(y.CodeName));
            ReorderDatas();
        }
    }

    public partial class IODatabase
    {
        // Load�� Database�� Caching�ص� ����
        private static readonly Dictionary<Type, IODatabase> databasesByType = new();

        public static IODatabase GetDatabase(Type type)
        {
            // Database�� �ѹ� �ҷȴٴ°� ������ �� �Ҹ� ���� �ִٴ� �Ҹ��̹Ƿ�,
            // ������ �ٷ� ã�ƿ� �� �ְ� Caching��
            if (!databasesByType.ContainsKey(type))
                databasesByType[type] = Resources.Load<IODatabase>($"Database/{type.Name}Database");
            return databasesByType[type];
        }

        public static IODatabase GetDatabase<T>() where T : IdentifiedObject
        {
            return GetDatabase(typeof(T));
        }

        public static T StaticSelect<T>(int id) where T : IdentifiedObject
        {
            return GetDatabase<T>().Select<T>(id);
        }

        public static T StaticSelect<T>(string codeName) where T : IdentifiedObject
        {
            return GetDatabase<T>().Select<T>(codeName);
        }

        public static T StaticSelect<T>(Func<T, bool> predicate) where T : IdentifiedObject
        {
            return GetDatabase<T>().Select(predicate);
        }

        public static IdentifiedObject StaticSelect(Type type, Func<IdentifiedObject, bool> predicate)
        {
            return GetDatabase(type).Select(predicate);
        }

        public static IdentifiedObject StaticSelect(Type type, int id)
        {
            return GetDatabase(type).Select(id);
        }

        public static IdentifiedObject StaticSelect(Type type, string codeName)
        {
            return GetDatabase(type).Select(codeName);
        }

        public static T[] StaticSelects<T>(Func<T, bool> predicate) where T : IdentifiedObject
        {
            return GetDatabase<T>().Selects(predicate);
        }

        public static IdentifiedObject[] StaticSelects(Type type, Func<IdentifiedObject, bool> predicate)
        {
            return GetDatabase(type).Selects(predicate);
        }
    }
}