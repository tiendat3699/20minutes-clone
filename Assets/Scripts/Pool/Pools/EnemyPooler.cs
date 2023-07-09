

public class EnemyPooler : Pooler<EnemyBehaviour>
{
    public override void Init(bool collectionCheck = false, int defaultCapacity = 10, int max = 100)
    {
        base.Init(collectionCheck, defaultCapacity, max);
    }
}
