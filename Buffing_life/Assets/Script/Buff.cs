using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    public BuffType Buff_name;
    public enum BuffType
    {
        Freeze,
        Heal,
        DamageUp,
        specialAttackGauge
    }
    private void Update()
    {
        switch (Buff_name)
        {
            case BuffType.Freeze:
                // 정지 효과 구현
                break;
            case BuffType.Heal:
                // 체력 회복 효과 구현
                break;
            case BuffType.DamageUp:
                // 데미지 증가 효과 구현
                break;
            case BuffType.specialAttackGauge:
                // 특수 공격 게이지 증가 구현
                break;
        }
        transform.Translate(Vector2.down * 2f * Time.deltaTime);
        if (Time.timeScale == 0)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "player")
        {
            GameManager.Instance.BuffCount++;
            switch (Buff_name)
            {
                case BuffType.Freeze:
                    GameManager.Instance.FreezeSkillTime = 0;
                    GameManager.Instance.Freeze = true;
                    break;
                case BuffType.Heal:
                    if (GameManager.Instance.PlayerLifeMax > GameManager.Instance.PlayerLife)
                    {
                        GameManager.Instance.PlayerLife++;
                    }
                    break;
                case BuffType.DamageUp:
                    GameManager.Instance.BulletDamage += 0.5f;
                    break;
                case BuffType.specialAttackGauge:
                    // 특수 공격 게이지 증가 구현
                    break;
            }
            Debug.Log(Buff_name);

            if (GameManager.Instance.buffsQueue.Count < 50)
            {
                GameManager.Instance.buffsQueue.Enqueue(Buff_name.ToString());
            }
            gameObject.SetActive(false);
        }
    }
}
