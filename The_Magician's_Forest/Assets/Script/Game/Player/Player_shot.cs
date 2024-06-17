using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ScenesManager;

public class Player_shot : MonoBehaviour
{
    public GameManager GameManager;
    public GameObject[] bulletobjs;
    public GameObject bulletobj;
    public int poolSize = 10;
    GameObject[] bulletPool;
    public GameObject pos;
    public float angle;
    float shotTime;
    public bool skilling;
    int bulletsToShoot = 3; // ShotType�� 0�� �� �߻��� �Ѿ� ����
    public BulletType BulletTypes;

    void Start()
    {
        GameManager = FindObjectOfType<GameManager>(); // GameManager ã��
        
    }

    private void OnEnable()
    {
        BulletTypes = ScenesManager.Instance.BulletTypes;
        switch (BulletTypes)
        {
            case BulletType.Ice:
                poolSize = 10;
                bulletsToShoot = 3;
                ScenesManager.Instance.BulletDamage = 1;
                bulletobj = bulletobjs[0];
                break;
            case BulletType.Fire:
                poolSize = 1;
                bulletsToShoot = 1;
                ScenesManager.Instance.BulletDamage = 3;
                bulletobj = bulletobjs[1];
                break;
            case BulletType.Thunder:
                poolSize = 1;
                bulletsToShoot = 1;
                ScenesManager.Instance.BulletDamage = 1.5f;
                bulletobj = bulletobjs[2];
                break;
            case BulletType.Wind:
                poolSize = 8;
                bulletsToShoot = 1;
                ScenesManager.Instance.BulletDamage = 1.5f;
                bulletobj = bulletobjs[3];
                break;
            default:
                BulletTypes = BulletType.Ice;
                poolSize = 10;
                bulletsToShoot = 3;
                ScenesManager.Instance.BulletDamage = 1;
                bulletobj = bulletobjs[0];
                break;
        }

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
        if (!GameManager.GameOver)
        {
            if (!skilling)
            {
                if ((BulletTypes != BulletType.Fire) && (BulletTypes != BulletType.Thunder)) shotTime += Time.deltaTime;
                else
                {
                    foreach (GameObject bullet in bulletPool)
                    {
                        if (!bullet.activeSelf)
                        {
                            bullet.SetActive(true);
                        }
                    }
                }
                if (shotTime >= ScenesManager.Instance.ShotInterval)
                {
                    // ShotType�� 0�� ���� 3���� �Ѿ��� �߻�
                    if (BulletTypes == BulletType.Ice)
                    {
                        // ù ��° �Ѿ��� ���� ���� ����
                        float startingAngle = -angle; // ù ��° �Ѿ��� -angle ������ �߻�

                        ShootBullets(startingAngle); // �Ѿ˵��� �߻��ϰ� �� �Ѿ��� ������ ����

                        shotTime = 0;
                    }
                    else
                    {
                        if (GetNextInactiveBullet() != null)
                        {
                            GameObject bullet = GetNextInactiveBullet(); // ��Ȱ��ȭ�� �Ѿ� ��������
                            ShootBullet(bullet, 0);
                            shotTime = 0;
                        }
                    }
                }
            }
        }
    }

    public void SkillShot()
    {
        if (BulletTypes == BulletType.Ice)
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
        else if(BulletTypes == BulletType.Fire)
        {

        }
        else if (BulletTypes == BulletType.Thunder)
        {

        }
        else if (BulletTypes == BulletType.Wind)
        {

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
