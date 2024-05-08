using System.Collections;
using UnityEngine;

public class BOSS : MonoBehaviour
{
    public GameManager GameManager;
    public poolManager PoolManager;
    public Types BOSSType;

    private int count = 0;
    private bool B;

    public enum Types
    {
        Crepe,
        Drill
    }

    private enum Pattern
    {
        Nomal,
        Barrier,
        Ultimate
    }

    private void OnEnable()
    {
        GameManager = FindObjectOfType<GameManager>();
        PoolManager = FindObjectOfType<poolManager>();
        count = 0;
    }

    void Crepe()
    {

        if (count >= 30)
        {
            count = 0;
        }
        else
        {
            if (count <= 5)
            {
                CrepePattern1();
                B = !B;
            }
            else if (count == 15)
            {
                CrepePattern2();
            }
            else if (count == 25)
            {
                CrepePattern3();
            }
            count++;
        }


    }

    void CrepePattern1()
    {
        Debug.Log("Crepe Pattern 1");

        // 총알 생성 및 위치 설정
        if (B)
        {
            // 총알 위치 배열
            Vector2[] bulletPositions = new Vector2[]
            {
                new Vector2(-1.9f, 2f),
                new Vector2(-0.64f, 2f),
                new Vector2(0.64f, 2f),
                new Vector2(1.9f, 2f)
            };

            // 총알 생성 및 위치 설정
            for (int i = 0; i < bulletPositions.Length; i++)
            {
                GameObject bullet = PoolManager.Get(11); // 총알 가져오기
                bullet.transform.position = bulletPositions[i]; // 총알 위치 설정
            }
        }
        else
        {
            // 총알 위치 배열
            Vector2[] bulletPositions = new Vector2[]
            {
                new Vector2(-2.5f, 2f),
                new Vector2(-1.25f, 2f),
                new Vector2(0f, 2f),
                new Vector2(1.25f, 2f),
                new Vector2(2.5f, 2f)
            };

            // 총알 생성 및 위치 설정
            for (int i = 0; i < bulletPositions.Length; i++)
            {
                GameObject bullet = PoolManager.Get(11); // 총알 가져오기
                bullet.transform.position = bulletPositions[i]; // 총알 위치 설정
            }
        }

    }

    void CrepePattern2()
    {
        // 두 번째 패턴: 다른 공격 패턴 구현
        Debug.Log("Crepe Pattern 2");

        Vector2[] bulletPositions = new Vector2[]
            {
                new Vector2(-2.4f, 2.0f),
                new Vector2(-1.6f, 2.0f),
                new Vector2(-0.8f, 2.0f),
                new Vector2(0f, 2.0f),
                new Vector2(0.8f, 2.0f),
                new Vector2(1.6f, 2.0f),
                new Vector2(2.4f, 2.0f)
            };

        // 총알 생성 및 위치 설정
        for (int i = 0; i < bulletPositions.Length; i++)
        {
            GameObject bullet = PoolManager.Get(12); // 총알 가져오기
            bullet.transform.position = bulletPositions[i]; // 총알 위치 설정
        }
    }

    void CrepePattern3()
    {
        // 세 번째 패턴: 다른 공격 패턴 구현
        Debug.Log("Crepe Pattern 3");
    }
}
