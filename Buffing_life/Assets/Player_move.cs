using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_move : MonoBehaviour
{
    private Vector2 playerPos;
    private Vector2 diffPos;
    private Vector2 cursorPos;
    public GameObject player;
    private void Update()
    {
        if (GameManager.Instance.GameOver)
        {
            player.SetActive(false);
            GameManager.Instance.GameOverUI.SetActive(true);
        }
        else
        {
            if (GameManager.Instance.PlayerLife <= 0)
            {
                GameManager.Instance.GameOver = true;
            }
        }
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.Gameing) playerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    private void OnMouseDrag()
    {
        if (GameManager.Instance.Gameing)
        {
            cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            diffPos = cursorPos - playerPos;
            playerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            player.transform.position = new Vector2(Mathf.Clamp(player.transform.position.x + diffPos.x, -2.8f, 2.8f),
                (Mathf.Clamp(player.transform.position.y + diffPos.y, -4.0f, 1.0f)));
        }
    }

}
