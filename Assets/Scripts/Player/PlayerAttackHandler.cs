using System;
using System.Collections;
using UnityEngine;
using MyCustomAttribute;

public class PlayerAttackHandler : MonoBehaviour
{
    public int ammoLimit;
    public float speedBullet;
    public int damage;
    [SerializeField] private Transform shootPoin;
    [SerializeField, ReadOnly] private int bulletAmount;
    public float timeReload;
    public float timeFireRate;
    [ReadOnly] public bool reloading, ready = true;
    private PlayerAnimationHandler animationHandler;
    private static event Action<float> OnReload;

    private void Awake() {
        animationHandler = GetComponent<PlayerAnimationHandler>();
        bulletAmount = ammoLimit;
    }

    private void LateUpdate() {
        reloading = animationHandler.IsAnimPlaying("Player_reload", 1);
    }

    public void Attack(Vector2 dirAttack) {
        if(!reloading && ready && bulletAmount > 0) {
            Bullet bullet = PoolManager.Instance.bulletPooler.Spawn(shootPoin.position, Quaternion.identity);
            bullet.Fire(dirAttack, speedBullet, damage);
            bulletAmount--;
            if(bulletAmount ==0) {
                Reload();
                return;
            }
            ready = false;
            Invoke(nameof(WaitForNextAttack), timeFireRate);
        }
    }

    public void Reload() {
        if(bulletAmount < ammoLimit && !reloading) {
            StartCoroutine(ReloadCoroutine());
        }
    }

    private void WaitForNextAttack() {
        ready = true;
    }

    private IEnumerator ReloadCoroutine() {
        float timer = 0;
        animationHandler.SetPlayReload(true);
        while(timer < timeReload) {
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
            OnReload?.Invoke(timer);
        }
        animationHandler.SetPlayReload(false);
        bulletAmount = ammoLimit;

    }

    private void OnDrawGizmosSelected() {
        if(shootPoin != null) {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(shootPoin.position, 0.1f);
        }
    }
}
