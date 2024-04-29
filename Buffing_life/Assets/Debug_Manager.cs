using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Debug_Manager : MonoBehaviour
{ 
    public GameManager GameManager;
    public TextMeshProUGUI TestText;
    private void Start()
    {
        
    }
    private void Update()
    {
        TestText.text = "BulletDamage : " + GameManager.BulletDamage.ToString();
    }
    public void BOSS_Button()
    {
        Debug.Log("BOSSBUTTON");
        if (GameManager.Level % 5 != 0)
        {
            GameManager.Level += 5 - (GameManager.Level % 5);
        }
    }
    public void Damage_Button()
    {
        GameManager.BulletDamage += (ScenesManager.Instance.BulletDamage / 5);
    }
}
