using Unity.VisualScripting;
using UnityEngine;

public class player_con : MonoBehaviour
{
    public float speed = 3.0f;
    

    private void OnEnable()
    {
        transform.position = new Vector2(0, -3.0f);
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
