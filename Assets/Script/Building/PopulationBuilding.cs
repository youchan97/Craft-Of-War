using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationBuilding : Building
{
    [SerializeField]
    private int addPopulation;
    private void Start()
    {
        addPopulation = 5;
    }
    public override void Die()
    {
    }

    public override void Hit()
    {
    }

    public override void OnEnable()
    {
        base.OnEnable();
        GameManager.Instance.Population += addPopulation;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        GameManager.Instance.Population -= addPopulation;
    }
}
