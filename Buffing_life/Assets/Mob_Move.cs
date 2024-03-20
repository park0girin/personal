using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob_Move : MonoBehaviour
{
    public mobType mob;
    public float speed_input;
    float speed;
    public float skillTime;
    public float hp_input;
    public float hp;
    bool specialSkill;
    bool cannotBeHit;
    sprite_change ChangeScript;

    public enum mobType
    {
        Red,
        Green,
        Blue,
        White,
        BOSS
    }
    float Ran(float max, float min)
    {
        float value = Random.Range(max, min);
        return value;
    }
    private void OnEnable()
    {
        if (mob==mobType.BOSS)
        {
            hp = GameManager.Instance.BOSS_hp;
        }
        else hp = hp_input;
        speed = Ran(speed_input + 1, speed_input - 1);
        specialSkill = false;
        skillTime = 0;
        if (mob == mobType.White)
        {
            ChangeScript = GetComponent<sprite_change>();
        }
    }
    void Green()
    {
        cannotBeHit = false;
        if (!specialSkill)
        {
            transform.Translate(Vector2.down * speed * Time.deltaTime);
        }
        else if(specialSkill)
        {
            skillTime += Time.deltaTime;
            if (skillTime > 2f)
            {
                cannotBeHit = false;
                specialSkill =! specialSkill;
            }
        }
    }
    void Red()
    {
        if (!specialSkill)
        {
            transform.Translate(Vector2.down * speed * Time.deltaTime);
        }
        if (transform.position.y < 4)
        {
            skillTime += Time.deltaTime;
            if (skillTime > Ran(8, 4))
            {
                specialSkill = true;
            }
            if (specialSkill)
            {
                Vector2 pos = transform.position;
                GameManager.Instance.Boom(pos);
                gameObject.SetActive(false);
            }
        }
    }
    void Blue()
    {
        float minX = -2.5f;
        float maxX = 2.5f;
        float minY = -4f;
        float maxY = 4f;

        if (transform.position.y <= maxY && transform.position.y >= minY)
        {
            float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
            transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);

            if (!specialSkill)
            {
                skillTime += Time.deltaTime;
                if (skillTime >= 1f)
                {
                    transform.Rotate(0, 0, Ran(360f, 0f));
                    specialSkill = true;
                }
            }
            else
            {
                skillTime -= Time.deltaTime;
                if (skillTime <= 0.9f)
                {
                    transform.Rotate(0, 0, 0);
                    skillTime = 0;
                    specialSkill = false;
                }
                else
                {
                    transform.Translate(Vector2.down * speed * 10f * Time.deltaTime);
                }
            }
        }
        else
        {
            transform.rotation = Quaternion.identity;
            transform.Translate(Vector2.down * speed * Time.deltaTime);
        }
    }
    void White()
    {
        if (!specialSkill)
        {
            transform.Translate(Vector2.down * speed * Time.deltaTime);
            if (hp <= (hp_input / 2))
            {
                specialSkill = true;
            }
        }
        if (specialSkill)
        {
            ChangeScript.Change();
            transform.Translate(Vector2.down * speed * 10 * Time.deltaTime);
        }
    }
    void BOSS()
    {
        if (transform.position.y >= 7)
        {
            transform.Translate(Vector2.down * speed * Time.deltaTime);
        }
    }
    private void Update()
    {
        if (GameManager.Instance.GameOver)
        {
            gameObject.SetActive(false);
        }
        else if (hp <= 0)
        {
            if (mob == mobType.BOSS) GameManager.Instance.StageClearUI.SetActive(true);
            specialSkill = false;
            Vector2 pos = transform.position;
            GameManager.Instance.MakeBuff(pos);
            gameObject.SetActive(false);
        }
        else
        {
            if (!GameManager.Instance.Freeze)
            {
                switch (mob)
                {
                    case mobType.Green:
                        Green();
                        break;
                    case mobType.Red:
                        Red();
                        break;
                    case mobType.Blue:
                        Blue();
                        break;
                    case mobType.White:
                        White();
                        break;
                    case mobType.BOSS:
                        BOSS();
                        break;
                }
                if (GameManager.Instance.BossBattle && mob != mobType.BOSS)
                {
                    gameObject.SetActive(false);
                    return;
                }
            }
            else
            {
                skillTime += Time.deltaTime;
                if(skillTime > 2.0f)
                {
                    GameManager.Instance.Freeze = false;
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(mob == mobType.Green && collision.CompareTag("bullet"))
        {
            specialSkill = true;
        }
        if (mob == mobType.Red && transform.position.y < 4)
        {
            specialSkill = true;
        }
        if (collision.CompareTag("bullet"))
        {
            if (!GameManager.Instance.Penetration)
            {
                collision.gameObject.SetActive(false);
            }
            if(!cannotBeHit) hp -= GameManager.Instance.BulletDamage;
        }
    }
}
