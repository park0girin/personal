using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player_shot;

public class bullet : MonoBehaviour
{
    public float bulletshot = 6.0f;

    void Update()
    {
        transform.Translate(Vector2.up * bulletshot * Time.deltaTime);
    }
}
