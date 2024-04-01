using UnityEngine;
using UnityEngine.UI;

public class PlayerUI_con : MonoBehaviour
{
    public Image gaugeImage; // 게이지 바 이미지
    public Transform player; // 플레이어 Transform

    // 게이지 바의 최소 및 최대 값을 정의합니다.
    public float minGaugeValue;
    public float maxGaugeValue;

    // 게이지 바의 현재 값
    public float currentValue;


    void Update()
    {
        if (!GameManager.Instance.GameOver)
        {
            // 플레이어의 위치를 화면 좌표로 변환합니다.
            Vector3 playerScreenPos = Camera.main.WorldToScreenPoint(player.position);

            // 게이지 바의 위치를 플레이어의 화면 좌표로 이동시킵니다.
            transform.position = playerScreenPos;

            // 게이지 바가 화면 바깥으로 나가지 않도록 화면 경계를 벗어나는지 확인하여 조정합니다.
            ClampToScreen();
        }
        else this.gameObject.SetActive(false);
    }

    // 화면 경계를 벗어나지 않도록 게이지 바의 위치를 조정합니다.
    void ClampToScreen()
    {
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, 0f, Screen.width);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, 0f, 1880.0f);
        transform.position = clampedPosition;
    }

    // 게이지 값을 변경하는 메서드
    public void ChangeGaugeValue(float newValue)
    {
        // 값의 범위를 최소 및 최대 값으로 제한합니다.
        currentValue = Mathf.Clamp(newValue, minGaugeValue, maxGaugeValue);

        // 게이지 바의 길이를 변경하여 UI에 반영합니다.
        float fillAmount = (currentValue - minGaugeValue) / (maxGaugeValue - minGaugeValue);
        gaugeImage.fillAmount = fillAmount;
    }
}
