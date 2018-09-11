using UnityEngine;

namespace fl
{
    public class ResourceManager : MonoBehaviour
    {
        public static ResourceManager Instance;
        public GameObject[] prefabs;

        private void Awake()
        {
            Instance = this;
        }

        public static GameObject GetPrefab(string name)
        {
            return Get(name + " (UnityEngine.GameObject)", Instance.prefabs);
        }

        public static T Get<T>(string name, T[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i].ToString() == name)
                {
                    return array[i];
                }
            }
            return default(T);
        }
    }
}
