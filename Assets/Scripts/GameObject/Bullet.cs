using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Bullet : Hurtbox
{
    private Rigidbody2D rb;
    private Vector2 dirMove;
    private Vector2 originPos;
    private TrailRenderer trail;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        trail = GetComponent<TrailRenderer>();
    }

    public void Fire(Vector2 direction, float force, int damage) {
        this.damage = damage;
        transform.eulerAngles = new Vector3(0,0, Utilities.Direction2Angle(direction));
        originPos = transform.position;
        dirMove = direction;
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }

    private void Update() {
        if(Vector2.Distance(originPos, rb.position) >= 30) {
            PoolManager.Instance.bulletPooler.Release(this);
        }
    }

    protected override void HandlerTrigger(Collider2D other, IDamageable damageable)
    {
        base.HandlerTrigger(other, damageable);
        PoolManager.Instance.bulletPooler.Release(this);
    }

    private void OnDisable() {
        trail.Clear();
    }
}
