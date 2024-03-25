using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public poolManager pool;
    public GameObject GameOverUI;
    public GameObject StageClearUI;
    public GameObject P_BT;

    //player
    public GameObject Player;
    public float ShotInterval = 0.5f;
    public int PlayerLifeMax = 3;
    public int PlayerLife = 3;

    //bullet
    public bool Penetration = false;
    public float BulletDamage = 1.0f;
    public bool RedBullet = false;
    public float RedArea;

    //mob
    public float SpawnSpeed = 1.0f;
    public bool BossBattle;
    public float BOSS_hp;
    int randomMob;

    // buff
    public int BuffCount;
    int RandomBuff;
    public Queue<string> buffsQueue = new Queue<string>();
    Dictionary<string, int> buffCountDict = new Dictionary<string, int>();
    public bool Freeze;

    // UI
    public float Level;
    public TextMeshProUGUI LV_text;
    public TextMeshProUGUI UI_Text;
    public TextMeshProUGUI Buff_Text;
    public bool GameOver;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Reset();
    }

    public float gameTime;
    private void Update()
    {
        UI_Text.text = ($"Life : {PlayerLife} / {PlayerLifeMax}\nBuff : {BuffCount} / 20");
        if (GameOver)
        {
            GameOverUI.SetActive(true);
        }
        else if(!BossBattle)
        {
            if (BuffCount >= 20)
            {
                GameObject BOSS = pool.Get(Random.Range(4, 6));
                BOSS.transform.position
                    = new Vector2(0f, 10f);
                Debug.Log("BOSS");
                BossBattle = true;
            }
            else
            {
                gameTime += Time.deltaTime;
                if (gameTime >= (2.5 / SpawnSpeed))
                {
                    SpawnSpeed += Level / 50;
                    randomMob = Random.Range(0, 4);
                    GameObject Mob = pool.Get(randomMob);
                    Mob.transform.position
                        = new Vector2(Random.Range(-2f, 2f), 6f);
                    gameTime = 0;
                }
                if (BuffCount != 0)
                {
                    while (buffsQueue.Count > 0)
                    {
                        string buffName = buffsQueue.Dequeue();

                        if (!buffCountDict.ContainsKey(buffName))
                        {
                            buffCountDict[buffName] = 1;
                        }
                        else
                        {
                            buffCountDict[buffName]++;
                        }
                    }

                    Buff_Text.text = "Buff : \n";
                    foreach (var kvp in buffCountDict)
                    {
                        string buffInfo = $"{kvp.Key} x{kvp.Value}\n";
                        Buff_Text.text += buffInfo;
                    }
                }

            }

            if (Level <= 5)
            {
                LV_text.text = ("LV " + Level.ToString()); 
                LV_text.color = Color.white;
            }
            else
            {
                LV_text.text = ("LV 5+");
                LV_text.color = new Color(1.0f, 0.5f, 0.0f);
            }
        }
        else if (BossBattle)
        {
            switch (Level)
            {
                case 1:
                    //??
                    break;
                case 2:
                    //??
                    break;
                case 3:
                    //??
                    break;
                case 4:
                    //??
                    break;
                case 5:
                    //??
                    break;
            }
        }
    }

    public void GamePause()
    {
        Time.timeScale = 0f;
        P_BT.SetActive(true);
    }
    public void Gamecontinue()
    {
        Time.timeScale = 1f;
        P_BT.SetActive(false);
    }
    public void Next()
    {
        Level++;
        Reset();
        Gamecontinue();
    }
    public void Re()
    {
        Reset();
        Gamecontinue();
    }
    private void Reset()
    {
        switch (Level)
        {
            case 1:
                BOSS_hp = 15.0f;
                break;
            case 2:
                BOSS_hp = 30.0f;
                break;
            case 3:
                BOSS_hp = 50.0f;
                break;
            case 4:
                BOSS_hp = 90.0f;
                break;
            case 5:
                BOSS_hp = 200.0f;
                break;
            default:
                BOSS_hp += BOSS_hp / 2;
                break;
        }
        P_BT.SetActive(false);
        Freeze = false;
        PlayerLifeMax = 3;
        PlayerLife = 3;
        Player.SetActive(true);
        GameOver = false;
        BulletDamage = 1;
        BuffCount = 0;
        BossBattle = false;
        gameTime = 0;
        GameOverUI.SetActive(false);
        StageClearUI.SetActive(false);
        buffsQueue.Clear();
        buffCountDict = new Dictionary<string, int>();
        Buff_Text.text = ($"Buff : \n");
        RedArea = 0.2f;
        Time.timeScale = 1.0f;
    }
    public void Boom(Vector2 pos)
    {
        for (int i = 0; i < Random.Range(3, 6); i++)
        {
            GameObject mob = pool.Get(6);

            mob.transform.position = pos;
            mob.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
            mob.SetActive(true);
        }

        gameTime = 0;
    }
    public void MakeBuff(Vector2 pos)
    {
        RandomBuff = Random.Range(7, 15);
        GameObject Buff = pool.Get(RandomBuff);
        Buff.transform.position = pos;
    }
    public void RedBulletBoom(Vector2 pos)
    {
        Debug.Log("BOOM");
        GameObject boom= pool.Get(15);
        boom.transform.position = pos;
    }
}
