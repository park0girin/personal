using Unity.VisualScripting;
using UnityEngine;

public class Player_HP : MonoBehaviour
{
    public GameManager GameManager;
    public float speed = 3.0f;
    public GameObject HPBar;
    public GameObject SkillBar;

    private void OnEnable()
    {
        transform.position = new Vector2(0, -3.0f);
        HPBar.SetActive(true);
        SkillBar.SetActive(true);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Mob"))
        {
            collision.gameObject.SetActive(false);
            GameManager.PlayerLife -= 1;
        }
        if (collision.CompareTag("BOSS"))
        {
            GameManager.PlayerLife -= 2;
        }
        GameManager.HPGaugeBar.ChangeGaugeValue(GameManager.PlayerLife);
        GameManager.SkillGaugeBar.ChangeGaugeValue(GameManager.SkillCount);
    }
}
