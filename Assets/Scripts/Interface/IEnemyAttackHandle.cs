using UnityEngine;

public interface IEnemyAttackHandle
{
    public float timerAttack { get; set; }
    void Attack();
    void AttackDone();
}
