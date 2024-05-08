using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    public GameManager GameManager;
    public BuffType Buff_name;
    float UpDamage;
    public enum BuffType
    {
        Freeze,
        Heal,
        DamageUp,
        SkillGauge
    }
    private void OnEnable()
    {
        GameManager = FindObjectOfType<GameManager>();
        UpDamage = ScenesManager.Instance.BulletDamage / 5;
    }
    private void Update()
    {
        switch (Buff_name)
        {
            case BuffType.Freeze:
                // ���� ȿ�� ����
                break;
            case BuffType.Heal:
                // ü�� ȸ�� ȿ�� ����
                break;
            case BuffType.DamageUp:
                // ������ ���� ȿ�� ����
                break;
            case BuffType.SkillGauge:
                // Ư�� ���� ������ ���� ����
                break;
        }
        transform.Translate(Vector2.down * 2f * Time.deltaTime);
        if (Time.timeScale == 0)
        {
            gameObject.SetActive(false);
        }
        if (GameManager.GameOver) this.gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "player")
        {
            GameManager.BuffCount++;
            switch (Buff_name)
            {
                case BuffType.Freeze:
                    GameManager.FreezeSkillTime = 0;
                    GameManager.Freeze = true;
                    break;
                case BuffType.Heal:
                    if (ScenesManager.Instance.PlayerLifeMax > GameManager.PlayerLife)
                    {
                        GameManager.PlayerLife++;
                    }
                    break;
                case BuffType.DamageUp:
                    GameManager.BulletDamage += UpDamage;
                    break;
                case BuffType.SkillGauge:
                    GameManager.SkillCount++;
                    break;
            }

            if (GameManager.buffsQueue.Count < 50)
            {
                GameManager.buffsQueue.Enqueue(Buff_name.ToString());
            }
            gameObject.SetActive(false);
        }
    }
}