using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class Building : MonoBehaviour, IHitAble
{
    private int hp;
    public int Hp 
    {
        get => hp; 
        set => hp = value; 
    }

    public string buildingName;
    public abstract  void Die();
    public abstract void Hit();

    public virtual void Hit(IAttackAble attacker)
    {
        Hp -= attacker.Atk;
    }

    [PunRPC]
    public void RPCSetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}
