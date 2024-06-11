using System.Collections;
using TMPro;
using UnityEngine;

public class BOSS : MonoBehaviour
{
    public GameManager GameManager; // ���� �Ŵ��� ��ü
    public PoolManager PoolManager; // Ǯ �Ŵ��� ��ü
    public Types BOSSType; // ���� Ÿ��
    private Patterns Pattern; // ���� ����
    public float hp; // ������ ���� ü��
    public float inhp; // �ʱ� ü��
    public Mob_Move mm; // �̵� ���� ��ü

    private bool Pattern_toggle; // ���� ��ȯ �÷���
    public bool Barrier_Active; // �踮�� Ȱ��ȭ ����

    private Vector3 nextPosition; // ���� ��ġ
    public float moveSpeed; // �̵� �ӵ�

    public int bulletsToShoot = 7; // �߻��� �Ѿ� ��

    SpriteColliderToggle toggle; // ��������Ʈ �浹 ���

    private Coroutine shootingCoroutine; // �Ѿ� �߻� �ڷ�ƾ
    private Rigidbody2D rb;
    public Vector2 targetPosition;
    public float tolerance = 0.1f; // ��� ����
    public Transform playerTransform; // �÷��̾� ������Ʈ�� Transform
    private bool isRotating = false;
    public float rotationDuration;

    public int? DPTint;
    public int? fstPT;
    int count = 0;
    public float skTime = 0;
    private int offCreams = 0;

    // ���� Ÿ�� ������
    public enum Types
    {
        Crepe,
        Drill
    }

    // ���� ������
    private enum Patterns
    {
        Normal,
        Barrier,
        Aim,
        Ultimate
    }

    private void OnEnable()
    {
        // ���� �Ŵ��� �� Ǯ �Ŵ��� �ʱ�ȭ
        GameManager = FindObjectOfType<GameManager>();
        PoolManager = FindObjectOfType<PoolManager>();

        // Rigidbody2D ������Ʈ�� �Ҵ�
        rb = GetComponent<Rigidbody2D>();

        // �÷��̾� ������Ʈ ã��
        playerTransform = GameObject.FindWithTag("player").transform;

        // �̵� ��ü �Ҵ�
        mm = GetComponent<Mob_Move>();

        // �ʱ� ü�� ����
        inhp = mm.hpmax;

        // ���� Ÿ�Կ� ���� �ʱ�ȭ
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

        // �ʱ� ���� ����
        hp = inhp; // ������ ���� ü���� �ʱ� ü������ ����
        Pattern_toggle = false; // ���� ��ȯ �÷��� �ʱ�ȭ
        Barrier_Active = false; // �踮�� Ȱ��ȭ ���� �ʱ�ȭ
        nextPosition = transform.position; // ���� ��ġ�� ���� ��ġ�� �ʱ�ȭ
        isRotating = false; // ȸ�� ���� �ʱ�ȭ
    }

    private void Update()
    {
        if (!GameManager.BossBattle) mm.hp = 0;
        hp = mm.hp; // ���� ü�� ������Ʈ
        float healthRatio = hp / inhp; // ü�� ���� ���

        Debug.Log(healthRatio);
        if (mm.specialSkill)
        {
            // ���� Ÿ�Կ� ���� ���� ����
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

    // �帱 ���� ���� �޼���
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

    // �帱 ���� 1 (�踮�� ����)
    private void DrillPattern1()
    {
        Debug.Log("Drill Pattern 1");
        toggle.Toggle(false);

        // ���� ��ġ�� �̵�
        Vector3 targetPosition = Vector3.MoveTowards(transform.position, nextPosition, moveSpeed * Time.deltaTime);
        targetPosition = ClampPosition(targetPosition);

        if (Vector3.Distance(transform.position, nextPosition) < 0.05f)
        {
            nextPosition = GetRandomPositionWithinBounds();
        }

        transform.position = targetPosition;

        // �Ѿ� �߻� �ڷ�ƾ ���� (�̹� ���� ���� ��� �������� ����)
        if (shootingCoroutine == null)
        {
            shootingCoroutine = StartCoroutine(ShootBulletsRoutine());
        }
    }

    // �Ѿ� �߻� �ڷ�ƾ
    private IEnumerator ShootBulletsRoutine()
    {
        while (true)
        {
            ShootBullets(-90f); // �Ѿ� �߻�
            yield return new WaitForSeconds(1.0f); // 1�� ����
        }
    }

    // �Ѿ� �߻� �޼���
    void ShootBullets(float startingAngle)
    {
        if (PoolManager == null) return; // Ǯ �Ŵ����� ���� ��� ����

        float intervalAngle = 180f / (bulletsToShoot - 1); // �Ѿ� �߻� ���� ����

        for (int i = 0; i < bulletsToShoot; i++)
        {
            GameObject bullet = PoolManager.Get(13); // Ǯ���� �Ѿ� ��������
            if (bullet != null)
            {
                float bulletAngle = startingAngle + i * intervalAngle;
                ShootBullet(bullet, bulletAngle); // �Ѿ� �߻�
            }
        }
    }

    // ���� �Ѿ� �߻� �޼���
    void ShootBullet(GameObject bullet, float angle)
    {
        bullet.transform.position = transform.position; // �Ѿ� ��ġ ����
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle); // �Ѿ� ���� ����
        bullet.SetActive(true); // �Ѿ� Ȱ��ȭ
    }

    // ������ ��ġ ���� �޼���
    private Vector3 GetRandomPositionWithinBounds()
    {
        float randomX = Random.Range(-1.2f, 1.2f);
        float randomY = Random.Range(0f, 3f);
        return new Vector3(randomX, randomY, transform.position.z);
    }

    // ��ġ ���� �޼���
    private Vector3 ClampPosition(Vector3 position)
    {
        float clampedX = Mathf.Clamp(position.x, -1.2f, 1.2f);
        float clampedY = Mathf.Clamp(position.y, 0f, 3f);
        return new Vector3(clampedX, clampedY, position.z);
    }

    // �帱 ���� 2
    private void DrillPattern2()
    {
        if (count >= 3) DPTint = null;
        StopShootingCoroutine(); // �Ѿ� �߻� �ڷ�ƾ ����
        Debug.Log("Drill Pattern 2");
        toggle.Toggle(true); // �ݶ��̴� �� ��������Ʈ ��ȯ
        MoveTowardsTarget();
        CheckAndUpdateTargetPosition();
    }

    // ��ǥ ��ġ�� ���� �̵��ϴ� �Լ�
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

    // ��ǥ ��ġ�� �����ߴ��� Ȯ���ϴ� �Լ�
    public bool IsAtTargetPosition(Vector2 currentPosition)
    {
        return Vector2.Distance(currentPosition, targetPosition) <= tolerance;
    }

    // ��ǥ ��ġ�� �����ϴ� �Լ�
    public void CheckAndUpdateTargetPosition()
    {
        if (IsAtTargetPosition(rb.position) && !isRotating)
        {
            Pattern_toggle = !Pattern_toggle; // Pattern_toggle ����
            count++;
            if (Pattern_toggle)
            {
                targetPosition = new Vector2(0, 3); // Ư�� ��ġ�� ����
            }
            else
            {
                targetPosition = playerTransform.position; // �÷��̾� ��ġ�� ����
            }
        }
    }

    // �÷��̾ ���� ���� �ð� ���� ȸ���� �� ���� ��ǥ ��ġ�� �̵��ϴ� �ڷ�ƾ
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

        // ȸ���� �Ϸ�� �� ��ǥ ��ġ�� �����ϰ� �̵� �簳
        isRotating = false;
        CheckAndUpdateTargetPosition();
    }

    // �帱 ���� 3
    private void DrillPattern3()
    {
        Debug.Log("Drill Pattern 3");
        toggle.Toggle(false);
        StopShootingCoroutine(); // �Ѿ� �߻� �ڷ�ƾ ����
        rb.rotation = 0;
        targetPosition = new Vector2(0, 3);
        if (!IsAtTargetPosition(rb.position))
        {
            rb.MovePosition(targetPosition);
        }
    }

    // �帱 ���� 4
    private void DrillPattern4()
    {
        Debug.Log("Drill Pattern 4");
        toggle.Toggle(true);
        DPTint = null;
        StopShootingCoroutine(); // �Ѿ� �߻� �ڷ�ƾ ����
    }

    // �Ѿ� �߻� �ڷ�ƾ ���� �޼���
    private void StopShootingCoroutine()
    {
        if (shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine);
            shootingCoroutine = null;
        }
    }
    // ũ���� ���� ���� �޼���
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

    // ũ���� ���� 1
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

    // ũ���� ���� 2
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
            Barrier_Active = true;  // ���⿡ Barrier_Active�� true�� ����
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

    // ũ���� ���� 3
    void CrepePattern3()
    {
        if (!Barrier_Active)
        {
            skTime = 0;
            offCreams++;
            Debug.Log(offCreams);
            hp -= inhp * 0.15f;
            Debug.Log("����3 ���� HP : " + hp);
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
