using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sprite_change : MonoBehaviour
{
    public Sprite sprite1;
    public Sprite sprite2;
    SpriteRenderer spriteRenderer;

    private void OnEnable()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite1;
    }
    public void Change()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = sprite2;
        }
    }
}
