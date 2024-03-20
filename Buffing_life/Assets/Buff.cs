using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    public BuffType Buff_name;
    public enum BuffType
    {
        RedExplosion,
        GreenInvincibility,
        BlueFreeze,
        Heal,
        MaxHealth,
        DamageIncrease,
        BulletIncrease,
        Penetration
    }
    private void Update()
    {
        switch (Buff_name)
        {
            case BuffType.RedExplosion:
                // 폭발 효과 구현
                break;
            case BuffType.GreenInvincibility:
                // 무적 효과 구현
                break;
            case BuffType.BlueFreeze:
                // 정지 효과 구현
                break;
            case BuffType.Heal:
                // 체력 회복 효과 구현
                break;
            case BuffType.MaxHealth:
                // 최대 체력 증가 효과 구현
                break;
            case BuffType.DamageIncrease:
                // 데미지 증가 효과 구현
                break;
            case BuffType.BulletIncrease:
                // 탄막 수 증가 효과 구현
                break;
            case BuffType.Penetration:
                // 관통 효과 구현
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
                case BuffType.RedExplosion:
                    GameManager.Instance.RedBullet = true;
                    GameManager.Instance.RedArea += 0.05f;
                    break;
                case BuffType.GreenInvincibility:
                    // 무적 효과 구현
                    break;
                case BuffType.BlueFreeze:
                    GameManager.Instance.Freeze = true;
                    break;
                case BuffType.Heal:
                    if (GameManager.Instance.PlayerLifeMax > GameManager.Instance.PlayerLife)
                    {
                        GameManager.Instance.PlayerLife++;
                    }
                    break;
                case BuffType.MaxHealth:
                    GameManager.Instance.PlayerLifeMax++;
                    break;
                case BuffType.DamageIncrease:
                    GameManager.Instance.BulletDamage += 0.5f;
                    break;
                case BuffType.BulletIncrease:
                    // 탄막 수 증가 효과 구현
                    break;
                case BuffType.Penetration:
                    GameManager.Instance.Penetration = true;
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
