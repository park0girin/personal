using Unity.VisualScripting;
using UnityEngine;

public class player_con : MonoBehaviour
{
    public float speed = 3.0f;
    private float Player_posY = -3f;
    private void OnEnable()
    {
        transform.position = new Vector2(0, Player_posY);
    }
    private void Update()
    {
        if (GameManager.Instance.GameOver)
        {
            gameObject.SetActive(false);
            GameManager.Instance.GameOverUI.SetActive(true);
        }
        else
        {
            MovePlayer();
            if (GameManager.Instance.PlayerLife <= 0)
            {
                GameManager.Instance.GameOver = true;
            }
        }
    }

    private void MovePlayer()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(horizontalInput, 0f) * speed * Time.deltaTime;

        // 이동 범위 제한
        float newX = Mathf.Clamp(transform.position.x + movement.x, -2.8f, 2.8f);
        transform.position = new Vector2(newX, Player_posY);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Mob"))
        {
            collision.gameObject.SetActive(false);
            GameManager.Instance.PlayerLife -= 1;
        }
        if (collision.CompareTag("BOSS"))
        {
            GameManager.Instance.PlayerLife -= 2;
        }
    }
}
