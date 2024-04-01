using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_move : MonoBehaviour
{
    public GameManager GameManager;
    private Vector2 playerPos;
    private Vector2 diffPos;
    private Vector2 cursorPos;
    public GameObject player;
    private void Update()
    {
        if (GameManager.GameOver)
        {
            player.SetActive(false);
            GameManager.GameOverUI.SetActive(true);
        }
        else
        {
            if (GameManager.PlayerLife <= 0)
            {
                GameManager.GameOver = true;
            }
        }
    }

    private void OnMouseDown()
    {
        if (GameManager.Gameing) playerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    private void OnMouseDrag()
    {
        if (GameManager.Gameing)
        {
            cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            diffPos = cursorPos - playerPos;
            playerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            player.transform.position = new Vector2(Mathf.Clamp(player.transform.position.x + diffPos.x, -2.8f, 2.8f),
                (Mathf.Clamp(player.transform.position.y + diffPos.y, -3.5f, 4.0f)));
        }
    }

}
