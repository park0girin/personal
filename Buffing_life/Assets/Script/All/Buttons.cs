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

    public GameObject UI_text; // 활성화할 오브젝트
    public float activationDuration = 3.0f; // 활성화 지속 시간 (초)

    public void Coming_Soon()
    {
        // 오브젝트를 활성화
        UI_text.SetActive(true);

        // activationDuration 시간 후에 비활성화
        StartCoroutine(DeactivateAfterDelay());
    }
    IEnumerator DeactivateAfterDelay()
    {
        // activationDuration 만큼 대기
        yield return new WaitForSeconds(activationDuration);

        // 지연 후 오브젝트를 비활성화
        UI_text.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
