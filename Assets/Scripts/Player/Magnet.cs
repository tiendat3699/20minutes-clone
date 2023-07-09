using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(CircleCollider2D))]
public class Magnet : MonoBehaviour
{
    public float radius;
    [SerializeField] private float speed;
    private CircleCollider2D circleCollider;

    private void Awake() {
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.radius = radius;
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(other.TryGetComponent(out ExpItem expItem)) {
            Vector2 dir = (transform.position - expItem.transform.position).normalized;
            expItem.transform.Translate(dir * speed * Time.deltaTime);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, Vector3.back, radius);
    }
#endif
}
