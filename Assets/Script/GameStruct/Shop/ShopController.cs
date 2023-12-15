using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class ShopController: MonoBehaviour
{
    //상점을 내비매쉬를 사용하여 이동시킬 예정입니다.
    private NavMeshAgent shopAgent;
    public Transform[] shopMovePoint;
    [SerializeField]
    private const float leavingShopTime = 60f;
    private float curCoolTime;
    public float CurCoolTime
    {
        get { return curCoolTime; }
        set 
        {
            curCoolTime = value;
            UIManager.Instance.shopClosingTime.text = "남은시간 : " + curCoolTime;
        }
    }

    private int shopStopIndex; // 상점이 정지할 위치
    private int addValue; //변화값
    [SerializeField]
    private int index; //ShopMovePoint에 배열 값입니다.
    public int Index
    {
        get => index;
        set
        {
            index = value;
            if (index > shopMovePoint.Length - 2) //배열에 크기와 같아지면 -1을 더하여 index값이 감소합니다.
                addValue = -1;
            if (index <= 0) //index가 0이 되면 1을 더하여 index값이 증가합니다.
                addValue = 1;
        }
    }
    private bool shopStop; //상점이 특정 구간에 멈춰서 상점을 이용할수있게 하는 변수입니다.
    public bool ShopStop
    {
        get => shopStop;
        set
        {
            shopStop = value;
        }
    }
    private void Start()
    {
        Index = 0;
        addValue = 1;
        //변수를 초기화 합니다.
        shopAgent = GetComponent<NavMeshAgent>();
        shopAgent.SetDestination(shopMovePoint[Index].position);
    }
    private void Update()
    {
        if (Vector3.Distance(transform.position, shopMovePoint[Index].position) <= 0.5f)
            ShopDirection();
    }

    public void ShopDirection()
    {
        if (Index == 0)
            shopStopIndex = Random.Range(0, shopMovePoint.Length);
        StartCoroutine(ShopStopCo(shopStopIndex));
    }
    IEnumerator ShopStopCo(int stopIndex)
    {
        Index += addValue;
        shopAgent.SetDestination(shopMovePoint[Index].position);
        Debug.Log(shopStopIndex);
        if (Index == stopIndex)
        {
            ShopStop = true;
            shopAgent.enabled = false;
            Debug.Log("나 멈출꺼야");
            CurCoolTime = leavingShopTime;
            while (CurCoolTime >= 0)
            {
                CurCoolTime -= Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
            Debug.Log("나 움직일꺼야");
            ShopStop = false;
            shopAgent.enabled = true;
            shopAgent.SetDestination(shopMovePoint[Index].position);
        }
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if(Index == 0)
    //        shopStopIndex = Random.Range(0, shopMovePoint.Length); // 다시 돌아 올때 마다 랜덤으로 정지 위치를 정해줌
    //    if (other.gameObject.tag == "MovePoint")
    //        StartCoroutine(ShopStopCo(shopStopIndex));
    //}

}
