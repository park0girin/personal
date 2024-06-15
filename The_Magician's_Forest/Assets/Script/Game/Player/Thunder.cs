using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : MonoBehaviour
{
    public GameObject plPos;
    public BoxCollider2D Box;
    public bool Sk;
    public float SkTime;

    public void OnEnable()
    {
        Box = GetComponent<BoxCollider2D>();
        Sk = false;
        SkTime = 0;
        if (Box != null) Box.enabled = false;
        if (plPos == null)
        {
            plPos = GameObject.Find("bulletPos");
        }
    }

    private void Update()
    {
        if (plPos != null)
        {
            this.gameObject.transform.position = plPos.transform.position + new Vector3(0, 4f, 0);
        }
        else
        {
            Debug.LogError("plPos가 할당되지 않았습니다! 위치를 설정할 수 없습니다.");
            return;
        }
        if (Sk)
        {
            Box.enabled = true;
            SkTime += Time.deltaTime;
            if (SkTime >= 0.05f)
            {
                Box.enabled = false;
                Sk = false;
                Debug.Log("ThunderSkil");
            }
        }
    }
    public void Thunder_True()
    {
        if (Box != null) Box.enabled = true;
    }
    public void Thunder_False()
    {
        if (Box != null) Box.enabled = false;
    }
    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
    public void ThunderSkil()
    {
        Sk = true;
        SkTime = 0;
    }
}
