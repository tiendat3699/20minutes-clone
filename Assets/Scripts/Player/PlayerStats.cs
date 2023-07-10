using System;
using UnityEngine;
using MyCustomAttribute;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private PlayerSO playerSO;
    public int projectile;
    public int maxHp;
    public int ammo;
    public int speed;
    public int bulletSpeed;
    public float reloadTime;
    public float fireRateTime;
    public int damage;
    public event Action<PlayerStats> OnUpgrade;

    private void Awake() {
        projectile = playerSO.projectile;
        maxHp = playerSO.maxHp;
        ammo = playerSO.ammo;
        speed = playerSO.speed;
        bulletSpeed = playerSO.bulletSpeed;
        reloadTime = playerSO.reloadTime;
        fireRateTime = playerSO.fireRateTime;
        damage = playerSO.damage;
    }

    public void Upgrade(TalentBase talent) {
        projectile += talent.projectilePlus;
        maxHp += talent.maxHpPlus;
        ammo += (int)(talent.ammoRate * ammo) / 100;
        speed += (int)(talent.speedRate * speed) / 100;
        bulletSpeed += (int)(talent.bulletSpeedRate * bulletSpeed) / 100;
        reloadTime -= (talent.reloadRate * reloadTime) / 100;
        fireRateTime -= (talent.fireRate * fireRateTime) / 100;
        damage += (int)(talent.damageRate * damage) / 100;
        OnUpgrade?.Invoke(this);
    }
}
