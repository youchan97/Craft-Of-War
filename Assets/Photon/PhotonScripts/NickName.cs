using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NickName : MonoBehaviour
{
    public GameObject Player1;
    void Start()
    {
        if (Player1.GetComponent<TextMeshProUGUI>() != null)
            Player1.GetComponent<TextMeshProUGUI>().text = PhotonNetwork.LocalPlayer.NickName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
