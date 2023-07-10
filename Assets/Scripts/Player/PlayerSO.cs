using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSO", menuName = "20minutes-clone/PlayerSO", order = 0)]
public class PlayerSO : ScriptableObject
{
    public int projectile;
    public int maxHp;
    public int ammo;
    public int speed;
    public int bulletSpeed;
    public float reloadTime;
    public float fireRateTime;
    public int damage;
}
