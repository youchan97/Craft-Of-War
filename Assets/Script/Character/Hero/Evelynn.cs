using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evelynn : Hero
{
    public int attackRange = 3;
    [SerializeField] List<Skill> skillList;
    public Transform defaultTrans;

    Coroutine attackdelayCo;
    public Coroutine qSkilldelayCo;
    public Coroutine wSkilldelayCo;
    public Coroutine eSkilldelayCo;
    public Coroutine rSkilldelayCo;
    public int attackCount;

    public override void Awake()
    {
        base.Awake();
        sm = new StateMachine<Character>(this);

        skillDic = new Dictionary<int, Skill>();
        skillDic.Add((int)SKILL_TYPE.QSkill, skillList[(int)SKILL_TYPE.QSkill]);
        skillDic.Add((int)SKILL_TYPE.WSkill, skillList[(int)SKILL_TYPE.WSkill]);
        skillDic.Add((int)SKILL_TYPE.ESkill, skillList[(int)SKILL_TYPE.ESkill]);
        skillDic.Add((int)SKILL_TYPE.RSkill, skillList[(int)SKILL_TYPE.RSkill]);

        foreach (var keyValue in skillDic)
        {
            keyValue.Value.SetOwner(this);
        }

        //for (int i = 0; i < UIManager.Instance.skillSlots.Length; i++)
        //{
        //    UIManager.Instance.skillSlots[i].Init(this);
        //}
        InitStats();
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.PlayerHero != this) return;

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                if (Vector3.Distance(hitInfo.transform.position, transform.position) > attackRange)
                    return;

                if (hitInfo.transform.gameObject.TryGetComponent(out Character target))
                {
                    if (target == this) return;

                    clickTarget = target;
                    curState = HERO_STATE.ATTACK;
                    agent.isStopped = true;
                    Attack(target, target.transform);
                }
            }
        }
        //if (Input.GetKeyDown(KeyCode.Q)) Debug.Log("이블린 Q");
        //if (Input.GetKeyDown(KeyCode.W)) Debug.Log("이블린 W");
        if (Input.GetKeyDown(KeyCode.E))
        {
            UseSkill(SKILL_TYPE.ESkill);
        }
        //if (Input.GetKeyDown(KeyCode.R)) Debug.Log("이블린 R");
    }
    public override void Update()
    {
        base.Update();
    }
    public override void InitStats()
    {
        base.InitStats();
        info.MaxHp = 200;
        info.CurentHp = info.MaxHp;
        info.Atk = 10;
        info.Def = 10;
        MoveSpeed = 10f;
        info.AtkSpeed = 1f;
        info.AtkRange = 10f;
        Agent.speed = MoveSpeed;
        Agent.angularSpeed = 1200f;
    }
    public override void Attack(IHitAble target)
    {
        base.Attack(target);
    }

    public override void UseSkill(SKILL_TYPE skillType)
    {
        UIManager.Instance.skillSlots[(int)skillType].TrySkillActive();
    }


    public override void Die()
    {

    }

    public override void Hit(IAttackAble attacker)
    {
        this.Hp -= attacker.Atk;
    }

    public override void Attack(IHitAble target, Transform targetTrans)
    {
        base.Attack(target);
        animator.SetBool("AttackBasic", true);
        transform.forward = (targetTrans.position - this.transform.position).normalized;
        InstantiateVFX("fx_morgana_skill04", defaultTrans, true);
        attackdelayCo = StartCoroutine(AttackDelayCo());
    }

    public GameObject InstantiateVFX(string prefabName, Transform transform, bool setPanrentNull = false)
    {
        GameObject temp = (GameObject)Instantiate(Resources.Load("VFX/" + prefabName), transform, false); //옵젝풀로 바꿔주는게 좋을까나
        if (setPanrentNull)
            temp.transform.SetParent(null);

        return temp;
    }

    IEnumerator AttackDelayCo()
    {
        yield return new WaitForSeconds(0.5f);
        curState = HERO_STATE.IDLE;
        animator.SetBool("AttackBasic", false);
        agent.isStopped = false;
        StopCoroutine(attackdelayCo);
        yield return null;
    }

    public IEnumerator QSkillDelayCo()
    {
        yield return new WaitForSeconds(0.5f);
        curState = HERO_STATE.IDLE;
        animator.SetBool("QSkill", false);
        agent.isStopped = false;
        StopCoroutine(qSkilldelayCo);
        yield return null;
    }

    public IEnumerator WSkillDelayCo()
    {
        yield return new WaitForSeconds(0.5f);
        curState = HERO_STATE.IDLE;
        animator.SetBool("WSkill", false);
        agent.isStopped = false;
        StopCoroutine(wSkilldelayCo);
        yield return null;
    }
    public IEnumerator ESkillDelayCo()
    {
        yield return new WaitForSeconds(1f);
        curState = HERO_STATE.IDLE;
        InstantiateVFX("fx_morgana_skill04", defaultTrans, true);
        StopCoroutine(eSkilldelayCo);
        yield return null;
    }

    public IEnumerator RSkillDelayCo()
    {
        yield return new WaitForSeconds(0.5f);
        curState = HERO_STATE.IDLE;
        animator.SetBool("RSkill", false);
        agent.isStopped = false;
        StopCoroutine(rSkilldelayCo);
        yield return null;
    }
}
