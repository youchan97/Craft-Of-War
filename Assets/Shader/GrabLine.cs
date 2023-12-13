using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrabLine : MonoBehaviour
{
    LineRenderer lineRenderer;
    public float sizeVar;
    GameObject startEffectObj;
    GameObject endEffectObj;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }
    private void OnEnable()
    {
        lineRenderer.enabled = true;
    }
    private void OnDisable()
    {
        Destroy(startEffectObj);
        Destroy(endEffectObj);
        lineRenderer.enabled = false;
    }

    public IEnumerator GrabCo(Transform startTr, Transform endTr)
    {
        endEffectObj = (GameObject)Instantiate(Resources.Load("VFX/fx_grab_end"), endTr, false);
        endEffectObj.transform.localScale *= sizeVar;
        startEffectObj = (GameObject)Instantiate(Resources.Load("VFX/fx_grab_start"), startTr, false);
        while (true)
        {
            if (lineRenderer != null && startTr && endTr)
            {
                lineRenderer.enabled = true;

                startEffectObj.transform.position = startTr.position + transform.forward + transform.up * 0.6f;
                lineRenderer.SetPosition(0, startTr.position + transform.forward + transform.up * 0.6f);
                lineRenderer.SetPosition(1, endTr.position);

            }
            else
                break;
            yield return null;
        }
    }

    //IEnumerator FirstSkillCo(Transform targetTr)
    //{
    //    float timer = 0;
    //    float duration = 2f;
    //    grabLine.enabled = true;
    //    Vector3 targetPos = Vector3.Lerp(targetTr.parent.position, Owner.transform.position, 0.7f);
    //    Coroutine tempCo = StartCoroutine(grabLine.GrabCo(Owner.transform, targetTr));
    //    while (timer < duration)
    //    {
    //        timer += Time.deltaTime;
    //        if (targetTr)
    //            targetTr.parent.position = Vector3.Lerp(targetTr.parent.position, targetPos, Time.deltaTime);
    //        yield return null;
    //    }
    //    StopCoroutine(tempCo);
    //    grabLine.enabled = false;
    //    yield return null;
    //}

}