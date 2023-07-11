using UnityEngine;

public class BossBehaviour : BasicEnemyBehaviour
{
    [SerializeField] private float timeToAttack;
    [SerializeField] private ParticleSystem particle;
    private float timer;
    private float charge;
    private Vector2 dirAttack;

    private void Update() {
        if(Vector3.Distance(transform.position, target.position) <= 15f) {
            timer += Time.deltaTime;
            if(timer >= timeToAttack) {
                timer = 0;
                rb.velocity = Vector2.zero;
                dirAttack = (target.position - transform.position).normalized;
                particle.Play();
                state = EnemyState.Attack;
            }
        } else {
            state = EnemyState.Chase;
            timer = 0;
        }
    }

    protected override void Attack()
    {
        base.Attack();
        charge += Time.fixedDeltaTime;
        if(charge >= 2) {
            particle.Stop();
            rb.AddForce(dirAttack * 10f, ForceMode2D.Impulse);
            Invoke(nameof(AttackDone), 0.8f);
            charge = 0;
        }
    }

    private void AttackDone() {
        state = EnemyState.Chase;
    }
}
