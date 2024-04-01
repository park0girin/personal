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
    public void GameScene()
    {
        SceneManager.LoadScene("Game");
    }
    public void ToolScene()
    {
        SceneManager.LoadScene("Tool");
    }
    public void TitleScene()
    {
        SceneManager.LoadScene("Title");
    }

    // Player
    public float ShotInterval = 0.5f;
    public int PlayerLifeMax = 3;

    // Bullet
    public float BulletDamage = 1.0f;
}