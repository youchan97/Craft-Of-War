using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System;


public abstract class Building : MonoBehaviourPunCallbacks, IHitAble, IPunObservable
{
    public Image hpCan; 

    PhotonView pv;

    public int index;
    protected int hp;
    public int Hp 
    {
        get => hp;
        set
        {
            hp = value;
            if (hp <= 0)
            {
                this.gameObject.GetComponent<Collider>().enabled = false;
                GameManager.Instance.buildingObjectPool.ReturnPool(this.gameObject, this.index);
                Die();
            }
            hpCan.fillAmount = (float)hp/(float)maxHp;
        }
    }

    public int maxHp;
    public int priority;
    public int Priority { get => priority; set => priority = value; }

    public int cost;


    public override void OnEnable()
    {
        base.OnEnable();
        pv.RPC("BuildingInitialize", RpcTarget.AllBuffered);
    }

    public virtual void Awake()
    {
        pv = GetComponent<PhotonView>();
        pv.RPC("BuildLayer", RpcTarget.AllBuffered);
        priority = 5;
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

    [PunRPC]
    public void BuildingInitialize()
    {
        this.maxHp = 300;
        this.hp = maxHp;
        this.gameObject.GetComponent<Collider>().enabled = true;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Hp);
        }
        else
        {
            this.Hp = (int)stream.ReceiveNext();
        }
    }
}
