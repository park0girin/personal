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
    float shotTime;
    void Start()
    {
        GameManager = FindObjectOfType<GameManager>(); // GameManager Ã£±â
        bulletPool = new GameObject[poolSize];

        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletobj);
            bulletPool[i] = bullet;
            bullet.SetActive(false);
        }
    }
    private void OnMouseDown()
    {
        /*for (int i = 0; i < poolSize; i++)
        {
            GameObject b = bulletPool[i];
            if (b.activeSelf == false)
            {
                b.SetActive(true);
                b.transform.position = pos.transform.position;
                break;
            }
        }*/
    }
    private void OnMouseDrag()
    {
        shotTime += Time.deltaTime;
        if (shotTime >= ScenesManager.Instance.ShotInterval)
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
