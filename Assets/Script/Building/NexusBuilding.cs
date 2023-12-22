using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NexusBuilding : Building, IProductAble
{

    public override void Die()
    {
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
