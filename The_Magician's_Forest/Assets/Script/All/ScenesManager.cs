using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SceneManager.LoadScene("Title");        
    }

    // Player
    public float ShotInterval = 0.5f;
    public int PlayerLifeMax = 3;

    // Bullet
    public float BulletDamage = 1.0f;

    //Type
    public enum BulletType
    {
        Ice,
        Fire,
        Thunder,
        Wind
    }

    public BulletType BulletTypes = BulletType.Ice;

    public float UpDamage_Ice = 1.0f;
    public float UpDamage_Fire = 0.3f;
    public float UpDamage_Thunder = 0.3f;
    public float UpDamage_Wind = 1.0f;
}
