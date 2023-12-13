using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveMesh : MonoBehaviour
{
    public float discardRate = 0.03f;
    public float discardRefreshRate = 0.01f;
    public float discardDelay = 1.25f;
    public MeshRenderer[] discardMeshObject;
    void Start()
    {
        StartCoroutine(ErodeObject());
        Destroy(gameObject, discardDelay + 0.5f);
    }

    IEnumerator ErodeObject()
    {
        if (discardMeshObject != null)
        {
            yield return new WaitForSeconds(discardDelay);
            float t = 0;
            while (t < 1)
            {
                t += discardRate;
                foreach (var obj in discardMeshObject)
                {
                    foreach (var material in obj.materials)
                    {
                        material.SetFloat("_Discard", t);
                    }
                }
                yield return new WaitForSeconds(discardRefreshRate);
            }
        }
    }
}
