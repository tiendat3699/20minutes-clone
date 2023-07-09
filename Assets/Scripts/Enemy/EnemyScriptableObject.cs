using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "20minutes-clone/EnemyScriptableObject", order = 0)]
public class EnemyScriptableObject : ScriptableObject {
    public float speed;
    public int maxHP;
    public int damage;

}
