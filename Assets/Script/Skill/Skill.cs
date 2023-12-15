using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    private float coolTime;
    [SerializeField] private bool isCool;
    public abstract void Active();
}





