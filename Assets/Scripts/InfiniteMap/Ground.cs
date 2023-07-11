using System.Collections;
using UnityEngine;

public class Ground : MonoBehaviour
{

    private void OnTriggerExit2D(Collider2D other) {
        StartCoroutine(MoveToNewPos());
    }

    private IEnumerator MoveToNewPos() {
        yield return new WaitForFixedUpdate();
        Vector3 playerPos = GameManager.Instance.player.position;
        float diffX = Mathf.Abs(playerPos.x - transform.position.x);
        float diffY = Mathf.Abs(playerPos.y - transform.position.y);

        Vector2 playerDir = GameManager.Instance.playerMoveDirection;
        float dirX = playerDir.x > 0 ? 1: -1;
        float dirY = playerDir.y > 0 ? 1: -1;

        if(diffX > diffY) {
            transform.Translate(dirX * 60 * Vector3.right );
        } else if(diffX < diffY) {
            transform.Translate(dirY * 60 * Vector3.up );
        }
    }
}


