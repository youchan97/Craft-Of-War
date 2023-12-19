using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;


public struct AttackCastInfo
{
    public Vector3 point;
    public float dst;
    public float angle;

    public AttackCastInfo(Vector3 _point, float _dst, float _angle)
    {
        point = _point;
        dst = _dst;
        angle = _angle;
    }
}

public class AsheWSkill : ActiveSkill
{
    [Range(0, 360)] public float attackAngle;
    public float attackRadius;
    public float resolution;
    List<Vector3> attackPoints;

    private void Start()
    {
        attackPoints = new List<Vector3>();
    }

    public override void Active()
    {
        base.Active();
        GetPoints();
        Ashe realOwner = (Ashe)owner;
        realOwner.animator.SetBool("AttackBasic", true);
        //realOwner.transform.forward = (realOwner.clickTarget.transform.position - realOwner.transform.position).normalized;
        for (int i = 0; i < attackPoints.Count; i++)
        {
            GameObject temp = realOwner.InstantiateVFX("fx_small_arrow", realOwner.defaultTrans, true);
            temp.transform.SetParent(null);
            temp.transform.forward = (attackPoints[i] - realOwner.defaultTrans.position).normalized;
        }
        attackPoints.Clear();
        realOwner.wSkilldelayCo = StartCoroutine(realOwner.WSkillDelayCo());
    }

    public Vector3 DirFromAngle(float angleDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Cos((-angleDegrees + 90) * Mathf.Deg2Rad), 0, Mathf.Sin((-angleDegrees + 90) * Mathf.Deg2Rad));
    }

    void GetPoints()
    {
        int stepCount = Mathf.RoundToInt(attackAngle * resolution);
        float stepAngleSize = attackAngle / stepCount;
        
        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - attackAngle / 2 + stepAngleSize * i;

            Vector3 dir = DirFromAngle(angle, true);
            AttackCastInfo newAttackCast = new AttackCastInfo(transform.position + dir * attackRadius, attackRadius, angle);
            attackPoints.Add(newAttackCast.point);
        }
    }
}

