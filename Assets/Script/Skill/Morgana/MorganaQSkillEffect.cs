using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorganaQSkillEffect : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] private Vector3 direction;
    [SerializeField] GameObject hitEffect;

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
        owner = (Morgana)GameManager.Instance.PlayerHero;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * moveSpeed *Time.deltaTime, Space.Self);
    }

    private void OnParticleTrigger()
    {
        
    }

}
