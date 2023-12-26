using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NexusBuilding : Building, IProductAble
{

    public override void Die()
    {
        if (photonView.IsMine)
        {
            UIManager.Instance.result.SetActive(true);
            UIManager.Instance.defeat.SetActive(true);
        }
        else
        {
            UIManager.Instance.result.SetActive(true);
            UIManager.Instance.win.SetActive(true);
        }
    }

    public override void Hit()
    {
    }

    public void Production()
    {
    }
    public override void OnEnable()
    {
        base.OnEnable();
        GameManager.Instance.NexusCount++;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        GameManager.Instance.NexusCount--;
    }
}
