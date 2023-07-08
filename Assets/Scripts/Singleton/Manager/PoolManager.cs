using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    [SerializeField] PoolerScriptableObject poolerScriptable;
    [HideInInspector] public BulletPooler bulletPooler;

    protected override void Awake()
    {
        base.Awake();

        bulletPooler = poolerScriptable.bulletPooler;
        bulletPooler.Init();
    }
}
