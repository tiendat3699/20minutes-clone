using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyHurtBox : MonoBehaviour
{
    private EnemyScriptableObject enemyScriptable;

    private void Awake() {
        enemyScriptable = GetComponentInParent<EnemyBehaviour>().enemyScriptable;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.TryGetComponent(out IDamageable damageable)) {
            Vector2 dir = (transform.forward - other.transform.position).normalized;
            damageable.TakeDamage(enemyScriptable.damage, dir);
        }

    }
}
