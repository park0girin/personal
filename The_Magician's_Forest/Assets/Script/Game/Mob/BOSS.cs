using System.Collections;
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
    public bool Cream; // ũ�� ���� ����

    private bool B; // ���� ��ȯ �÷���
    private bool Barrier_Active; // �踮�� Ȱ��ȭ ����

    private Vector3 nextPosition; // ���� ��ġ
    public float moveSpeed; // �̵� �ӵ�

    public int bulletsToShoot = 7; // �߻��� �Ѿ� ��

    SpriteColliderToggle toggle; // ��������Ʈ �浹 ���

    private Coroutine shootingCoroutine; // �Ѿ� �߻� �ڷ�ƾ

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

        mm = GetComponent<Mob_Move>(); // �̵� ��ü �Ҵ�

        inhp = mm.hp_input * GameManager.HpMultiplier; // �ʱ� ü�� ����

        if (BOSSType == Types.Drill)
        {
            toggle = GetComponent<SpriteColliderToggle>(); // �帱 Ÿ���� ��� ��� ��ü �Ҵ�
        }
    }

    private void Update()
    {
        hp = mm.hp; // ���� ü�� ������Ʈ
        float healthRatio = hp / inhp; // ü�� ���� ���
        Debug.Log(healthRatio);

        // ���� Ÿ�Կ� ���� ���� ����
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
        Debug.Log("Drill Pattern 2");
        toggle.Toggle(true); // �ݶ��̴� �� ��������Ʈ ��ȯ
        StopShootingCoroutine(); // �Ѿ� �߻� �ڷ�ƾ ����
    }

    // �帱 ���� 3
    private void DrillPattern3()
    {
        Debug.Log("Drill Pattern 3");
        toggle.Toggle(false);
        StopShootingCoroutine(); // �Ѿ� �߻� �ڷ�ƾ ����
    }

    // �帱 ���� 4
    private void DrillPattern4()
    {
        Debug.Log("Drill Pattern 4");
        toggle.Toggle(true);
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

    // ũ���� ���� 1
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

    // ũ���� ���� 2
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

    // ũ���� ���� 3
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
