using UnityEngine;

public class SpriteColliderToggle : MonoBehaviour
{
    public GameManager GameManager;

    public Sprite sprite1; // ù ��° ��������Ʈ
    public Sprite sprite2; // �� ��° ��������Ʈ
    public BOSS b;

    public SpriteRenderer spriteRenderer;
    public CircleCollider2D circleCollider;
    public PolygonCollider2D polygonCollider;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
        polygonCollider = GetComponent<PolygonCollider2D>();
    }

    void OnEnable()
    {
        GameManager = FindObjectOfType<GameManager>();
    }

    public void Toggle(bool shouldToggle)
    {
        Debug.Log("toggle ȣ��");
        if (shouldToggle)
        {
            // �� ��° ��������Ʈ�� ��ȯ
            spriteRenderer.sprite = sprite2;
        }
        else
        {
            // ù ��° ��������Ʈ�� ��ȯ
            spriteRenderer.sprite = sprite1;
        }

        // �ݶ��̴� ������Ʈ
        UpdateCollider(shouldToggle);
        Debug.Log("toggle ����");
    }

    private void UpdateCollider(bool shouldToggle)
    {
        if (shouldToggle)
        {
            if (polygonCollider != null) polygonCollider.enabled = true;
            if (circleCollider != null) circleCollider.enabled = false;
        }
        else
        {
            if (circleCollider != null) circleCollider.enabled = true;
            if (polygonCollider != null) polygonCollider.enabled = false;
        }

        // �ݶ��̴��� ������Ʈ �Ǿ����� ����� �α׷� Ȯ��
        Debug.Log("�ݶ��̴� ������Ʈ: " + (shouldToggle ? "PolygonCollider Ȱ��ȭ" : "CircleCollider Ȱ��ȭ"));
    }
}
