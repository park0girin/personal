using System.Collections;
using UnityEngine;

public class BOSS : MonoBehaviour
{
    public GameManager GameManager;
    public poolManager PoolManager;
    public Mob_Move mm; // Add reference to Mob_Move
    public Types BOSSType;
    private Patterns Pattern;
    private float hp;
    private float inhp;
    public bool Cream;

    private bool B;
    private bool Barrier_Active;

    private Vector3 nextPosition;
    public float moveSpeed;

    public enum Types
    {
        Crepe,
        Drill
    }

    private enum Patterns
    {
        Nomal,
        Barrier,
        Aim,
        Ultimate
    }

    private void OnEnable()
    {
        GameManager = FindObjectOfType<GameManager>();
        PoolManager = FindObjectOfType<poolManager>();
        mm = GetComponent<Mob_Move>(); // Ensure mm is assigned

        inhp = mm.hp_input * GameManager.HpMultiplier;
    }

    private void Update()
    {
        hp = mm.hp;
        if (BOSSType == Types.Drill)
        {
            UpdatePattern();
            ExecuteDrillPattern();
        }
    }

    void UpdatePattern()
    {
        if (hp > 75)
        {
            Pattern = Patterns.Barrier;
        }
        else if (hp > 50)
        {
            Pattern = Patterns.Nomal;
        }
        else if (hp > 25)
        {
            Pattern = Patterns.Aim;
        }
        else
        {
            Pattern = Patterns.Ultimate;
        }
    }

    private void ExecuteDrillPattern()
    {
        switch (Pattern)
        {
            case Patterns.Barrier:
                DrillPattern1();
                break;
            case Patterns.Nomal:
                DrillPattern2();
                break;
            case Patterns.Aim:
                DrillPattern3();
                break;
            case Patterns.Ultimate:
                DrillPattern4();
                break;
        }
    }

    private void DrillPattern1()
    {
        // 다음 위치를 향해 목표 위치 계산
        Vector3 targetPosition = Vector3.MoveTowards(transform.position, nextPosition, moveSpeed * Time.deltaTime);

        // 목표 위치를 지정된 범위 내로 제한
        targetPosition = ClampPosition(targetPosition);

        // 보스가 다음 위치에 도달했는지 확인
        if (Vector3.Distance(transform.position, nextPosition) < 0.05f)
        {
            // 지정된 범위 내에서 새로운 위치 설정
            nextPosition = GetRandomPositionWithinBounds();
        }

        // 보스의 위치 업데이트
        transform.position = targetPosition;
    }

    // 지정된 범위 내에서 무작위 위치를 얻는 헬퍼 메서드
    private Vector3 GetRandomPositionWithinBounds()
    {
        float randomX = Random.Range(-1.2f, 1.2f);
        float randomY = Random.Range(0f, 3f);
        return new Vector3(randomX, randomY, transform.position.z);
    }

    // 위치를 지정된 범위 내로 제한하는 헬퍼 메서드
    private Vector3 ClampPosition(Vector3 position)
    {
        float clampedX = Mathf.Clamp(position.x, -1.2f, 1.2f);
        float clampedY = Mathf.Clamp(position.y, 0f, 3f);
        return new Vector3(clampedX, clampedY, position.z);
    }

    private void DrillPattern2()
    {

    }

    private void DrillPattern3()
    {

    }

    private void DrillPattern4()
    {

    }

    void Crepe()
    {
        Debug.Log(hp / inhp);
        Debug.Log(Pattern);
        switch (Pattern)
        {
            case Patterns.Barrier:
                Cream = true;
                CrepePattern2();
                Barrier_Active = false;
                Pattern = Patterns.Ultimate;
                break;
            case Patterns.Ultimate:
                CrepePattern3();
                break;
            case Patterns.Nomal:
                CrepePattern1();
                B = !B;
                break;
        }

        if (hp / inhp <= 0.75f&& !Barrier_Active)
        {
            Barrier_Active = true;
            if (Pattern != Patterns.Ultimate) Pattern = Patterns.Barrier;
        }
    }

    void CrepePattern1()
    {
        //Debug.Log("Crepe Pattern 1");

        Vector2[] bulletPositions = B ? new Vector2[]
        {
            new Vector2(-1.9f, 2f),
            new Vector2(-0.64f, 2f),
            new Vector2(0.64f, 2f),
            new Vector2(1.9f, 2f)
        } : new Vector2[]
        {
            new Vector2(-2.5f, 2f),
            new Vector2(-1.25f, 2f),
            new Vector2(0f, 2f),
            new Vector2(1.25f, 2f),
            new Vector2(2.5f, 2f)
        };

        for (int i = 0; i < bulletPositions.Length; i++)
        {
            GameObject bullet = PoolManager.Get(11);
            bullet.transform.position = bulletPositions[i];
        }
    }

    void CrepePattern2()
    {
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

        for (int i = 0; i < bulletPositions.Length; i++)
        {
            GameObject bullet = PoolManager.Get(12);
            bullet.transform.position = bulletPositions[i];
        }
    }
    void CrepePattern3()
    {
        if (!Cream)
        {
            Pattern = Patterns.Nomal;
        }
        else
        {
            Debug.Log("Crepe Pattern 3");
        }
    }
}
