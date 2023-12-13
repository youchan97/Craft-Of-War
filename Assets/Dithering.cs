using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dithering : MonoBehaviour
{
    public SkinnedMeshRenderer[] renderers;
    MaterialPropertyBlock mpb;
    int targetMaterialIndex = 0;
    bool isTransparency = false;

    public bool IsTransparency
    {
        get => isTransparency;
    }

    private void Start()
    {
        targetMaterialIndex = 0;
        isTransparency = false;
        mpb = new MaterialPropertyBlock();
    }
    void SetMPB(int index, string propertyName, float value)
    {
        renderers[index].GetPropertyBlock(mpb, targetMaterialIndex); //index = 0
        mpb.SetFloat(propertyName, value);
        renderers[index].SetPropertyBlock(mpb, targetMaterialIndex);
    }

    void TurnOffTransparent(int index)
    {
        SetMPB(index, "_Fade", 0);
    }

    void TurnOnTransparent(int index)
    {
        SetMPB(index, "_Fade", 0.5f);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T)) // 조건 변경 필요
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                TurnOnTransparent(i);
            }
            isTransparency = true;
        }

        if(Input.GetKeyDown(KeyCode.S) && isTransparency) // 조건 변경 필요
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                TurnOffTransparent(i);
            }
            isTransparency = false;
        }
    }
}
