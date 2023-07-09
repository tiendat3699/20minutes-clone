

public class BulletPooler : Pooler<Bullet>
{
    public override void Init(bool collectionCheck = false, int defaultCapacity = 10, int max = 20)
    {
        base.Init(collectionCheck, defaultCapacity, max);
    }
}
