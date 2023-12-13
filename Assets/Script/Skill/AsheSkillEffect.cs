using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsheSkillEffect : MonoBehaviour
{
    public float speed;
    public Vector3 startPos;

    private void Update()
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("EnemyUnit") || other.gameObject.CompareTag("EnemyHero"))
        {
            Debug.Log("Ashe Skill hit!");
        }
    }
}
