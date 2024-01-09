using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using System;

public class MatchManager : MonoBehaviourPunCallbacks
{

    private string gameVersion = "1";
    public Button readyBtn;
    public string nickName;
    private int maxPlayer = 2;
    public GameObject loadingObj;
    public GameObject currentPlayerCount;
    public static int masterIndexPoint;
    public static int userIndexPoint;
    PhotonView pv;


    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true; // 씬 동기화
        loadingObj.SetActive(false);
        pv = GetComponent<PhotonView>();
    }
    void Start()
    {
        readyBtn.interactable = true;      
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings(); // 서버 연결
    }
    private void Update()
    {
        //닉네임을 입력해야 매칭 버튼을 누를 수 있게 설정
        if (FindObjectOfType<TMP_InputField>().text == "")
        {
            readyBtn.interactable = false;
        }
        else
        {
            readyBtn.interactable = true;
        }
    }



    public void Match()
    {
        //매칭 중일 때의 기능
        readyBtn.interactable = false;

        if(PhotonNetwork.IsConnected)
        {
            //서버의 연결되었을 시
            loadingObj.SetActive(true);
            nickName = FindObjectOfType<TMP_InputField>().text;
            PhotonNetwork.LocalPlayer.NickName = nickName;
            RoomOptions roomOptions = new RoomOptions(); // 방을 초기화
            roomOptions.MaxPlayers = maxPlayer; //방의 옵션 (최대 인원수)설정
            //방이 있을 시 랜덤 방 입장, 방이 없을 시 위의 설정한 옵션의 방을 새로 생성
            PhotonNetwork.JoinRandomOrCreateRoom(expectedCustomRoomProperties: 
                new ExitGames.Client.Photon.Hashtable(),roomOptions: roomOptions);
        }
        else
        {
            //서버의 연결에 실패했을 시
            PhotonNetwork.ConnectUsingSettings(); //다시 연결
        }
    }

    public void CancelMatching()
    {
        //매칭 취소 버튼 클릭 기능
        print("매칭 취소.");
        loadingObj.SetActive(false);

        print("방 떠남.");
        PhotonNetwork.LeaveRoom();
    }

    private void UpdatePlayerCounts()
    {
        // 매칭중인 화면에서 현재 방에 있는 인원 수 안내 문구
        if (currentPlayerCount.GetComponent<TextMeshProUGUI>() != null)
        {
            currentPlayerCount.GetComponent<TextMeshProUGUI>().text = $"{PhotonNetwork.CurrentRoom.PlayerCount}";
        }
    }

    public override void OnJoinedRoom() // 방에 들어왔을 때 함수 호출
    {
        print("방 참가 완료.");
        if(PhotonNetwork.IsMasterClient)
        {
            // 마스터 클라이언트의 스폰 포인트 랜덤으로 할당
            masterIndexPoint = UnityEngine.Random.Range(0, PhotonNetwork.CurrentRoom.MaxPlayers);
            // RPC 함수 호출로 나머지 사람의 스폰 포인트 할당
            pv.RPC("SpawnIndex", RpcTarget.AllBuffered, masterIndexPoint);
        }
        UpdatePlayerCounts(); //방 인원 수 증가

        loadingObj.SetActive(true);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) //플레이어가 방에 들어왔을 때
    {
        Debug.Log($"플레이어 {newPlayer.NickName} 방 참가.");
        UpdatePlayerCounts();

        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                //마스터 클라이언트가 방에 인원이 다 차면 방을 닫고 모든 플레이어의 씬을 게임 화면으로 이동시킴
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.LoadLevel("Main"); 
            }
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    [PunRPC]
    public int SpawnIndex(int index)
    {
        //마스터 클라이언트와 다른 스폰 지점 할당
        if (index == 0)
            return userIndexPoint = 1;
        else
            return userIndexPoint = 0;
    }
}
