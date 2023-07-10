using System;
using UnityEngine;
using MyCustomAttribute;

public class PlayerDamageable : MonoBehaviour, IDamageable
{

    [SerializeField] private ParticleSystem hitEffect;
    private GameManager gameManager;
    [SerializeField, ReadOnly] private bool immortal;
    private static event Action OnHit;
    public event Action OnBegin;
    public event Action OnDone;
    private Rigidbody2D rb;

    private void Awake() {
        gameManager = GameManager.Instance;
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable() {
        gameManager.OnPlayerDead += HandlePlayerDead;
    }

    private void OnDisable() {
        gameManager.OnPlayerDead -= HandlePlayerDead;
    }

    private void HandlePlayerDead() {
        GetComponent<PlayerAnimationHandler>().PlayDead();
    }

    public void TakeDamage(int damage, Vector2 hitDirection)
    {
        if(!immortal) {
            if(!gameManager.isDead) {
                hitEffect.Play();
                OnHit?.Invoke();
                OnBegin?.Invoke();
                gameManager.UpdatePlayerHP(-damage);
                if(!gameManager.isDead) {
                    rb.AddForce(hitDirection * 8f, ForceMode2D.Impulse);
                }
                Invoke(nameof(ResetKnockBack), 0.2f);
                BeImmortal(0.5f);
            }
        }
    }

    public void BeImmortal(float time) {
        immortal =  true;
        Invoke(nameof(CancelImmortal), time);
    }

    private void ResetKnockBack() {
        OnDone?.Invoke();
    }

    private void CancelImmortal() {
        immortal = false;
    }
}
