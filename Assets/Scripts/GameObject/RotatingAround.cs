using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class RotatingAround : MonoBehaviour
{
    [SerializeField] private Vector3 speed;
    [SerializeField] private float radius;

    private void Update() {
        transform.Rotate(speed * Time.deltaTime);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos() {
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, Vector3.back, radius);
    }
#endif
}
