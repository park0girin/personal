using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Debug_Manager : MonoBehaviour
{
    public GameManager GameManager;
    public TextMeshProUGUI TestText;
    public TMP_InputField LVField;
    public TMP_InputField DamageField;

    private void Start()
    {
        // 게임 매니저의 레벨과 데미지를 초기화
        LVField.text = GameManager.Level.ToString();
        DamageField.text = GameManager.BulletDamage.ToString("0.0");
    }

    private void Update()
    {
        // 테스트용 텍스트 업데이트
        TestText.text = GameManager.BulletDamage.ToString();
    }

    public void BOSS_Button()
    {
        // 보스 버튼을 클릭할 때 레벨을 5씩 증가
        if (GameManager.Level % 5 != 0)
        {
            GameManager.Level += 5 - (GameManager.Level % 5);
            LVField.text = GameManager.Level.ToString();
        }
    }

    public void Damage_Button()
    {
        // 데미지 버튼을 클릭할 때 데미지를 증가
        GameManager.BulletDamage += (ScenesManager.Instance.BulletDamage / 5);
        DamageField.text = GameManager.BulletDamage.ToString("0.0");
    }

    public void MaxHP()
    {
        GameManager.PlayerLife = GameManager.MaxHP;
        GameManager.HPGaugeBar.ChangeGaugeValue(GameManager.MaxHP);
    }

    public void LVInput()
    {
        // LVInput 함수: LVField 값이 변경될 때 호출되는 함수
        int level;
        if (int.TryParse(LVField.text, out level))
        {
            GameManager.Level = level;
        }
        else
        {
            Debug.LogWarning("Invalid level input");
        }
    }

    public void DamageInput()
    {
        // DamageInput 함수: DamageField 값이 변경될 때 호출되는 함수
        float damage;
        if (float.TryParse(DamageField.text, out damage))
        {
            GameManager.BulletDamage = damage;
        }
        else
        {
            Debug.LogWarning("Invalid damage input");
        }
    }
}
