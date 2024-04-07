using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
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
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
