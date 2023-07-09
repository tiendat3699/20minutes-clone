using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    [SerializeField] PoolerScriptableObject poolerScriptable;
    [HideInInspector] public BulletPooler bulletPooler;
    [HideInInspector] public EnemyPooler enemyPooler;

    protected override void Awake()
    {
        base.Awake();

        bulletPooler = poolerScriptable.bulletPooler;
        enemyPooler = poolerScriptable.enemyPooler;
        bulletPooler.Init();
        enemyPooler.Init();
    }
}
