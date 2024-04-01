using Unity.VisualScripting;
using UnityEngine;

public class Player_HP : MonoBehaviour
{
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
            GameManager.Instance.PlayerLife -= 1;
        }
        if (collision.CompareTag("BOSS"))
        {
            GameManager.Instance.PlayerLife -= 2;
        }
        GameManager.Instance.HPGaugeBar.ChangeGaugeValue(GameManager.Instance.PlayerLife);
        GameManager.Instance.SkillGaugeBar.ChangeGaugeValue(GameManager.Instance.SkillCount);
    }
}
