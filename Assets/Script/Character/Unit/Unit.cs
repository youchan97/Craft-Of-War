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

    public float coolTime;
    //public bool isDetect;
    //public float detectRange;
    //public LayerMask target;
    //public Collider[] cols;
    

    public new void Awake()
    {
        Hp = 100;
        base.Awake();
        //isDetect = false;
        InitSm();
    }
    public virtual void Update()
    {
        //cols = Physics.OverlapSphere(gameObject.transform.position, detectRange, target);
        sm.UpdateState();
        animator.SetInteger("State", sm.stateEnumInt);
/*        if(cols.Length > 0 )
        {
            isDetect = true;
        }
        else
        {
            isDetect = false;
        }*/
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
    public void RPCSetActive(bool active)
    {
        gameObject.SetActive(active);
        if(active)
        {
            Debug.Log("DebugID : 11,,,,"+ photonView.ViewID + ":" + photonView.Owner.NickName);
        }
    }
}
