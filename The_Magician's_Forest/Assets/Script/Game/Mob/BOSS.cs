using System.Collections;
using UnityEngine;

public class BOSS : MonoBehaviour
{
    public GameManager GameManager; // 게임 매니저 객체
    public PoolManager PoolManager; // 풀 매니저 객체
    public Types BOSSType; // 보스 타입
    private Patterns Pattern; // 현재 패턴
    public float hp; // 보스의 현재 체력
    public float inhp; // 초기 체력
    public Mob_Move mm; // 이동 관리 객체
    public bool Cream; // 크림 패턴 여부

    private bool B; // 패턴 전환 플래그
    private bool Barrier_Active; // 배리어 활성화 여부

    private Vector3 nextPosition; // 다음 위치
    public float moveSpeed; // 이동 속도

    public int bulletsToShoot = 7; // 발사할 총알 수

    SpriteColliderToggle toggle; // 스프라이트 충돌 토글

    private Coroutine shootingCoroutine; // 총알 발사 코루틴

    // 보스 타입 열거형
    public enum Types
    {
        Crepe,
        Drill
    }

    // 패턴 열거형
    private enum Patterns
    {
        Normal,
        Barrier,
        Aim,
        Ultimate
    }

    private void OnEnable()
    {
        // 게임 매니저 및 풀 매니저 초기화
        GameManager = FindObjectOfType<GameManager>();
        PoolManager = FindObjectOfType<PoolManager>();

        mm = GetComponent<Mob_Move>(); // 이동 객체 할당

        inhp = mm.hp_input * GameManager.HpMultiplier; // 초기 체력 설정

        if (BOSSType == Types.Drill)
        {
            toggle = GetComponent<SpriteColliderToggle>(); // 드릴 타입일 경우 토글 객체 할당
        }
    }

    private void Update()
    {
        hp = mm.hp; // 현재 체력 업데이트
        float healthRatio = hp / inhp; // 체력 비율 계산
        Debug.Log(healthRatio);

        // 보스 타입에 따른 패턴 실행
        switch (BOSSType)
        {
            case Types.Drill:
                UpdatePattern(healthRatio);
                ExecuteDrillPattern();
                break;
        }
    }
    void UpdatePattern(float healthRatio)
    {
        if (hp / inhp > 0.75f)
        {
            Pattern = Patterns.Barrier;
        }
        else if (hp / inhp > 0.50f)
        {
            Pattern = Patterns.Normal;
        }
        else if (hp / inhp > 0.25f)
        {
            Pattern = Patterns.Aim;
        }
        else
        {
            Pattern = Patterns.Ultimate;
        }
    }

    // 드릴 패턴 실행 메서드
    private void ExecuteDrillPattern()
    {
        switch (Pattern)
        {
            case Patterns.Barrier:
                DrillPattern1();
                break;
            case Patterns.Normal:
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

    // 드릴 패턴 1 (배리어 패턴)
    private void DrillPattern1()
    {
        Debug.Log("Drill Pattern 1");
        toggle.Toggle(false);

        // 다음 위치로 이동
        Vector3 targetPosition = Vector3.MoveTowards(transform.position, nextPosition, moveSpeed * Time.deltaTime);
        targetPosition = ClampPosition(targetPosition);

        if (Vector3.Distance(transform.position, nextPosition) < 0.05f)
        {
            nextPosition = GetRandomPositionWithinBounds();
        }

        transform.position = targetPosition;

        // 총알 발사 코루틴 시작 (이미 실행 중인 경우 시작하지 않음)
        if (shootingCoroutine == null)
        {
            shootingCoroutine = StartCoroutine(ShootBulletsRoutine());
        }
    }

    // 총알 발사 코루틴
    private IEnumerator ShootBulletsRoutine()
    {
        while (true)
        {
            ShootBullets(-90f); // 총알 발사
            yield return new WaitForSeconds(1.0f); // 1초 간격
        }
    }

    // 총알 발사 메서드
    void ShootBullets(float startingAngle)
    {
        if (PoolManager == null) return; // 풀 매니저가 없는 경우 종료

        float intervalAngle = 180f / (bulletsToShoot - 1); // 총알 발사 각도 간격

        for (int i = 0; i < bulletsToShoot; i++)
        {
            GameObject bullet = PoolManager.Get(13); // 풀에서 총알 가져오기
            if (bullet != null)
            {
                float bulletAngle = startingAngle + i * intervalAngle;
                ShootBullet(bullet, bulletAngle); // 총알 발사
            }
        }
    }

    // 개별 총알 발사 메서드
    void ShootBullet(GameObject bullet, float angle)
    {
        bullet.transform.position = transform.position; // 총알 위치 설정
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle); // 총알 각도 설정
        bullet.SetActive(true); // 총알 활성화
    }

    // 무작위 위치 설정 메서드
    private Vector3 GetRandomPositionWithinBounds()
    {
        float randomX = Random.Range(-1.2f, 1.2f);
        float randomY = Random.Range(0f, 3f);
        return new Vector3(randomX, randomY, transform.position.z);
    }

    // 위치 제한 메서드
    private Vector3 ClampPosition(Vector3 position)
    {
        float clampedX = Mathf.Clamp(position.x, -1.2f, 1.2f);
        float clampedY = Mathf.Clamp(position.y, 0f, 3f);
        return new Vector3(clampedX, clampedY, position.z);
    }

    // 드릴 패턴 2
    private void DrillPattern2()
    {
        Debug.Log("Drill Pattern 2");
        toggle.Toggle(true); // 콜라이더 및 스프라이트 전환
        StopShootingCoroutine(); // 총알 발사 코루틴 중지
    }

    // 드릴 패턴 3
    private void DrillPattern3()
    {
        Debug.Log("Drill Pattern 3");
        toggle.Toggle(false);
        StopShootingCoroutine(); // 총알 발사 코루틴 중지
    }

    // 드릴 패턴 4
    private void DrillPattern4()
    {
        Debug.Log("Drill Pattern 4");
        toggle.Toggle(true);
        StopShootingCoroutine(); // 총알 발사 코루틴 중지
    }

    // 총알 발사 코루틴 중지 메서드
    private void StopShootingCoroutine()
    {
        if (shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine);
            shootingCoroutine = null;
        }
    }

    // 크레페 패턴 실행 메서드
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
            case Patterns.Normal:
                CrepePattern1();
                B = !B;
                break;
        }

        if (hp / inhp <= 0.75f && !Barrier_Active)
        {
            Barrier_Active = true;
            if (Pattern != Patterns.Ultimate) Pattern = Patterns.Barrier;
        }
        else if (hp / inhp <= 0.25f && !Barrier_Active)
        {
            Barrier_Active = true;
            if (Pattern != Patterns.Ultimate) Pattern = Patterns.Barrier;
        }
    }

    // 크레페 패턴 1
    void CrepePattern1()
    {
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

        foreach (var position in bulletPositions)
        {
            GameObject bullet = PoolManager.Get(11);
            if (bullet != null)
            {
                bullet.transform.position = position;
                bullet.SetActive(true);
            }
        }
    }

    // 크레페 패턴 2
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

        foreach (var position in bulletPositions)
        {
            GameObject bullet = PoolManager.Get(12);
            if (bullet != null)
            {
                bullet.transform.position = position;
                bullet.SetActive(true);
            }
        }
    }

    // 크레페 패턴 3
    void CrepePattern3()
    {
        if (!Cream)
        {
            Pattern = Patterns.Normal;
        }
        else
        {
            Debug.Log("Crepe Pattern 3");
        }
    }

    private void OnDisable()
    {
        Debug.Log("Boss disabled");
    }
}
