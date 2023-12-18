using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsheRSkillEffect : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] Vector3 direction;
    [SerializeField] float radius;
    [SerializeField] GameObject hitEffect;
    Rigidbody rb;
    public Ashe owner;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        radius = gameObject.GetComponent<SphereCollider>().radius;
    }

    private void OnEnable()
    {

            
    }

    private void Update()
    {
        rb.MovePosition(rb.transform.position + direction * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {

        if(other.TryGetComponent(out Character character))
        {
            if(character != owner)
            {
                other.GetComponent<IHitAble>().Hit(owner);

                if (Physics.OverlapSphere(transform.position, radius, LayerMask.NameToLayer("Unit")).Length >= 0)
                {
                    Destroy(this.gameObject);
                    GameObject hit = Instantiate(hitEffect, transform.position, transform.rotation);
                }
            }
        }
            
    }
}
