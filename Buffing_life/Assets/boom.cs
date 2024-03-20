using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boom : MonoBehaviour
{
    CircleCollider2D circleCollider;
    float skilltime;
    // Start is called before the first frame update
    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
    }
    private void OnEnable()
    {
        skilltime = 0;
    }
    // Update is called once per frame
    void Update()
    {
        circleCollider.radius = GameManager.Instance.RedArea;
        skilltime += Time.deltaTime;
        if (skilltime > 0.3f) gameObject.SetActive(false);
    }
}
