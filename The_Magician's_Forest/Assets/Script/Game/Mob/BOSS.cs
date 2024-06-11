using System.Collections;
using TMPro;
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

    private bool Pattern_toggle; // 패턴 전환 플래그
    public bool Barrier_Active; // 배리어 활성화 여부

    private Vector3 nextPosition; // 다음 위치
    public float moveSpeed; // 이동 속도

    public int bulletsToShoot = 7; // 발사할 총알 수

    SpriteColliderToggle toggle; // 스프라이트 충돌 토글

    private Coroutine shootingCoroutine; // 총알 발사 코루틴
    private Rigidbody2D rb;
    public Vector2 targetPosition;
    public float tolerance = 0.1f; // 허용 오차
    public Transform playerTransform; // 플레이어 오브젝트의 Transform
    private bool isRotating = false;
    public float rotationDuration;

    public int? DPTint;
    public int? fstPT;
    int count = 0;
    public float skTime = 0;
    private int offCreams = 0;

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

        // Rigidbody2D 컴포넌트를 할당
        rb = GetComponent<Rigidbody2D>();

        // 플레이어 오브젝트 찾기
        playerTransform = GameObject.FindWithTag("player").transform;

        // 이동 객체 할당
        mm = GetComponent<Mob_Move>();

        // 초기 체력 설정
        inhp = mm.hpmax;

        // 보스 타입에 따라 초기화
        if (BOSSType == Types.Crepe)
        {
            skTime = 0;
            offCreams = 0;
        }
        else if (BOSSType == Types.Drill)
        {
            toggle = GetComponent<SpriteColliderToggle>();
            targetPosition = new Vector3(0, 3, 0);
            DPTint = Random.Range(0, 2);
            fstPT = DPTint;
            count = 0;
        }

        // 초기 상태 설정
        hp = inhp; // 보스의 현재 체력을 초기 체력으로 설정
        Pattern_toggle = false; // 패턴 전환 플래그 초기화
        Barrier_Active = false; // 배리어 활성화 여부 초기화
        nextPosition = transform.position; // 다음 위치를 현재 위치로 초기화
        isRotating = false; // 회전 상태 초기화
    }

    private void Update()
    {
        if (!GameManager.BossBattle) mm.hp = 0;
        hp = mm.hp; // 현재 체력 업데이트
        float healthRatio = hp / inhp; // 체력 비율 계산

        Debug.Log(healthRatio);
        if (mm.specialSkill)
        {
            // 보스 타입에 따른 패턴 실행
            switch (BOSSType)
            {
                case Types.Crepe:
                    if (Pattern == Patterns.Ultimate)
                    {
                        skTime += Time.deltaTime;
                        Debug.Log("skTime:" + (int)skTime);
                    }
                    else skTime = 0;
                    break;
                case Types.Drill:
                    UpdatePattern(healthRatio);
                    ExecuteDrillPattern();
                    break;
            }
        }
    }
    void UpdatePattern(float healthRatio)
    {
        if (hp / inhp > 0.75f)
        {
            Pattern = Patterns.Barrier;
            DPTint = 2;
        }
        else if (hp / inhp > 0.50f)
        {
            if (DPTint != null)
            {
                if (fstPT == 0) Pattern = Patterns.Normal;
                else Pattern = Patterns.Ultimate;
            }
            else
            {
                Pattern = Patterns.Barrier;
            }
        }
        else if (hp / inhp > 0.25f)
        {
            Pattern = Patterns.Aim;
            DPTint = 2;
        }
        else
        {
            if (DPTint != null)
            {
                if (fstPT == 0) Pattern = Patterns.Ultimate;
                else Pattern = Patterns.Normal;
            }
            else Pattern = Patterns.Aim;
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
        if (count >= 3) DPTint = null;
        StopShootingCoroutine(); // 총알 발사 코루틴 중지
        Debug.Log("Drill Pattern 2");
        toggle.Toggle(true); // 콜라이더 및 스프라이트 전환
        MoveTowardsTarget();
        CheckAndUpdateTargetPosition();
    }

    // 목표 위치를 향해 이동하는 함수
    public void MoveTowardsTarget()
    {
        Vector2 currentPosition = rb.position;

        if (!IsAtTargetPosition(currentPosition) && !isRotating)
        {
            Vector2 direction = (targetPosition - currentPosition).normalized;
            Vector2 newPosition = currentPosition + direction * 10 * Time.deltaTime;
            rb.MovePosition(newPosition);
        }
        else if (targetPosition == new Vector2(0, 3) && !isRotating)
        {
            StartCoroutine(RotateTowardsPlayer());
        }
    }

    // 목표 위치에 도달했는지 확인하는 함수
    public bool IsAtTargetPosition(Vector2 currentPosition)
    {
        return Vector2.Distance(currentPosition, targetPosition) <= tolerance;
    }

    // 목표 위치를 갱신하는 함수
    public void CheckAndUpdateTargetPosition()
    {
        if (IsAtTargetPosition(rb.position) && !isRotating)
        {
            Pattern_toggle = !Pattern_toggle; // Pattern_toggle 반전
            count++;
            if (Pattern_toggle)
            {
                targetPosition = new Vector2(0, 3); // 특정 위치로 설정
            }
            else
            {
                targetPosition = playerTransform.position; // 플레이어 위치로 설정
            }
        }
    }

    // 플레이어를 향해 일정 시간 동안 회전한 후 최종 목표 위치로 이동하는 코루틴
    private IEnumerator RotateTowardsPlayer()
    {
        isRotating = true;
        float elapsedTime = 0f;

        while (elapsedTime < rotationDuration)
        {
            Vector2 currentPosition = rb.position;
            Vector2 playerDirection = (playerTransform.position - (Vector3)currentPosition).normalized;
            float targetAngle = Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg + 90;
            rb.rotation = Mathf.LerpAngle(rb.rotation, targetAngle, elapsedTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rb.rotation = Mathf.Atan2(playerTransform.position.y - rb.position.y, playerTransform.position.x - rb.position.x) * Mathf.Rad2Deg + 90;

        // 회전이 완료된 후 목표 위치를 갱신하고 이동 재개
        isRotating = false;
        CheckAndUpdateTargetPosition();
    }

    // 드릴 패턴 3
    private void DrillPattern3()
    {
        Debug.Log("Drill Pattern 3");
        toggle.Toggle(false);
        StopShootingCoroutine(); // 총알 발사 코루틴 중지
        rb.rotation = 0;
        targetPosition = new Vector2(0, 3);
        if (!IsAtTargetPosition(rb.position))
        {
            rb.MovePosition(targetPosition);
        }
    }

    // 드릴 패턴 4
    private void DrillPattern4()
    {
        Debug.Log("Drill Pattern 4");
        toggle.Toggle(true);
        DPTint = null;
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
                CrepePattern2();
                break;
            case Patterns.Ultimate:
                CrepePattern3();
                break;
            case Patterns.Normal:
                Barrier_Active = false;
                CrepePattern1();
                Pattern_toggle = !Pattern_toggle;
                break;
        }

        if (hp / inhp <= 0.75f && offCreams == 0)
        {
            if (Pattern != Patterns.Ultimate) Pattern = Patterns.Barrier;
        }
        else if (hp / inhp <= 0.25f && offCreams == 1)
        {
            if (Pattern != Patterns.Ultimate) Pattern = Patterns.Barrier;
        }
    }

    // 크레페 패턴 1
    void CrepePattern1()
    {
        Vector2[] bulletPositions = Pattern_toggle ? new Vector2[]
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
        Debug.Log("Cream");

        Vector2[] bulletPositions = new Vector2[]
        {
        new Vector2(-2.4f, 1.5f),
        new Vector2(-1.6f, 1.5f),
        new Vector2(-0.8f, 1.5f),
        new Vector2(0f, 1.5f),
        new Vector2(0.8f, 1.5f),
        new Vector2(1.6f, 1.5f),
        new Vector2(2.4f, 1.5f)
        };

        if (!Barrier_Active)
        {
            Barrier_Active = true;  // 여기에 Barrier_Active를 true로 설정
            foreach (var position in bulletPositions)
            {
                GameObject bullet = PoolManager.Get(12);
                if (bullet != null)
                {
                    bullet.transform.position = position;
                    bullet.SetActive(true);
                    Debug.Log(Barrier_Active);
                }
            }
        }
        Pattern = Patterns.Ultimate;
    }

    // 크레페 패턴 3
    void CrepePattern3()
    {
        if (!Barrier_Active)
        {
            skTime = 0;
            offCreams++;
            Debug.Log(offCreams);
            hp -= inhp * 0.15f;
            Debug.Log("패턴3 이후 HP : " + hp);
            Pattern = Patterns.Normal;
        }
        else if (skTime > 5f)
        {
            Debug.Log("Crepe Pattern 3");
            GameObject bnn = PoolManager.Get(15);
            bnn.SetActive(true);
            offCreams++;
            Pattern = Patterns.Normal;
        }
    }

    private void OnDisable()
    {
        Debug.Log("Boss disabled");
        GameManager.BossBattle = false;
    }
}
