using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bg_scroll : MonoBehaviour
{
    public GameManager GameManager;
    public float scrollSpeed;
    float targetOffset;
    Renderer rendererer;

    void Start()
    {
        rendererer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (!GameManager.BossBattle)
        {
            targetOffset += Time.deltaTime * scrollSpeed;
            rendererer.material.mainTextureOffset
                = new Vector2(0, targetOffset);
        }
    }
}
