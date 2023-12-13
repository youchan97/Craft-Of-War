using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BuildingManager : MonoBehaviourPunCallbacks
{
    public MeshRenderer meshRenderer;
    MaterialPropertyBlock mpb;
    public PhotonView pv;
    //List<Color> colorList = new List<Color>();// [2];
    private void SetMPB(string propertyName, Color color)
    {
        meshRenderer.GetPropertyBlock(mpb);
        mpb.SetColor(propertyName, color);
        meshRenderer.SetPropertyBlock(mpb);
    }

    private void Awake()
    {
        mpb = new MaterialPropertyBlock();
        pv = GetComponent<PhotonView>();
        /*colorList.Add(Color.red);
        colorList.Add(Color.green);
        foreach (Color color in colorList)
        {
            Debug.Log(color);
        }
        Debug.Log(GameManager.Instance.index);*/
    }

    private void Start()
    {
        if(pv.IsMine)
            SetMPB("_PlayerColor", Color.red);
        else
            SetMPB("_PlayerColor", Color.green);
    }

    


}
