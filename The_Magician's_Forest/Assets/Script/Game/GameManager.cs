using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ScenesManager;

public class GameManager : MonoBehaviour
{
    public PoolManager pool;
    public GameObject GameOverUI;
    public GameObject GamePauseUI;
    public GameObject P_BT;
    public GameObject Black;

    //player
    public GameObject Player;
    public int PlayerLife = 3;
    public int MaxHP;

    //bullet
    public float BulletDamage;
    //public bool Penetration = false;
    public bool RedBullet = false;

    //mob
    public float SpawnSpeed = 1.0f;
    public bool BossBattle;
    public float HpMultiplier = 1.1f ;
    int randomMob;

    // buff
    public int BuffCount;
    public int MobCount;
    public Queue<string> buffsQueue = new Queue<string>();
    Dictionary<string, int> buffCountDict = new Dictionary<string, int>();
    public bool Freeze;
    public float FreezeSkillTime;
    public int SkillCountMax;
    public int SkillCount;

    // UI
    public float Level;
    public TextMeshProUGUI LV_text;
    //public TextMeshProUGUI UI_Text;
    //public TextMeshProUGUI Buff_Text;
    public PlayerUI_con HPGaugeBar;
    public PlayerUI_con SkillGaugeBar;
    public Button myButton;

    // Debug UI
    public GameObject DebugUI;

    // 상태
    public bool GameOver;
    public bool Gameing;
    public bool Debuging;

    public BulletType BulletTypes;
    public float UpDamage;
    public float Damage;

    private void OnEnable()
    {
        BulletTypes = ScenesManager.Instance.BulletTypes;
        switch (BulletTypes)
        {
            case BulletType.Ice:
                UpDamage = ScenesManager.Instance.UpDamage_Ice;
                break;
            case BulletType.Fire:
                UpDamage = ScenesManager.Instance.UpDamage_Fire;
                break;
            case BulletType.Thunder:
                UpDamage = ScenesManager.Instance.UpDamage_Thunder;
                break;
            case BulletType.Wind:
                UpDamage = ScenesManager.Instance.UpDamage_Wind;
                break;
            default:
                BulletTypes = BulletType.Ice;
                UpDamage = ScenesManager.Instance.UpDamage_Ice;
                break;
        }
        MaxHP = ScenesManager.Instance.PlayerLifeMax;
        Reset();
    }

    public float gameTime;
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.D))
        {
            Debuging =! Debuging;
            if (Debuging)
            {
                Time.timeScale = 0f;
                Gameing = false;
                Black.SetActive(true);
            }
            else
            {
                Gamecontinue();
            }
            DebugUI.SetActive(Debuging);
        }

        //UI_Text.text = ($"Life : {PlayerLife} / {ScenesManager.Instance.PlayerLifeMax}\nMob : {(Level % 5 == 0 ? 0 : ((Level % 5) - 1) * 10 + MobCount)} / 50");
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
                    GameObject BOSS = pool.Get(5);
                    BOSS.transform.position = new Vector2(0f, 7f);
                    BossBattle = true;
                    if (SpawnSpeed < 5) SpawnSpeed += 0.5f;
                    else SpawnSpeed = 5;
                }
                else
                {
                    if (MobCount >= 10)
                    {
                        MobCount = 0;
                        Level += 1;
                        HpMultiplier = Mathf.Pow(1.2f, Level);

                    }
                    else
                    {
                        gameTime += Time.deltaTime;
                        if (gameTime >= (2.5 / SpawnSpeed))
                        {
                            randomMob = Random.Range(0, 4);
                            GameObject Mob = pool.Get(randomMob);
                            Mob.transform.position = new Vector2(Random.Range(-2f, 2f), 6f);
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

                            /*
                            Buff_Text.text = "Buff : \n";
                            foreach (var kvp in buffCountDict)
                            {
                                string buffInfo = $"{kvp.Key} x{kvp.Value}\n";
                                Buff_Text.text += buffInfo;
                            }
                            */
                        }

                    }

                }
                LV_text.text = ("LV " + Level.ToString());
                LV_text.color = new Color(0.7f, 1.0f, 0.5f);

                if (SkillCount >= SkillCountMax)
                {
                    ActivateButton();
                }
            }
            else
            {
                LV_text.text = ("LV " + Level.ToString());
                LV_text.color = new Color(1.0f, 0.8f, 0.4f);
            }
        }
    }

    public void GamePause()
    {
        Gameing = false;
        Time.timeScale = 0f;
        P_BT.SetActive(true);
        Black.SetActive(true);
        GamePauseUI.SetActive(true);
    }
    public void Gamecontinue()
    {
        Gameing = true;
        Time.timeScale = 1f;
        P_BT.SetActive(false);
        Black.SetActive(false);
        GamePauseUI.SetActive(false);
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
        Debuging = false;
        DebugUI.SetActive(false);
        Level = 1;
        Black.SetActive(false);
        P_BT.SetActive(false);
        Freeze = false;
        FreezeSkillTime = 0;
        PlayerLife = MaxHP;
        Player.SetActive(true);
        HPGaugeBar.maxGaugeValue = MaxHP;
        HPGaugeBar.ChangeGaugeValue(MaxHP);
        DeactivateButton();
        Gameing = true;
        GameOver = false;
        Damage = 1;
        BulletDamage = Damage * ScenesManager.Instance.BulletDamage;
        BuffCount = 0;
        MobCount = 0;
        HpMultiplier = 1;
        SpawnSpeed = 1; 
        BossBattle = false;
        gameTime = 0;
        GameOverUI.SetActive(false);
        GamePauseUI.SetActive(false);
        buffsQueue.Clear();
        buffCountDict = new Dictionary<string, int>();
        //Buff_Text.text = ($"Buff : \n");
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
        if (Random.Range(0, 2) == 0){
            Debug.Log("버프 발생");
            int[] prefabProbabilities;

            if (PlayerLife != 3)
            {
                // HP가 3이 아닌 경우
                prefabProbabilities = new int[] { 20, 30, 30, 20 }; // 각 프리팹 확률 (예시)
            }
            else
            {
                // HP가 3인 경우
                prefabProbabilities = new int[] { 20, 40, 40, 0 }; // 각 프리팹 확률 (예시)
            }

            // 확률에 따라 프리팹을 선택합니다.
            int totalProbability = 0;
            for (int i = 0; i < prefabProbabilities.Length; i++)
            {
                totalProbability += prefabProbabilities[i];
            }

            int randomValue = Random.Range(0, totalProbability); // 0부터 총 확률까지의 랜덤 값 생성

            int prefabIndex = 0;
            int cumulativeProbability = 0;

            // 랜덤 값이 어느 프리팹 확률 범위에 속하는지 찾습니다.
            for (int i = 0; i < prefabProbabilities.Length; i++)
            {
                cumulativeProbability += prefabProbabilities[i];

                if (randomValue < cumulativeProbability)
                {
                    prefabIndex = i + 7; // 인덱스 0부터 시작하므로 7을 더해서 7부터 10 사이의 값을 얻습니다.
                    break;
                }
            }

            // 선택된 프리팹을 풀에서 가져와 위치 설정 후 활성화합니다.
            if (prefabIndex >= 7 && prefabIndex <= 10)
            {
                GameObject Buff = pool.Get(prefabIndex);
                if (Buff != null)
                {
                    Buff.transform.position = pos;
                    Buff.SetActive(true);
                }
                else
                {
                    Debug.LogError("풀에서 프리팹을 가져오지 못했습니다.");
                }
            }
            else
            {
                Debug.LogError("올바른 프리팹 인덱스가 아닙니다.");
            }
        }
        else Debug.Log("버프 발생하지 않음");
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
