using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Back_Box : MonoBehaviour
{
    public GameManager GameManager;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (GameManager.GameOver) collision.gameObject.SetActive(false);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("BOSS") && !collision.CompareTag("Thunder")) collision.gameObject.SetActive(false);
    }
}
