using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsheNormalEffect : MonoBehaviour
{
    Ashe owner;
    bool isHit;
    void Start()
    {
        owner = (Ashe)GameManager.Instance.PlayerHero;
        isHit = false;
    }
    void Update()
    {
        transform.position += transform.forward * owner.skillDic[0].speed * Time.deltaTime;

        if (isHit)
        {
            Destroy(gameObject);
            isHit = false;
        }
    }

    private void OnDestroy()
    {
        owner.InstantiateVFX("fx_ice_hit_01", this.transform, true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Character component)) //null ¿¡·¯ ³²
        {
            if (component == this.owner) return;
            component.Hit(owner);
            isHit = true;
        }
    }
}
