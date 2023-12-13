using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    [SerializeField] private GameObject bulidingMaker;
    [SerializeField]
    bool select;
    public bool Select
    {
        get { return select; }
        set { select = value; } 
    }

 
    public void SelectBuliding()
    {
        //if (bulidingMaker == null)
        //    return;
        bulidingMaker.SetActive(true);
        Select = true;
    }

    public void DeselectBuliding()
    {
        //if (bulidingMaker == null)
        //    return;
        bulidingMaker.SetActive(false);
        Select = false;
    }
    private void Update()
    {

    }
}
