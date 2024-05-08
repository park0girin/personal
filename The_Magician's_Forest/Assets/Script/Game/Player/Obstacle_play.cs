using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public GameObject player;
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle")) // �浹�� ������Ʈ�� �±׸� Ȯ��
        {
            // �浹 ������Ʈ�� �浹ü�� ������
            Collider2D collider = collision.collider;

            // �浹 ������Ʈ�� ��踦 �������� �÷��̾��� ��ġ ����
            Vector2 newPosition = player.transform.position;

            // �浹 ������Ʈ�� �ٱ������� �̵�
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
