using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class Building : MonoBehaviourPunCallbacks, IHitAble
{
    private int hp;
    PhotonView pv;
    public int Hp 
    {
        get => hp; 
        set => hp = value; 
    }

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        pv.RPC("BuildLayer", RpcTarget.AllBuffered);
    }

    public string buildingName;
    public abstract  void Die();
    public abstract void Hit();

    public virtual void Hit(IAttackAble attacker)
    {
        Hp -= attacker.Atk;
    }

    [PunRPC]
    public void BuildLayer()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (pv.IsMine)
                this.gameObject.layer = 12;
            else
                this.gameObject.layer = 13;
        }
        else
        {
            if (pv.IsMine)
                this.gameObject.layer = 13;
            else
                this.gameObject.layer = 12;
        }
    }


    [PunRPC]
    public void RPCSetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}
