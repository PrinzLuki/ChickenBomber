
using UnityEngine;

public class SingletonL<T> : MonoBehaviour where T : Component
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                var objs = FindObjectsOfType(typeof(T)) as T[];
                if(objs.Length > 0)
                    instance = objs[0];
                if(objs.Length > 1)
                {
                    for (int i = 1; i < objs.Length; i++)
                    {
                        Destroy(objs[i]);
                    }
                }
                if(instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.hideFlags = HideFlags.HideAndDontSave;
                    instance = obj.AddComponent<T>();
                }
            }
            return instance;
        }
    }
}
