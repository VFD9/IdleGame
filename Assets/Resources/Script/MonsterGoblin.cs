using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGoblin : Object, IAttack
{
    void Start()
    {
        giveGold = Random.Range(28, 32);
        objectAnimator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        objectAnimator.SetFloat("attackspeed", attackSpeed);
        Attack();
        ChangeDefault();
    }

    public void Attack()
    {
        if (Hp <= 0)
        {
            Death();
            return;
        }
        AnimatorStateInfo stateInfo = objectAnimator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Idle"))
            IdleState();
        else if (stateInfo.IsName("Attack"))
            AttackState();
    }

    void IdleState()
    {
        targetCollider = GetComponent<DetectCollider>().ColliderInfo();

        if (targetCollider != null && targetCollider.gameObject.CompareTag("Player"))
            objectAnimator.SetBool("attack", true);
    }

    void AttackState()
    {
        float normalizedTime = objectAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        float normalizedTimeInProcess = normalizedTime - Mathf.Floor(normalizedTime);

        if (normalizedTimeInProcess > 0.85f && normalizedTime > atkLoop)
        {
            atkLoop += 1;
            targetCollider.GetComponent<IAttack>().GetAttackDamage(Atk);
            attackSound.Play();

            IObject targetObject = targetCollider.gameObject.GetComponent<IObject>();
            if (targetObject.currentHp() <= 0)
            {
                objectAnimator.SetBool("attack", false);
                atkLoop = 0;
                targetCollider = null;
            }
        }
    }

    public float currentAtk()
    {
        return Atk;
    }

    public float currentAtk(float addAtk)
    {
        Atk += addAtk;
        return Atk;
    }

    public void GetAttackDamage(float dmg)
    {
        GameManager.Instance.numberText.TakeDamage(
            dmg, GameManager.Instance.numberText.gameObject, textPos);
        Hp -= dmg;
    }

    // 아무 영향이 없는 현재 체력메서드
    public override float currentHp()
    {
        return Hp;
    }

    // 데미지를 받은 수치만큼 텍스트를 보여주는 메서드
    public override float currentHp(float _hp)
    {
        GameManager.Instance.numberText.TakeDamage(
            Mathf.Abs(_hp), GameManager.Instance.numberText.gameObject, textPos);
        Hp += _hp;
        return Hp;
    }

    // 스테이지가 오를 수록 체력을 올리기 위해 사용하는 메서드
    public override float HpUp(float _hp)
    {
        Hp += _hp;
        defaultHp += _hp;
        return Hp;
    }
}
