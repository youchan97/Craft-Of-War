using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXController : MonoBehaviour
{
    [SerializeField] GameObject[] hideObj;
    void Start()
    {
        for(int i = 0; i < hideObj.Length; i++) 
        {
            hideObj[i].SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 8) // 8 = FOV
        {
            for(int i = 0; i < hideObj.Length;i++)
            {
                hideObj[i].SetActive(true);
            }
        }
    }
}
