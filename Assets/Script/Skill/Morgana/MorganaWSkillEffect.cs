using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorganaWSkillEffect : MonoBehaviour
{
    [SerializeField] float skillRange; // 스킬 범위
    [SerializeField] LayerMask targetLayer;
    private float durationTime = 4f;
    public Morgana owner;

    private void Start()
    {
        owner = (Morgana)GameManager.Instance.playerHero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Character character))
        {
            if(character != owner)
            {
                other.GetComponent<IHitAble>().Hit(owner);

            }
        }
    }







}
