using UnityEngine;
using UnityEngine.Pool;

public class Pooler<T>  where T: MonoBehaviour
{
    [SerializeField] private T prefabs;
    private ObjectPool<T> pool;

    public T Spawn() {
        return pool.Get();
    }

    public void Release(T obj) {
        pool.Release(obj);
    }

    public T Spawn(Vector2 position, Quaternion rotation) {
        T obj = pool.Get();
        obj.transform.SetPositionAndRotation(position, rotation);
        return obj;
    }

    public virtual void Init(bool collectionCheck = false, int defaultCapacity = 10, int max = 100) {
        pool = new ObjectPool<T>(CreateFunc, GetFunc, ReleaseFunc, DestroySetup, collectionCheck, defaultCapacity, max);
    }


    private T CreateFunc() {
        return GameObject.Instantiate(prefabs);
    }

    private void GetFunc(T obj) {
        obj.gameObject.SetActive(true);
    }

    private void ReleaseFunc(T obj) {
        obj.gameObject.SetActive(false);
    }

    private void DestroySetup(T obj) {
        GameObject.Destroy(obj);
    }

}
