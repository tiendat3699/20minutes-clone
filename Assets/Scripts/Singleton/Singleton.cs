using UnityEngine;

public class Singleton<T> : MonoBehaviour where T: Component
{
    protected static T _instance;
    public static T Instance {
        get {
            if(_instance == null) {
                _instance = FindObjectOfType<T>();
                if(_instance ==  null) {
                    GameObject obj = new(typeof(T).Name);
                    _instance = obj.AddComponent<T>();
                }
            }

            return _instance;
        }
    }


    protected virtual void Awake() {
        if(_instance != null && _instance != this) {
            Destroy(this);
        } else {
            _instance = this as T;
        }
    }

}

public class PersistentSingleton<T> : Singleton<T> where T : Component
{
    protected override void Awake()
    {
        base.Awake();
        if(_instance == this) {
            DontDestroyOnLoad(gameObject);
        }
    }
}
