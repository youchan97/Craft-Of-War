using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorganaQSkillEffect : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] private Vector3 direction;
    [SerializeField] GameObject hitEffect;
    Rigidbody rb;
    public Morgana owner;

    public Vector3 Direction
    {
        get => direction;
        set
        {
            direction = value.normalized;
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        owner = (Morgana)GameManager.Instance.PlayerHero;
    }



}
