using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Player_shot : MonoBehaviour
{
    public GameObject bulletobj;
    public int poolSize = 10;
    GameObject[] bulletPool;
    public GameObject pos;
    float shotTime;
    void Start()
    {
        bulletPool = new GameObject[poolSize];

        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletobj);
            bulletPool[i] = bullet;
            bullet.SetActive(false);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            for (int i = 0; i < poolSize; i++)
            {
                GameObject b = bulletPool[i];
                if (b.activeSelf == false)
                {
                    b.SetActive(true);
                    b.transform.position = pos.transform.position;
                    break;
                }
            }
        }
        else if (Input.GetKey(KeyCode.F))
        {
            shotTime += Time.deltaTime;
            if (shotTime >= GameManager.Instance.ShotInterval)
            {
                for (int i = 0; i < poolSize; i++)
                {
                    GameObject b = bulletPool[i];
                    if (b.activeSelf == false)
                    {
                        b.SetActive(true);
                        b.transform.position = pos.transform.position;
                        shotTime = 0;
                        break;
                    }
                }
            }
        }
    }
}
