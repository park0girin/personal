using System.Collections;
using UnityEngine;

public class Thunder : MonoBehaviour
{
    public GameObject plPos;
    public BoxCollider2D Box;
    public Animator animator;
    public RuntimeAnimatorController animatorController; // �ִϸ����� ��Ʈ�ѷ��� ���� �Ҵ�

    public bool Sk = false;

    private void OnEnable()
    {
        Box = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();

        // �ִϸ����� ��Ʈ�ѷ��� �Ҵ�Ǿ� �ִ��� Ȯ���ϰ� �Ҵ�
        if (animator.runtimeAnimatorController == null && animatorController != null)
        {
            animator.runtimeAnimatorController = animatorController;
        }

        animator.SetBool("Skill", Sk);

        if (Box != null)
        {
            Box.enabled = false;
        }

        plPos = GameObject.Find("bulletPos");
    }

    private void Update()
    {
        if (plPos != null)
        {
            transform.position = plPos.transform.position + new Vector3(0, 4f, 0);
        }
    }

    public void Thunder_True()
    {
        if (Box != null)
        {
            Box.enabled = true;
        }
    }

    public void Thunder_False()
    {
        if (Box != null)
        {
            Box.enabled = false;
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void ThunderSkill()
    {
        StartCoroutine(ThunderSkillCoroutine());
    }

    private IEnumerator ThunderSkillCoroutine()
    {
        Thunder_True();
        yield return new WaitForSeconds(0.005f);
        Thunder_False();
    }

    public void Skill_bool()
    {
        Sk = !Sk;
        animator.SetBool("Skill", Sk);
    }

    public void TriggerThunderSkill()
    {
        GameObject thunderObject = GameObject.FindWithTag("Thunder"); // "Thunder" �±׸� ���� ������Ʈ ã��
        if (thunderObject != null)
        {
            Thunder thunderScript = thunderObject.GetComponent<Thunder>();
            if (thunderScript != null)
            {
                thunderScript.Skill_bool();
            }
        }
    }

    public void Shot()
    {
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            animator.SetTrigger("Shot");
        }
    }
}
