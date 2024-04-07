using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOSS : MonoBehaviour
{
    public GameManager GameManager;
    public poolManager PoolManager;
    public GameObject inobj1;
    public GameObject inobj2;
    public GameObject inobj3;
    public GameObject inobj4;
    public GameObject inobj5;
    public Types BOSSType;

    public enum Types
    {
        Crepe,
        Drill
    }
    private void OnEnable()
    {
        GameManager = FindObjectOfType<GameManager>();
        PoolManager = FindObjectOfType<poolManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void Crepe()
    {
        int RandomAtack = Random.Range(0, 3);
        switch (RandomAtack)
        {
            case 0:
                bool b = true;
                Crepe_1(b);
                break;
            case 1:
                Crepe_2();
                break;
            case 2:
                Crepe_3();
                break;
        }
    }

    void Crepe_1(bool bullet)
    {
        Debug.Log("Crepe_1");
        for (int i = 0; i < 3; i++)
        {
            if (bullet)
            {
                // 프리팹 11을 풀로부터 가져오기
                GameObject b5 = PoolManager.Get(11);
                b5.transform.position = new Vector2(0f, -1f);
                bullet = !bullet;
            }
            else if (!bullet)
            {
                // 프리팹 12를 풀로부터 가져오기
                GameObject b6 = PoolManager.Get(12);
                b6.transform.position = new Vector2(0f, -1f);
                bullet = !bullet;
            }
        }
    }
    void Crepe_2()
    {
        Debug.Log("Crepe_2");
    }
    void Crepe_3()
    {
        Debug.Log("Crepe_3");
    }
}
