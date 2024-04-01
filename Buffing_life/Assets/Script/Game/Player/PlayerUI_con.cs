using UnityEngine;
using UnityEngine.UI;

public class PlayerUI_con : MonoBehaviour
{
    public Image gaugeImage; // ������ �� �̹���
    public Transform player; // �÷��̾� Transform

    // ������ ���� �ּ� �� �ִ� ���� �����մϴ�.
    public float minGaugeValue;
    public float maxGaugeValue;

    // ������ ���� ���� ��
    public float currentValue;


    void Update()
    {
        if (!GameManager.Instance.GameOver)
        {
            // �÷��̾��� ��ġ�� ȭ�� ��ǥ�� ��ȯ�մϴ�.
            Vector3 playerScreenPos = Camera.main.WorldToScreenPoint(player.position);

            // ������ ���� ��ġ�� �÷��̾��� ȭ�� ��ǥ�� �̵���ŵ�ϴ�.
            transform.position = playerScreenPos;

            // ������ �ٰ� ȭ�� �ٱ����� ������ �ʵ��� ȭ�� ��踦 ������� Ȯ���Ͽ� �����մϴ�.
            ClampToScreen();
        }
        else this.gameObject.SetActive(false);
    }

    // ȭ�� ��踦 ����� �ʵ��� ������ ���� ��ġ�� �����մϴ�.
    void ClampToScreen()
    {
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, 0f, Screen.width);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, 0f, 1880.0f);
        transform.position = clampedPosition;
    }

    // ������ ���� �����ϴ� �޼���
    public void ChangeGaugeValue(float newValue)
    {
        // ���� ������ �ּ� �� �ִ� ������ �����մϴ�.
        currentValue = Mathf.Clamp(newValue, minGaugeValue, maxGaugeValue);

        // ������ ���� ���̸� �����Ͽ� UI�� �ݿ��մϴ�.
        float fillAmount = (currentValue - minGaugeValue) / (maxGaugeValue - minGaugeValue);
        gaugeImage.fillAmount = fillAmount;
    }
}
