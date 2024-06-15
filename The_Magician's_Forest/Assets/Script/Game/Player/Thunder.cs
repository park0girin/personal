using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : MonoBehaviour
{
    public GameObject plPos;
    public BoxCollider2D Box;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator가 할당되지 않았습니다!");
        }
    }

    public void OnEnable()
    {
        Box = GetComponent<BoxCollider2D>();
        if (animator != null)
        {
            animator.SetBool("Skill", false);
        }
        if (Box != null) Box.enabled = false;
        if (plPos == null)
        {
            plPos = GameObject.Find("bulletPos");
        }
    }

    private void Update()
    {
        if (plPos != null)
        {
            this.gameObject.transform.position = plPos.transform.position + new Vector3(0, 4f, 0);
        }
        else
        {
            Debug.LogError("plPos가 할당되지 않았습니다! 위치를 설정할 수 없습니다.");
            return;
        }
    }
    public void Thunder_True()
    {
        if (Box != null) Box.enabled = true;
    }
    public void Thunder_False()
    {
        if (Box != null) Box.enabled = false;
    }
    public void Hide()
    {
        if (animator != null)
        {
            animator.SetBool("Skill", false);
        }
        this.gameObject.SetActive(false);
    }
    public void ThunderSkil()
    {
        StartCoroutine(ThunderSkilCoroutine());
    }

    private IEnumerator ThunderSkilCoroutine()
    {
        // Thunder_True를 호출하여 콜라이더를 활성화합니다.
        Thunder_True();

        // 일정 시간 동안 대기합니다.
        yield return new WaitForSeconds(0.005f);

        // Thunder_False를 호출하여 콜라이더를 비활성화합니다.
        Thunder_False();
    }
    public void Skill_bool()
    {
        Debug.Log("스킬활성화다거지같은것");
        if (animator != null)
        {
            animator.SetBool("Skill", true);
        }
    }
}
