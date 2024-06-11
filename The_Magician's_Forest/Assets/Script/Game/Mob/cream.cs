using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cream : MonoBehaviour
{
    public GameManager GameManager;
    public BOSS BOSS;
    public float inputHP;
    public float HP;
    float trigger_time;
    bool player_trigger;
    private void OnEnable()
    {
        GameManager = FindObjectOfType<GameManager>();
        HP = inputHP;
        trigger_time = 0;
        player_trigger = false;
        BOSS.Barrier_Active = true;
    }
    private void Update()
    {
        this.gameObject.SetActive(BOSS.Barrier_Active);
        if (HP <= 0)
        {
            BOSS.Barrier_Active = false;
            Debug.Log("Å©¸² ¶ÕÀ½");
        }
        if (player_trigger)
        {
            trigger_time += Time.deltaTime;
            if (trigger_time >= 2)
            {
                GameManager.PlayerLife--;
                trigger_time = 0;
            }
        }
        if (GameManager.GameOver) this.gameObject.SetActive(false);
        if (!GameManager.BossBattle) this.gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("bullet"))
        {
            HP -= GameManager.BulletDamage;
            collision.gameObject.SetActive(false);
        }
        if (collision.gameObject.name == "player")
        {
            GameManager.PlayerLife--;
            player_trigger = true;
        }
        if (collision.CompareTag("banana"))
        {
            HP = 0;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "player")
        {
            player_trigger = false;
        }
    }
    private void OnDisable()
    {
        Debug.Log("Cream disabled");
    }
}
