using UnityEngine;

public enum TalentType {
    AddWeapon,
    StatsUpgrade,
}


[CreateAssetMenu(fileName = "TalentBase", menuName = "20minutes-clone/TalentBase", order = 0)]
public class TalentBase : ScriptableObject
{
    public Sprite sprite;
    public TalentType type;
    public GameObject talentPrefabs;
    [TextArea(5, 10)] public string description;
}
