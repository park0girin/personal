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
        //Debug.Log(collision.gameObject.name);
        collision.gameObject.SetActive(false);
    }
}
