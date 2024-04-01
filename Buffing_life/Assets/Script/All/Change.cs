using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Change : MonoBehaviour
{
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
}
