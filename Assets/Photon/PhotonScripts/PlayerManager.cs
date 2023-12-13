using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private PhotonView pv;

    void Start()
    {   
        pv = GetComponent<PhotonView>();

        //자신의 캐릭터일 경우
        if(pv.IsMine)
        {
            Debug.Log("자신의 캐릭터 입니다");
        }
    }

   
    void Update()
    {
        
    }
}
