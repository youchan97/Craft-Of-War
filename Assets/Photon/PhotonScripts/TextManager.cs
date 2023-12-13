using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI connectText;
    void Awake()
    {
        
        connectText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        connectText.text = "Connect Server..";
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectText.text = "Fail to Join Room..Retry...";
    }

    public override void OnJoinedRoom()
    {
        connectText.text = "Succes Join Room";
    }
    public override void OnConnectedToMaster()
    {
        connectText.text = "Succes Connection Server";
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        connectText.text = "Disconnect..Retry..";
    }

    public void MatchText()
    {
        connectText.text = "Matching...";
        if (PhotonNetwork.IsConnected)
        {
            connectText.text = "Soon Join Room";
        }
        else
        {
            connectText.text = "Reconnecting...";
        }
    }
}
