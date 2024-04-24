using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_shot : MonoBehaviour
{
    public GameManager GameManager;
    public GameObject bulletobj;
    public int poolSize = 10;
    GameObject[] bulletPool;
    public GameObject pos;
    public float angle;
    float shotTime;
    bool skilling;
    int bulletsToShoot = 3; // ShotType�� 0�� �� �߻��� �Ѿ� ����

    void Start()
    {
        GameManager = FindObjectOfType<GameManager>(); // GameManager ã��
        bulletPool = new GameObject[poolSize];

        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletobj);
            bulletPool[i] = bullet;
            bullet.SetActive(false);
        }
    }

    // �Ѿ� �߻� �޼���
    void ShootBullets(float startingAngle)
    {
        float intervalAngle = angle; // �� �Ѿ� ������ ���� ����

        for (int i = 0; i < bulletsToShoot; i++)
        {
            GameObject bullet = GetNextInactiveBullet(); // ��Ȱ��ȭ�� �Ѿ� ��������
            if (bullet != null)
            {
                // �� �Ѿ��� �߻� ���� ���
                float bulletAngle = startingAngle + i * intervalAngle;
                ShootBullet(bullet, bulletAngle);
            }
        }
    }

    // ���� ��Ȱ��ȭ�� �Ѿ� ��������
    GameObject GetNextInactiveBullet()
    {
        foreach (GameObject bullet in bulletPool)
        {
            if (!bullet.activeSelf)
            {
                return bullet;
            }
        }
        return null;
    }

    // �Ѿ� �߻� �޼���
    void ShootBullet(GameObject bullet, float angle)
    {
        // ������ �������� ��ȯ�Ͽ� �Ѿ��� ���� ���� ���
        Vector3 direction = Quaternion.Euler(0, 0, angle) * Vector3.up;

        // �Ѿ��� ����� ��ġ ����
        bullet.transform.position = pos.transform.position; // �߻� ��ġ
        bullet.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction); // �߻� ����
        bullet.SetActive(true);
    }

    private void Update()
    {
        if (!skilling)
        {
            shotTime += Time.deltaTime;
            if (shotTime >= ScenesManager.Instance.ShotInterval)
            {
                // ShotType�� 0�� ���� 3���� �Ѿ��� �߻�
                if (ScenesManager.Instance.ShotType == 0)
                {
                    // ù ��° �Ѿ��� ���� ���� ����
                    float startingAngle = -angle; // ù ��° �Ѿ��� -angle ������ �߻�

                    ShootBullets(startingAngle); // �Ѿ˵��� �߻��ϰ� �� �Ѿ��� ������ ����

                    shotTime = 0;
                }
            }
        }
    }

    public void SkillShot()
    {
        // ��� Ȱ��ȭ�� �Ѿ��� ��Ȱ��ȭ
        DeactivateAllBullets();

        // ù ��° �Ѿ��� ���� x ��ġ
        float startX = -2.5f;
        // ���� ���� ���
        float interval = CalculateBulletInterval();

        // �Ѿ� �߻�
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = GetNextInactiveBullet(); // ��Ȱ��ȭ�� �Ѿ� ��������
            if (bullet != null)
            {
                // �Ѿ� ��ġ ���� (x�� ���� �������� ��ġ, y�� ������ ������ ����)
                float posX = startX + i * interval;
                float posY = -4.0f;

                // �Ѿ� �߻� ���� ���� (���� �������� �߻�)
                float bulletAngle = 0.0f;

                // �Ѿ� Ȱ��ȭ �� ��ġ/���� ����
                bullet.SetActive(true);
                bullet.transform.position = new Vector3(posX, posY, 0f);
                bullet.transform.rotation = Quaternion.Euler(0f, 0f, bulletAngle);
            }
        }
    }


    // ��� Ȱ��ȭ�� �Ѿ��� ��Ȱ��ȭ�ϴ� �޼���
    void DeactivateAllBullets()
    {
        foreach (GameObject bullet in bulletPool)
        {
            if (bullet.activeSelf)
            {
                bullet.SetActive(false);
            }
        }
    }

    // ���� ���� ���
    float CalculateBulletInterval()
    {
        return (5.0f / (poolSize - 1)); // -2.5���� 2.5������ ������ poolSize-1 ���� ���� ��
    }
}
