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
        PhotonNetwork.AutomaticallySyncScene = true; // �� ����ȭ
        loadingObj.SetActive(false);
        pv = GetComponent<PhotonView>();
    }
    void Start()
    {
        readyBtn.interactable = true;      
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings(); // ���� ����
    }
    private void Update()
    {
        //�г����� �Է��ؾ� ��Ī ��ư�� ���� �� �ְ� ����
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
        //��Ī ���� ���� ���
        readyBtn.interactable = false;

        if(PhotonNetwork.IsConnected)
        {
            //������ ����Ǿ��� ��
            loadingObj.SetActive(true);
            nickName = FindObjectOfType<TMP_InputField>().text;
            PhotonNetwork.LocalPlayer.NickName = nickName;
            RoomOptions roomOptions = new RoomOptions(); // ���� �ʱ�ȭ
            roomOptions.MaxPlayers = maxPlayer; //���� �ɼ� (�ִ� �ο���)����
            //���� ���� �� ���� �� ����, ���� ���� �� ���� ������ �ɼ��� ���� ���� ����
            PhotonNetwork.JoinRandomOrCreateRoom(expectedCustomRoomProperties: 
                new ExitGames.Client.Photon.Hashtable(),roomOptions: roomOptions);
        }
        else
        {
            //������ ���ῡ �������� ��
            PhotonNetwork.ConnectUsingSettings(); //�ٽ� ����
        }
    }

    public void CancelMatching()
    {
        //��Ī ��� ��ư Ŭ�� ���
        print("��Ī ���.");
        loadingObj.SetActive(false);

        print("�� ����.");
        PhotonNetwork.LeaveRoom();
    }

    private void UpdatePlayerCounts()
    {
        // ��Ī���� ȭ�鿡�� ���� �濡 �ִ� �ο� �� �ȳ� ����
        if (currentPlayerCount.GetComponent<TextMeshProUGUI>() != null)
        {
            currentPlayerCount.GetComponent<TextMeshProUGUI>().text = $"{PhotonNetwork.CurrentRoom.PlayerCount}";
        }
    }

    public override void OnJoinedRoom() // �濡 ������ �� �Լ� ȣ��
    {
        print("�� ���� �Ϸ�.");
        if(PhotonNetwork.IsMasterClient)
        {
            // ������ Ŭ���̾�Ʈ�� ���� ����Ʈ �������� �Ҵ�
            masterIndexPoint = UnityEngine.Random.Range(0, PhotonNetwork.CurrentRoom.MaxPlayers);
            // RPC �Լ� ȣ��� ������ ����� ���� ����Ʈ �Ҵ�
            pv.RPC("SpawnIndex", RpcTarget.AllBuffered, masterIndexPoint);
        }
        Debug.Log($"{PhotonNetwork.LocalPlayer.NickName} / {PhotonNetwork.CurrentRoom.PlayerCount}");
        UpdatePlayerCounts(); //�� �ο� �� ����

        loadingObj.SetActive(true);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) //�÷��̾ �濡 ������ ��
    {
        Debug.Log($"�÷��̾� {newPlayer.NickName} �� ����.");
        UpdatePlayerCounts();

        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                //������ Ŭ���̾�Ʈ�� �濡 �ο��� �� ���� ���� �ݰ� ��� �÷��̾��� ���� ���� ȭ������ �̵���Ŵ
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
        //������ Ŭ���̾�Ʈ�� �ٸ� ���� ���� �Ҵ�
        if (index == 0)
            return userIndexPoint = 1;
        else
            return userIndexPoint = 0;
    }
}
