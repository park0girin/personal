using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float bulletshot = 6.0f;
    public sprite_change ChangeScript;
    int Red;

    private void Start()
    {
        ChangeScript = FindObjectOfType<sprite_change>();
    }

    private void OnEnable()
    {
        if (ChangeScript != null)
        {
            Red = Mathf.FloorToInt(Random.Range(0, 5));
            if (Red == 0) ChangeScript.Change();
        }
    }
    void Update()
    {
        transform.Translate(Vector2.up * bulletshot * Time.deltaTime);
    }
}
