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
        // ���� �Ŵ����� ������ �������� �ʱ�ȭ
        LVField.text = GameManager.Level.ToString();
        DamageField.text = GameManager.BulletDamage.ToString("0.0");
    }

    private void Update()
    {
        // �׽�Ʈ�� �ؽ�Ʈ ������Ʈ
        TestText.text = GameManager.BulletDamage.ToString();
    }

    public void BOSS_Button()
    {
        // ���� ��ư�� Ŭ���� �� ������ 5�� ����
        if (GameManager.Level % 5 != 0)
        {
            GameManager.Level += 5 - (GameManager.Level % 5);
            LVField.text = GameManager.Level.ToString();
        }
    }

    public void Damage_Button()
    {
        // ������ ��ư�� Ŭ���� �� �������� ����
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
        // LVInput �Լ�: LVField ���� ����� �� ȣ��Ǵ� �Լ�
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
        // DamageInput �Լ�: DamageField ���� ����� �� ȣ��Ǵ� �Լ�
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
