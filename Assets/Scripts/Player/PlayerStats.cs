using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private PlayerSO playerSO;
    public int projectile {get; private set;}
    public int maxHp {get; private set;}
    public int ammo {get; private set;}
    public int speed {get; private set;}
    public int bulletSpeed {get; private set;}
    public float reloadTime {get; private set;}
    public float fireRateTime {get; private set;}
    public int damage {get; private set;}
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
