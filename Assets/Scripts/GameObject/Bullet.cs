using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    private LayerMask layerMask;
    private Rigidbody2D rb;
    private int damage;
    private Vector2 dirMove;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Fire(Vector2 direction, float force, int damage) {
        transform.eulerAngles = new Vector3(0,0, Utilities.Direction2Angle(direction));

        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter(Collision other) {
        
    }

    private void OnBecameInvisible() {
        PoolManager.Instance.bulletPooler.pool.Release(this);
    }
}
