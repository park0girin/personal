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

    private void OnEnable()
    {
        GameManager = FindObjectOfType<GameManager>();
        PoolManager = FindObjectOfType<poolManager>();
    }

    void Crepe()
    {
        if (count >= 30)
        {
            count = 0;
        }
        else
        {
            if (count < 5)
            {
                CrepePattern1();
                B = !B;
            }
            else if (count < 15)
            {
                CrepePattern2();
            }
            else if (count < 25)
            {
                CrepePattern3();
            }

            Debug.Log(count);
            count++;
        }
    }

    void CrepePattern1()
    {
        Debug.Log("Crepe Pattern 1");

        // �Ѿ� ���� �� ��ġ ����
        if (B)
        {
            // �Ѿ� ��ġ �迭
            Vector2[] bulletPositions = new Vector2[]
            {
                new Vector2(-1.9f, 4f),
                new Vector2(-0.64f, 4f),
                new Vector2(0.64f, 4f),
                new Vector2(1.9f, 4f)
            };

            // �Ѿ� ���� �� ��ġ ����
            for (int i = 0; i < bulletPositions.Length; i++)
            {
                GameObject bullet = PoolManager.Get(11); // �Ѿ� ��������
                bullet.transform.position = bulletPositions[i]; // �Ѿ� ��ġ ����
            }
        }
        else
        {
            // �Ѿ� ��ġ �迭
            Vector2[] bulletPositions = new Vector2[]
            {
                new Vector2(-2.5f, 4f),
                new Vector2(-1.25f, 4f),
                new Vector2(0f, 4f),
                new Vector2(1.25f, 4f),
                new Vector2(2.5f, 4f)
            };

            // �Ѿ� ���� �� ��ġ ����
            for (int i = 0; i < bulletPositions.Length; i++)
            {
                GameObject bullet = PoolManager.Get(11); // �Ѿ� ��������
                bullet.transform.position = bulletPositions[i]; // �Ѿ� ��ġ ����
            }
        }

    }

    void CrepePattern2()
    {
        // �� ��° ����: �ٸ� ���� ���� ����
        Debug.Log("Crepe Pattern 2"); // �ٸ� ������ ����
    }

    void CrepePattern3()
    {
        // �� ��° ����: �ٸ� ���� ���� ����
        Debug.Log("Crepe Pattern 3"); // �ٸ� ������ ����
    }
}
