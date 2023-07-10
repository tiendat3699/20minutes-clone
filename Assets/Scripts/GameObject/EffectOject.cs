using UnityEngine;


[RequireComponent(typeof(ParticleSystem))]
public class EffectOject : MonoBehaviour
{
    private ParticleSystem particle;

    private void Awake() {
        particle = GetComponent<ParticleSystem>();
    }
    private void Update() {
        if(!particle.IsAlive()) {
            PoolManager.Instance.hitImpactPooler.Release(this);
        }
    }
}
