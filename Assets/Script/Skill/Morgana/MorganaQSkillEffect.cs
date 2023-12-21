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

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Character character))
        {
            if(character != owner)
            {
                other.GetComponent<IHitAble>().Hit(owner);

                Destroy(this.gameObject);
                GameObject hit = Instantiate(hitEffect,other.gameObject.transform.position + new Vector3(0,2f,0),transform.rotation);
                StartCoroutine(HitEffectCor(hit));
            }
        }
    }

    IEnumerator HitEffectCor(GameObject hitEffect)
    {
        float curTime = 0;
        while (true)
        {
            curTime += Time.deltaTime;
            Debug.Log(curTime);
            if(curTime > 5f)
            {
                Destroy(hitEffect);
                curTime = 0;
            }
            yield return null;
        }
    }
}
