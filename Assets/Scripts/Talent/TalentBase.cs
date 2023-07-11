using UnityEngine;
using MyCustomAttribute;

public enum TalentType {
    AddTalent,
    StatsUpgrade,
}


[CreateAssetMenu(fileName = "TalentBase", menuName = "20minutes-clone/TalentBase", order = 0)]
public class TalentBase : ScriptableObject
{
    public Sprite sprite;
    public TalentType type;
    public GameObject talentPrefabs;
    public int projectilePlus;
    public int maxHpPlus;
    [Label("Ammo Rate (%)")] public float ammoRate;
    [Label("Speed Rate (%)")] public float speedRate;
    [Label("Bullet Speed Rate (%)")] public float bulletSpeedRate;
    [Label("Reload Rate (%)")] public float reloadRate;
    [Label("Fire Rate (%)")] public float fireRate;
    [Label("Damage Rate (%)")] public float damageRate;
    [TextArea(5, 10)] public string description;
}
