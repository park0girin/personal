using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_new : MonoBehaviour
{
    public GameObject Obj;
    private void Start()
    {
        Close();
    }
    public void Open()
    {
        Obj.SetActive(true);
    }
    public void Close()
    {
        Obj.SetActive(false);
    }

}
