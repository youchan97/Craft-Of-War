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

    public Sprite faceSprite;

    public float coolTime;
    public override void Awake()
    {
        base.Awake();
        pv.RPC("UnitLayer", RpcTarget.AllBuffered);
        InitSm();
    }

    public override void OnEnable()
    {
        pv.RPC("Initialize", RpcTarget.AllBuffered);
    }
    public virtual void Update()
    {
        sm.UpdateState();
        animator.SetInteger("State", sm.stateEnumInt);
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
