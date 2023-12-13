using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDie : MonoBehaviour
{
    public void Die()
    {
        Destroy(this.gameObject);
    }
}
