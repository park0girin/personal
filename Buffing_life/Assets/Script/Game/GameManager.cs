using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public poolManager pool;
    public GameObject GameOverUI;
    public GameObject P_BT;
    public GameObject Black;
    public TextMeshPro testtext;

    //player
    public GameObject Player;
    public int PlayerLife = 3;

    //bullet
    public float BulletDamage;
    public bool Penetration = false;
    public bool RedBullet = false;

    //mob
    public float SpawnSpeed = 1.0f;
    public bool BossBattle;
    public float HpMultiplier = 1.1f ;
    int randomMob;

    // buff
    public int BuffCount;
    int RandomBuff;
    public Queue<string> buffsQueue = new Queue<string>();
    Dictionary<string, int> buffCountDict = new Dictionary<string, int>();
    public bool Freeze;
    public float FreezeSkillTime;
    public float SkillCountMax;
    public int SkillCount;

    // UI
    public float Level;
    public TextMeshProUGUI LV_text;
    public TextMeshProUGUI UI_Text;
    public TextMeshProUGUI Buff_Text;
    public PlayerUI_con HPGaugeBar;
    public PlayerUI_con SkillGaugeBar;
    public Button myButton;

    // 상태
    public bool GameOver;
    public bool Gameing;

    private void OnEnable()
    {
        Reset();
    }

    public float gameTime;
    private void Update()
    {
        UI_Text.text = ($"Life : {PlayerLife} / {ScenesManager.Instance.PlayerLifeMax}\nBuff : {((Level % 5) - 1) * 10 + BuffCount} / 50");
        testtext.text = ("Damage : " + BulletDamage);
        if (GameOver)
        {
            Black.SetActive(true);
            HPGaugeBar.ChangeGaugeValue(0);
            GameOverUI.SetActive(true);
        }
        else
        {
            if (!BossBattle)
            {
                if (Level % 5 == 0)
                {
                    //GameObject BOSS = pool.Get(Random.Range(4, 6));
                    GameObject BOSS = pool.Get(4);
                    BOSS.transform.position
                        = new Vector2(0f, 7f);
                    Debug.Log("BossBattle");
                    BossBattle = true;
                }
                else
                {
                    if (BuffCount >= 10)
                    {
                        BuffCount = 0;
                        Debug.Log("Level up");
                        Level += 1;
                    }
                    else
                    {
                        gameTime += Time.deltaTime;
                        if (gameTime >= (2.5 / SpawnSpeed))
                        {
                            if(SpawnSpeed < 5) SpawnSpeed += Level / 50;
                            else SpawnSpeed = 5;
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

                }

                if (Level / 5 == 0)
                {
                    LV_text.text = ("LV " + Level.ToString());
                    LV_text.color = new Color(1.0f, 0.5f, 0.0f);
                }
                else
                {
                    LV_text.text = ("LV " + Level.ToString());
                    LV_text.color = new Color(0.7f, 1.0f, 0.5f);
                }

                if (SkillCount >= SkillCountMax)
                {
                    ActivateButton();
                }
            }
            else if (BossBattle)
            {

            }
        }
    }

    public void GamePause()
    {
        Gameing = false;
        Time.timeScale = 0f;
        P_BT.SetActive(true);
        Black.SetActive(true);
    }
    public void Gamecontinue()
    {
        Gameing = true;
        Time.timeScale = 1f;
        P_BT.SetActive(false);
        Black.SetActive(false);
    }
    public void Next()
    {
        Level += 1;
        Gamecontinue();
    }
    public void Re()
    {
        Reset();
        Gamecontinue();
    }
    private void Reset()
    {
        Level = 1;
        Black.SetActive(false);
        P_BT.SetActive(false);
        Freeze = false;
        FreezeSkillTime = 0;
        PlayerLife = ScenesManager.Instance.PlayerLifeMax;
        Player.SetActive(true);
        HPGaugeBar.maxGaugeValue = ScenesManager.Instance.PlayerLifeMax;
        HPGaugeBar.ChangeGaugeValue(ScenesManager.Instance.PlayerLifeMax);
        DeactivateButton();
        Gameing = true;
        GameOver = false;
        BulletDamage = ScenesManager.Instance.BulletDamage;
        BuffCount = 0;
        SpawnSpeed = 1; 
        BossBattle = false;
        gameTime = 0;
        GameOverUI.SetActive(false);
        buffsQueue.Clear();
        buffCountDict = new Dictionary<string, int>();
        Buff_Text.text = ($"Buff : \n");
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
        RandomBuff = Random.Range(7, 11);
        GameObject Buff = pool.Get(RandomBuff);
        Buff.transform.position = pos;
    }
    public void ActivateButton()
    {
        myButton.interactable = true; // 버튼을 활성화합니다.
        myButton.image.color = new Color(0.3f, 0.9f, 0.5f);
    }

    // 버튼 비활성화 함수
    public void DeactivateButton()
    {
        if (Gameing)
        {
            SkillCount = 0;
            SkillGaugeBar.ChangeGaugeValue(SkillCount);
            myButton.image.color = new Color(0.8f, 0.8f, 0.8f, 0.5f);
            myButton.interactable = false; // 버튼을 비활성화합니다.
        }
    }
}
