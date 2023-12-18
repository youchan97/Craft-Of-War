using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitController : MonoBehaviour
{
    [SerializeField] private GameObject unitMaker;
    private NavMeshAgent navMeshAgent;
    private Animator anim;
    public Unit unit;

    public NavMeshAgent NavMeshAgent { get { return navMeshAgent; } }

    private void Start()
    {
        //인스펙터 창에 컴포넌트 활성화 버튼이 사라지는 버그가 있어서 빈 Start를 넣어놨습니다.
        unit = GetComponent<Unit>();   
        navMeshAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    public void SelectUnit()
    {
        unitMaker.SetActive(true);
    }

    public void DeselectUnit()
    {
        unitMaker.SetActive(false);
    }

    public void MoveTo(Vector3 end) // 유닛 이동
    {
        NavMeshAgent.SetDestination(end);
    }

    //도착했을때, 유닛이 낑기면 자동으로 경로초기화
    //아직 이상함 수정해야함



}
