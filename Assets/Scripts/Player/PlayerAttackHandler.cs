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
    private static event Action<float> onReload;

    private void Awake() {
        animationHandler = GetComponent<PlayerAnimationHandler>();
        bulletAmount = ammoLimit;
    }

    public void Attack(Vector2 dirAttack) {
        if(!reloading && ready && bulletAmount > 0) {
            ready = false;
            Bullet bullet = PoolManager.Instance.bulletPooler.Spawn(shootPoin.position, Quaternion.identity);
            bullet.Fire(dirAttack, speedBullet, damage);
            bulletAmount--;
            if(bulletAmount ==0) {
                Reload();
            }

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
        reloading = true;
        while(timer < timeReload) {
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
            onReload?.Invoke(timer);
        }

        bulletAmount = ammoLimit;
        reloading = false;
        animationHandler.SetPlayReload(false);

    }

    private void OnDrawGizmosSelected() {
        if(shootPoin != null) {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(shootPoin.position, 0.1f);
        }
    }
}
