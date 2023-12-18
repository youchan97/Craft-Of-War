using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsheRSkillEffect : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float radius;
    [SerializeField] private Vector3 direction;
    [SerializeField] GameObject hitEffect;
    Rigidbody rb;
    public Ashe owner;

    public Vector3 Direction { get { return direction; } set { direction = value; } }
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        radius = gameObject.GetComponent<SphereCollider>().radius;
        owner = (Ashe)GameManager.Instance.PlayerHero;
    }


    private void Update()
    {
        rb.MovePosition(owner.skillTransform[0].transform.position - Input.mousePosition  * moveSpeed * Time.deltaTime);
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
