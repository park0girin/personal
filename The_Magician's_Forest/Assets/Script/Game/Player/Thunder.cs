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
            Debug.LogError("Animator�� �Ҵ���� �ʾҽ��ϴ�!");
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
            Debug.LogError("plPos�� �Ҵ���� �ʾҽ��ϴ�! ��ġ�� ������ �� �����ϴ�.");
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
        // Thunder_True�� ȣ���Ͽ� �ݶ��̴��� Ȱ��ȭ�մϴ�.
        Thunder_True();

        // ���� �ð� ���� ����մϴ�.
        yield return new WaitForSeconds(0.005f);

        // Thunder_False�� ȣ���Ͽ� �ݶ��̴��� ��Ȱ��ȭ�մϴ�.
        Thunder_False();
    }
    public void Skill_bool()
    {
        Debug.Log("��ųȰ��ȭ�ٰ���������");
        if (animator != null)
        {
            animator.SetBool("Skill", true);
        }
    }
}
