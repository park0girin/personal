using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public GameObject player;
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle")) // 충돌한 오브젝트의 태그를 확인
        {
            // 충돌 오브젝트의 충돌체를 가져옴
            Collider2D collider = collision.collider;

            // 충돌 오브젝트의 경계를 기준으로 플레이어의 위치 조정
            Vector2 newPosition = player.transform.position;

            // 충돌 오브젝트의 바깥쪽으로 이동
            if (newPosition.x < collider.bounds.min.x)
            {
                newPosition.x = collider.bounds.min.x;
            }
            else if (newPosition.x > collider.bounds.max.x)
            {
                newPosition.x = collider.bounds.max.x;
            }

            if (newPosition.y < collider.bounds.min.y)
            {
                newPosition.y = collider.bounds.min.y;
            }
            else if (newPosition.y > collider.bounds.max.y)
            {
                newPosition.y = collider.bounds.max.y;
            }

            player.transform.position = newPosition;
        }
    }
}
