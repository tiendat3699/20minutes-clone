using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Hurtbox : MonoBehaviour
{

    [SerializeField] private LayerMask layerMask;
    [SerializeField] protected int damage;

    protected void OnTriggerEnter2D(Collider2D other) {
        if((layerMask & (1 << other.gameObject.layer)) != 0) {
            if(other.TryGetComponent(out IDamageable damageable)) {
                HandlerTrigger(other, damageable);
            }
        }
    }

    protected virtual void HandlerTrigger(Collider2D other, IDamageable damageable) {
        Vector2 dir = other.transform.position - transform.position;
        damageable.TakeDamage(damage, dir);
    }
}
