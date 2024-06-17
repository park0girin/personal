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
    int bulletsToShoot = 3; // ShotType이 0일 때 발사할 총알 개수
    public BulletType BulletTypes;

    void Start()
    {
        GameManager = FindObjectOfType<GameManager>(); // GameManager 찾기
        
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

    // 총알 발사 메서드
    void ShootBullets(float startingAngle)
    {
        float intervalAngle = angle; // 각 총알 사이의 각도 간격

        for (int i = 0; i < bulletsToShoot; i++)
        {
            GameObject bullet = GetNextInactiveBullet(); // 비활성화된 총알 가져오기
            if (bullet != null)
            {
                // 각 총알의 발사 각도 계산
                float bulletAngle = startingAngle + i * intervalAngle;
                ShootBullet(bullet, bulletAngle);
            }
        }
    }

    // 다음 비활성화된 총알 가져오기
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

    // 총알 발사 메서드
    void ShootBullet(GameObject bullet, float angle)
    {
        // 각도를 라디안으로 변환하여 총알의 방향 벡터 계산
        Vector3 direction = Quaternion.Euler(0, 0, angle) * Vector3.up;

        // 총알의 방향과 위치 설정
        bullet.transform.position = pos.transform.position; // 발사 위치
        bullet.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction); // 발사 각도
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
                    // ShotType이 0일 때만 3개의 총알을 발사
                    if (BulletTypes == BulletType.Ice)
                    {
                        // 첫 번째 총알의 시작 각도 설정
                        float startingAngle = -angle; // 첫 번째 총알은 -angle 각도로 발사

                        ShootBullets(startingAngle); // 총알들을 발사하고 각 총알의 각도를 설정

                        shotTime = 0;
                    }
                    else
                    {
                        if (GetNextInactiveBullet() != null)
                        {
                            GameObject bullet = GetNextInactiveBullet(); // 비활성화된 총알 가져오기
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
            // 모든 활성화된 총알을 비활성화
            DeactivateAllBullets();

            // 첫 번째 총알의 시작 x 위치
            float startX = -2.5f;
            // 일정 간격 계산
            float interval = CalculateBulletInterval();

            // 총알 발사
            for (int i = 0; i < poolSize; i++)
            {
                GameObject bullet = GetNextInactiveBullet(); // 비활성화된 총알 가져오기
                if (bullet != null)
                {
                    // 총알 위치 설정 (x는 일정 간격으로 배치, y는 고정된 값으로 설정)
                    float posX = startX + i * interval;
                    float posY = -4.0f;

                    // 총알 발사 각도 설정 (위쪽 방향으로 발사)
                    float bulletAngle = 0.0f;

                    // 총알 활성화 및 위치/각도 설정
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


    // 모든 활성화된 총알을 비활성화하는 메서드
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

    // 일정 간격 계산
    float CalculateBulletInterval()
    {
        return (5.0f / (poolSize - 1)); // -2.5에서 2.5까지의 간격을 poolSize-1 개로 나눈 값
    }
}
