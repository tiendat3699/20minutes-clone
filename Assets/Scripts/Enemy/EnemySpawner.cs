using System;
using UnityEngine;
using Random = UnityEngine.Random;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private float spawnTime;
    [SerializeField, Range(5, 20)] private int spawnPoinAmount;
    private Vector2[] spawnPoins;

    private void Awake() {
        spawnPoins = GetSpawnPoins();
    }

    private void Start() {
        InvokeRepeating(nameof(SpawnEnemy),0f, spawnTime);
    }

    private Vector2[] GetSpawnPoins() {
        if(spawnPoinAmount == 0) throw new ArgumentException("spawn poin amount cant be zero");
        Vector2[] spawnPoins = new Vector2[spawnPoinAmount];
        float angleIncrease = 360/spawnPoinAmount;
        float angle = 0;
        for (int i = 0; i < spawnPoinAmount; i++) {
            spawnPoins[i] = transform.position + Quaternion.Euler(0, 0, angle) * transform.up * radius;
            angle += angleIncrease;
        }
        return spawnPoins;
    }

    private void SpawnEnemy() {
        int index = Random.Range(0,spawnPoinAmount);
        PoolManager.Instance.enemyPooler.Spawn(spawnPoins[index], Quaternion.identity);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        Handles.color = Color.blue;
        Handles.DrawWireDisc(transform.position, Vector3.back ,radius);
        Vector2[] poins = GetSpawnPoins();
        Gizmos.color = Color.red;
        foreach(Vector2 p in poins) {
            Gizmos.DrawSphere(p, 0.2f);
        }
    }
#endif
}
