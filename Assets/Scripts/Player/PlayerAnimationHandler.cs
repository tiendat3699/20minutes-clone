using UnityEngine;


[RequireComponent(typeof(Animator))]
public class PlayerAnimationHandler : MonoBehaviour
{
    private Animator animator;
    private int deadHash, velocityHash, reloadHash;

    private void Awake() {
        animator = GetComponent<Animator>();
        deadHash = Animator.StringToHash("dead");
        velocityHash = Animator.StringToHash("velocity");
        reloadHash = Animator.StringToHash("reload");
    }

    public void PlayDead() {
        animator.SetTrigger(deadHash);
    }

    public void PlayRun(float velocity) {
        animator.SetFloat(velocityHash, velocity);
    }

    public void SetPlayReload(bool play) {
        animator.SetBool(reloadHash, play);
    }

    public bool IsAnimPlaying(string animationName, int layerIndex = 0) {
        return animator.GetCurrentAnimatorStateInfo(layerIndex).IsName(animationName);
    }


}
