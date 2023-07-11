using System;

public class PoolManager : Singleton<PoolManager>
{
    public BulletPooler bulletPooler;
    public EnemyPooler enemyPooler;
    public ExpPooler expPooler;
    public hitImpactPooler hitImpactPooler;

    protected override void Awake()
    {
        base.Awake();
        bulletPooler.Init();
        enemyPooler.Init();
        expPooler.Init();
        hitImpactPooler.Init();
    }
}


//bullet pooler
[Serializable]
public class BulletPooler : Pooler<Bullet>
{
    public override void Init(bool collectionCheck = false, int defaultCapacity = 10, int max = 100)
    {
        base.Init(collectionCheck, defaultCapacity, max);
    }
}

//enemy pooler
[Serializable]
public class EnemyPooler : Pooler<BasicEnemyBehaviour>
{
    public override void Init(bool collectionCheck = false, int defaultCapacity = 10, int max = 100)
    {
        base.Init(collectionCheck, defaultCapacity, max);
    }
}

//exp pooler
[Serializable]
public class ExpPooler : Pooler<ExpItem>
{
    public override void Init(bool collectionCheck = false, int defaultCapacity = 10, int max = 100)
    {
        base.Init(collectionCheck, defaultCapacity, max);
    }
}

//hitImpact pooler
[Serializable]
public class hitImpactPooler : Pooler<EffectOject>
{
    public override void Init(bool collectionCheck = false, int defaultCapacity = 10, int max = 100)
    {
        base.Init(collectionCheck, defaultCapacity, max);
    }
}


