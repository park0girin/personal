using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mob_Move : MonoBehaviour
{
    public GameManager GameManager;
    public PoolManager PoolManager;
    public mobType mob;
    public float speed_input;
    float speed;
    public float skillTime;
    public float hp_input;
    public float hpmax;
    public float hp;
    public bool specialSkill;
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
        GameManager = FindObjectOfType<GameManager>();
        PoolManager = FindObjectOfType<PoolManager>();
        hpmax = hp_input * GameManager.HpMultiplier;
        hp = hpmax;
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
        if (!specialSkill)
        {
            skillTime = 0;
            transform.Translate(Vector2.down * speed * Time.deltaTime);
        }
        else if (specialSkill)
        {
            if (skillTime == 0)
            {
                GameObject gb = PoolManager.Get(14);
                gb.transform.position = this.transform.position;
                gb.SetActive(true);
            }
            skillTime += Time.deltaTime;
            if (skillTime > 2f)
            {
                Debug.Log("specialSkill false");
                specialSkill = !specialSkill;
            }
        }
    }
    void Red()
    {
        if (!specialSkill)
        {
            transform.Translate(Vector2.down * speed * Time.deltaTime);
        }
        skillTime += Time.deltaTime;
        if (skillTime > Ran(5, 2))
        {
            specialSkill = true;
        }
        if (specialSkill)
        {
            Vector2 pos = transform.position;
            GameManager.Boom(pos);
            gameObject.SetActive(false);
        }
    }
    void Blue()
    {
        float minX = -2.5f;
        float maxX = 2.5f;
        float minY = -4f;
        float maxY = 4f;

        if (transform.position.y < maxY && transform.position.y > minY)
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
                transform.Rotate(Vector2.zero);
                skillTime -= Time.deltaTime;
                if (skillTime <= 0.9f)
                {
                    skillTime = 0;
                    specialSkill = false;
                }
                else
                {
                    transform.Translate(Vector2.down * speed * 5f * Time.deltaTime);
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
        if (hp <= (hp_input / 2))
        {
            if (!specialSkill)
            {
                specialSkill = true;
            }
            if (specialSkill)
            {
                ChangeScript.Change();
                transform.Translate(Vector2.down * 10 * Time.deltaTime);
            }
        }
        else transform.Translate(Vector2.down * speed * Time.deltaTime);
    }
    void BOSS()
    {
        if (transform.position.y >= 3)
        {
            transform.Translate(Vector2.down * speed * Time.deltaTime);
        }
        else specialSkill = true;
    }
    private void Update()
    {
        if (GameManager.GameOver)
        {
            hp = 0;
            hpmax = hp_input;
            gameObject.SetActive(false);
        }
        else if (hp <= 0)
        {
            specialSkill = false;
            Vector2 pos = transform.position;
            if (mob == mobType.BOSS)
            {
                GameManager.BossBattle = false;
                GameManager.Level++;
                GameManager.Gamecontinue();
            }
            else
            {
                GameManager.MobCount++;
                GameManager.MakeBuff(pos);
            }
            gameObject.SetActive(false);
        }
        else
        {
            if (!GameManager.Freeze)
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
                if (GameManager.BossBattle && mob != mobType.BOSS)
                {
                    gameObject.SetActive(false);
                    return;
                }
            }
            else
            {
                GameManager.FreezeSkillTime += Time.deltaTime;
                if (GameManager.FreezeSkillTime > 2.0f)
                {
                    GameManager.Freeze = false;
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.transform.position.y <= 5)
        {
            if (mob == mobType.Green && (collision.CompareTag("bullet") || collision.CompareTag("Thunder")))
            {
                specialSkill = true;
            }
            if (mob == mobType.Red && (!collision.CompareTag("BG") || !collision.CompareTag("BUFF")))
            {
                specialSkill = true;
            }
            if (mob == mobType.Blue && (collision.CompareTag("bullet") || collision.CompareTag("Thunder")))
            {
                hp = 0;
            }
            if (collision.CompareTag("bullet"))
            {
                collision.gameObject.SetActive(false);
                if (!(mob == mobType.BOSS && transform.position.y > 4))
                {
                    hp -= GameManager.BulletDamage;
                }
            }
            if (collision.CompareTag("Thunder"))
            {
                if (!(mob == mobType.BOSS && transform.position.y > 4))
                {
                    hp -= GameManager.BulletDamage;
                }
            }
        }
    }
}