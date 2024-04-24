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

    public GameObject UI_text; // Ȱ��ȭ�� ������Ʈ
    public float activationDuration = 3.0f; // Ȱ��ȭ ���� �ð� (��)

    public void Coming_Soon()
    {
        // ������Ʈ�� Ȱ��ȭ
        UI_text.SetActive(true);

        // activationDuration �ð� �Ŀ� ��Ȱ��ȭ
        StartCoroutine(DeactivateAfterDelay());
    }
    IEnumerator DeactivateAfterDelay()
    {
        // activationDuration ��ŭ ���
        yield return new WaitForSeconds(activationDuration);

        // ���� �� ������Ʈ�� ��Ȱ��ȭ
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
