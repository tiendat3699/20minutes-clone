using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ExpItem : MonoBehaviour
{
    [SerializeField] private int exp;
    private TrailRenderer trail;

    private void Awake() {
        trail = GetComponent<TrailRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            GameManager.Instance.IncreaseExp(exp);
            PoolManager.Instance.expPooler.Release(this);
        }
    }

    private void OnDisable() {
        trail.Clear();
    }
}
