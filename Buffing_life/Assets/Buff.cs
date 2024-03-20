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
                // ���� ȿ�� ����
                break;
            case BuffType.GreenInvincibility:
                // ���� ȿ�� ����
                break;
            case BuffType.BlueFreeze:
                // ���� ȿ�� ����
                break;
            case BuffType.Heal:
                // ü�� ȸ�� ȿ�� ����
                break;
            case BuffType.MaxHealth:
                // �ִ� ü�� ���� ȿ�� ����
                break;
            case BuffType.DamageIncrease:
                // ������ ���� ȿ�� ����
                break;
            case BuffType.BulletIncrease:
                // ź�� �� ���� ȿ�� ����
                break;
            case BuffType.Penetration:
                // ���� ȿ�� ����
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
                    // ���� ȿ�� ����
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
                    // ź�� �� ���� ȿ�� ����
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
