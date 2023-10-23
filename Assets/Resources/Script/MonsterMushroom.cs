using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMushroom : Object
{
    void Start()
    {
        giveGold = Random.Range(32, 36);
        objectAnimator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        objectAnimator.SetFloat("attackspeed", attackSpeed);
        Attack();
        ChangeDefault();
    }

    public override void Attack()
    {
        if (hp <= 0)
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
            targetCollider.GetComponent<IAttack>().GetAttackDamage(atk);
            attackSound.Play();

            IObject targetObject = targetCollider.gameObject.GetComponent<IObject>();
            if (targetObject.CurrentHp() <= 0)
            {
                objectAnimator.SetBool("attack", false);
                atkLoop = 0;
                targetCollider = null;
            }
        }
    }

    public override float CurrentAtk()
    {
        return atk;
    }

    public override float CurrentAtk(float addAtk)
    {
        atk += addAtk;
        return atk;
    }

    public override void GetAttackDamage(float dmg)
    {
        GameManager.Instance.numberText.TakeDamage(
            dmg, GameManager.Instance.numberText.gameObject, textPos);
        hp -= dmg;
    }

    // �ƹ� ������ ���� ���� ü�¸޼���
    public override float CurrentHp()
    {
        return hp;
    }

    // �������� ���� ��ġ��ŭ �ؽ�Ʈ�� �����ִ� �޼���
    public override float CurrentHp(float _hp)
    {
        GameManager.Instance.numberText.TakeDamage(
            Mathf.Abs(_hp), GameManager.Instance.numberText.gameObject, textPos);
        hp += _hp;
        return hp;
    }

    // ���������� ���� ���� ü���� �ø��� ���� ����ϴ� �޼���
    public override float HpUp(float _hp)
    {
        hp += _hp;
        defaultHp += _hp;
        return hp;
    }
}
