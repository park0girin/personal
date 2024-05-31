using UnityEngine;

public class SpriteColliderToggle : MonoBehaviour
{
    public GameManager GameManager;

    public Sprite sprite1; // 첫 번째 스프라이트
    public Sprite sprite2; // 두 번째 스프라이트
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
        Debug.Log("toggle 호출");
        if (shouldToggle)
        {
            // 두 번째 스프라이트로 전환
            spriteRenderer.sprite = sprite2;
        }
        else
        {
            // 첫 번째 스프라이트로 전환
            spriteRenderer.sprite = sprite1;
        }

        // 콜라이더 업데이트
        UpdateCollider(shouldToggle);
        Debug.Log("toggle 종료");
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

        // 콜라이더가 업데이트 되었음을 디버그 로그로 확인
        Debug.Log("콜라이더 업데이트: " + (shouldToggle ? "PolygonCollider 활성화" : "CircleCollider 활성화"));
    }
}
