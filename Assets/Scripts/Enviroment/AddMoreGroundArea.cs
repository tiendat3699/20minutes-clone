using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AddMoreGroundArea : MonoBehaviour
{


    private void Awake() {
    }


    private Vector2[] GetSpawnPositions() {
        Vector2[] spawnPositions = new Vector2[4];
        Tilemap tilemap = GetComponent<Tilemap>();
        spawnPositions[0] = transform.position + new Vector3(0, tilemap.size.y);
        spawnPositions[1] = transform.position + new Vector3(tilemap.size.x, 0);
        spawnPositions[2] = transform.position + new Vector3(0, -tilemap.size.y);
        spawnPositions[3] = transform.position + new Vector3(-tilemap.size.x, 0);
        return spawnPositions;
    }



    private void OnDrawGizmosSelected() {
        Vector2[] positions = GetSpawnPositions();
        Gizmos.color = Color.blue;
        foreach(Vector2 pos in positions) {
            Gizmos.DrawSphere(pos, 0.5f);
        }
    }
}
