using System;
using UnityEngine;
using Random = UnityEngine.Random;
using MyCustomAttribute;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class WaveManager : Singleton<WaveManager>
{
    [SerializeField] private float radius;
    [SerializeField] private float maxSpawnTime;
    [SerializeField, Range(5, 20)] private int spawnPoinAmount;
    [SerializeField, ReadOnly] private float spawnTime;
    [SerializeField] private int amountSpawnActive = 2;
    [SerializeField] private float countdownTime;
    [SerializeField] private bool active = true;
    [SerializeField] private BossSpawnInfo[] bossSpawnInfos;
    private float timerSpawn, timerIncreaseSpawnActive, timerBoss;
    public event Action<float> OnCountDown;
    private GameManager gameManager;

    protected override void Awake() {
        base.Awake();
        gameManager = GameManager.Instance;
        spawnTime = maxSpawnTime;
    }

    private void Start() {
        timerSpawn = countdownTime;
        if(active) {
            InvokeRepeating(nameof(SpawnEnemy),0f, spawnTime);
        }
    }

    private void Update() {
        if(timerSpawn > 0) {
            timerSpawn -= Time.deltaTime;
            timerIncreaseSpawnActive += Time.deltaTime;
            timerBoss += Time.deltaTime;
        } else {
            timerSpawn = 0;
            gameManager.Win();
        }
        SpawnBoss();
        CaculateSpawnTime();
        OnCountDown?.Invoke(timerSpawn);
    }

    private void CaculateSpawnTime() {

        spawnTime = maxSpawnTime * (timerSpawn / countdownTime);
        if(timerIncreaseSpawnActive >= 120) {
            amountSpawnActive++;
            timerIncreaseSpawnActive = 0;
        }
    }

    private Vector2[] GetSpawnPoins(Vector3 center) {
        if(spawnPoinAmount == 0) throw new ArgumentException("spawn poin amount cant be zero");
        Vector2[] spawnPoins = new Vector2[spawnPoinAmount];
        float angleIncrease = 360/spawnPoinAmount;
        float angle = 0;
        for (int i = 0; i < spawnPoinAmount; i++) {
            spawnPoins[i] = center + Quaternion.Euler(0, 0, angle) * transform.up * radius;
            angle += angleIncrease;
        }
        return spawnPoins;
    }

    private void SpawnEnemy() {
        Vector2[] spawnPoins = GetSpawnPoins(gameManager.player.position);
        for(int i = 0 ; i < amountSpawnActive; i++) {
            int index = Random.Range(0,spawnPoinAmount);
            PoolManager.Instance.enemyPooler.Spawn(spawnPoins[index], Quaternion.identity);
        }
    }

    private void SpawnBoss() {
        for(int i = 0; i < bossSpawnInfos.Length; i++) {
            if(!bossSpawnInfos[i].spawned && timerBoss >= bossSpawnInfos[i].timeSpawm) {
                Vector2[] spawnPoins = GetSpawnPoins(gameManager.player.position);
                int index = Random.Range(0,spawnPoinAmount);
                Instantiate(bossSpawnInfos[i].bossPrefab, spawnPoins[index], Quaternion.identity);
                bossSpawnInfos[i].spawned = true;
            }

        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        Handles.color = Color.blue;
        Vector3 center = FindObjectOfType<PlayerController>().transform.position;
        Handles.DrawWireDisc(center, Vector3.back ,radius);
        Vector2[] poins = GetSpawnPoins(center);
        Gizmos.color = Color.red;
        foreach(Vector2 p in poins) {
            Gizmos.DrawSphere(p, 0.2f);
        }
    }
#endif
}

[Serializable]
public class BossSpawnInfo {
    public float timeSpawm;
    public BossBehaviour bossPrefab;
    [ReadOnly] public bool spawned;
}
