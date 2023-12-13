using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Creep : MonoBehaviour
{
    //// public ENEMY_STATE_TYPE curType;
    //public State curState;

    //public ATTACK_TYPE atkType;
    //public LayerMask targetMask;
    //public float detectiveRange;
    //public float attackableRange;
    //public AttackState attackStrategy = null;

    //public Transform target;
    //public NavMeshAgent agent;
    //// Start is called before the first frame update
    //void Start()
    //{
    //    if (atkType == ATTACK_TYPE.Melee)
    //    {
    //        attackStrategy = new MeleeAttackState(this);
    //    }
    //    else
    //    {
    //        attackStrategy = new RanageAttackState(this);
    //    }

    //    curState = new IdleState(this);
    //    agent = GetComponent<NavMeshAgent>();
    //}

    //public bool isDetectiveTarget;
    //public bool isAttackable;

    //private void Update()
    //{
    //    isDetectiveTarget = Physics.OverlapSphere(transform.position, detectiveRange, targetMask).Length > 0;
    //    isAttackable = Physics.OverlapSphere(transform.position, attackableRange, targetMask).Length > 0;
    //    curState.Update();
    //}

    //public void SetTarget()
    //{
    //    agent.SetDestination(target.position);
    //}

    //public void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireSphere(transform.position, detectiveRange);
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, attackableRange);
    //}
}
//public enum EEnemyStateType
//{
//    IDLE,
//    TRACE,
//    ATTACK
//}

//public class Enemy : MonoBehaviour
//{
//    public EEnemyStateType curType;

//    public Transform target;
//    public NavMeshAgent agent;

//    private void Start()
//    {
//        curType = EEnemyStateType.IDLE;
//        agent = GetComponent<NavMeshAgent>();
//    }

//    private void Update()
//    {
//        switch(curType)
//        {
//            case EEnemyStateType.IDLE:
//                {
//                    Collider[] cols = Physics.OverlapSphere(transform.position, 3f);

//                    foreach (Collider col in cols)
//                    {
//                        if (col.gameObject.GetComponent<Player>())
//                        {
//                            curType = EEnemyStateType.TRACE;
//                            target = col.gameObject.transform;
//                            break;
//                        }
//                    }
//                }
//            break;
//            case EEnemyStateType.TRACE:
//                {
//                    SetTarget();
//                    if(Vector3.Distance(transform.position, target.position) < 1)
//                    {
//                        agent.isStopped = true;
//                        curType = EEnemyStateType.ATTACK;
//                    }
//                }

//            break;
//            case EEnemyStateType.ATTACK:
//                Debug.Log("Attack!");
//                if (Vector3.Distance(transform.position, target.position) > 1)
//                {
//                    curType = EEnemyStateType.TRACE;
//                }
//                break;
//        }
//    }

//    public void SetTarget()
//    {
//        agent.isStopped = false;
//        agent.SetDestination(target.position);
//    }

//}
