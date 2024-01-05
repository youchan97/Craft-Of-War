using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public enum UNIT_STATE
{
    None,
    Idle,
    Move,
    Attack,
    Work,
    Die
}

public abstract class Unit : Character
{
    public int obpId;
    public SkinnedMeshRenderer skinMeshRenderer;
    public Sprite faceSprite;
    MaterialPropertyBlock mpb;
    public float coolTime;
    public int cost;

    private void SetMPB(string propertyName, Color color)
    {
        if (skinMeshRenderer != null)
        {
            skinMeshRenderer.GetPropertyBlock(mpb);
            mpb.SetColor(propertyName, color);
            skinMeshRenderer.SetPropertyBlock(mpb);
        }
        else
            return;
    }
    public override void Awake()
    {
        base.Awake();
        mpb = new MaterialPropertyBlock();
        pv.RPC("UnitLayer", RpcTarget.AllBuffered);
        InitSm();
    }

    public override void OnEnable()
    {
        pv.RPC("Initialize", RpcTarget.AllBuffered);
        if (pv.IsMine)
            SetMPB("_PlayerColor", Color.green);
        else
            SetMPB("_PlayerColor", Color.red);
    }

    public override void OnDisable()
    {
        base.OnDisable();
    }
    public virtual void Update()
    {
        sm.UpdateState();
    }

    private void InitSm()
    {
        sm.AddState((int)UNIT_STATE.Idle, new UnitIdleState());
        sm.AddState((int)UNIT_STATE.Move, new UnitMoveState());
        sm.AddState((int)UNIT_STATE.Attack, new UnitAttackState());
        sm.AddState((int)UNIT_STATE.Work, new UnitWorkState());
        sm.AddState((int)UNIT_STATE.Die, new UnitDieState());
        //기본상태로 돌려놈
        sm.SetState((int)UNIT_STATE.Idle);
    }
    [PunRPC]
    public void UnitLayer()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (pv.IsMine)
                this.gameObject.layer = 6;
            else
                this.gameObject.layer = 7;
        }
        else
        {
            if(pv.IsMine)
                this.gameObject.layer = 7;
            else
                this.gameObject.layer = 6;
        }
    }



    [PunRPC]
    public void RPCSetActive(bool active)
    {
        gameObject.SetActive(active);
        if(active)
        {
            Debug.Log("DebugID : 11,,,,"+ photonView.ViewID + ":" + photonView.Owner.NickName);
        }
    }
}
