using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsheNormalEffect : MonoBehaviour
{
    Ashe owner;
    void Start()
    {
        owner = (Ashe)GameManager.Instance.PlayerHero;
    }
    void Update()
    {
        transform.position += transform.forward * owner.skillDic[0].speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == owner.clickTarget.gameObject) //null ¿¡·¯ ³²
        {
            Destroy(this.gameObject);
        }
    }
}
