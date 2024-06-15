using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBT : MonoBehaviour
{
    public GameObject UI;
    public GameObject StickUI;

    public void StickBT()
    {
        StickUI.SetActive(true);
        UI.SetActive(false);
    }
    public void IceStick()
    {
        ScenesManager.Instance.BulletTypes = ScenesManager.BulletType.Ice;
        ScenesManager.Instance.BulletDamage = 1;
    }
    public void FireStick()
    {
        ScenesManager.Instance.BulletTypes = ScenesManager.BulletType.Fire;
        ScenesManager.Instance.BulletDamage = 3;
    }
    public void ThunderStick()
    {
        ScenesManager.Instance.BulletTypes = ScenesManager.BulletType.Thunder;
        ScenesManager.Instance.BulletDamage = 1.5f;
    }
    public void WindStick()
    {
        ScenesManager.Instance.BulletTypes = ScenesManager.BulletType.Wind;
        ScenesManager.Instance.BulletDamage = 1.5f;
    }
    public void Close()
    {
        UI.SetActive(true);
        StickUI.SetActive(false);
    }
}
