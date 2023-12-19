using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsheESkillEffect : MonoBehaviour
{
    Ashe owner;
    void Start()
    {
        owner = (Ashe)GameManager.Instance.PlayerHero;
    }
    void Update()
    {
        transform.position += transform.forward * owner.skillDic[2].speed * Time.deltaTime;
    }
}
