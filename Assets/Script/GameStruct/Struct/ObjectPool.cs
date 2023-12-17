using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ObjectPool : MonoBehaviourPunCallbacks
{ 
    Dictionary<int, Stack<GameObject>> poolDict;//프리팹리스트 인덱스에 맞는 스텍딕셔너리 
    //인트부분을 바깥에서 이넘에서 인트로 캐스팅하여 주면 이넘형으로 넘겨받기 가능
    public List<GameObject> prefabList;//인스펙터에서 프리팹을 가져올거라 여기서 초기화하면 안됌
    public int initSize;//넣을갯수
    private void Start()
    {
         Init();
    }

    void Init()
    {
        poolDict = new Dictionary<int, Stack<GameObject>>();

        for (int i = 0; i < prefabList.Count; i++)
        {
            poolDict.Add(i, new Stack<GameObject>());
        }
        //프리팹갯수종류만큼 각 InitSize 대로 채워줌
        for (int j = 0; j < prefabList.Count; j++)
        {
            for (int i = 0; i < initSize; i++)
            {
                if (prefabList[0] == null)//예외처리
                break;

                GameObject temp = PhotonNetwork.Instantiate(prefabList[j].name, transform.position, prefabList[j].transform.rotation);
                temp.transform.SetParent(transform);//오브젝트풀링 부모로
                temp.GetComponent<PhotonView>().RPC("RPCSetActive", RpcTarget.AllBuffered, false);
                GameManager.Instance.onRoundEnd += () => { ReturnPool(temp); };// 게임종료하면 필드 오브젝트 전부 풀로 반환댐 이벤트라 알아서
                poolDict[j].Push(temp);
            }
        }
    }
    
    public GameObject Pop(int prefabListIndex = 0)
    {
        if (poolDict[prefabListIndex].Count <= 0)
        {
            for (int i = 0; i < 5; i++)//예외처리
                //풀 비어있으면 5개씩 추가생성해서 넣기
            {
                GameObject temp = PhotonNetwork.Instantiate(prefabList[prefabListIndex].name, transform.position, Quaternion.identity);
                temp.GetComponent<PhotonView>().RPC("RPCSetActive", RpcTarget.AllBuffered, false);
                poolDict[prefabListIndex].Push(temp);
            }
        }
        GameObject outObj = poolDict[prefabListIndex].Pop();
        outObj.GetComponent<PhotonView>().RPC("RPCSetActive", RpcTarget.All, true);
        return outObj;
    }
    public void ReturnPool(GameObject targetObj,int prefabListIndex = 0)
    {
        targetObj.GetComponent<PhotonView>().RPC("RPCSetActive", RpcTarget.AllBuffered, false);
        poolDict[prefabListIndex].Push(targetObj);
    }

    public GameObject Peek(int prefabListIndex = 0)
    {
        GameObject outObj = poolDict[prefabListIndex].Peek();
        return outObj;
    }
}
