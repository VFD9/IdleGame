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
        if (Hp > 0)
        {
            if (objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                GetCollider = GetComponent<DetectCollider>().ColliderInfo();

                if (GetCollider != null && GetCollider.gameObject.CompareTag("Player"))
                    objectAnimator.SetBool("attack", true);
            }
            else if (objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                float normalizedTime = objectAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                float normalizedTimeInProcess = normalizedTime - Mathf.Floor(normalizedTime);

                if (normalizedTimeInProcess > 0.85f &&
                    normalizedTime > atkLoop)
                {
                    atkLoop += 1;
                    GetCollider.GetComponent<IAttack>().AttackDamage(Atk);
                    attackSound.Play();

                    if (GetCollider.gameObject.GetComponent<IObject>().currentHp() <= 0)
                    {
                        objectAnimator.SetBool("attack", false);
                        atkLoop = 0;
                        GetCollider = null;
                    }
                }
            }
        }
        else
            Death();
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

    public void AttackDamage(float dmg)
    {
        GameManager.Instance.numberText.TakeDamage(
            dmg, GameManager.Instance.numberText.gameObject, textPos);
        Hp -= dmg;
    }

    // �ƹ� ������ ���� ���� ü�¸޼���
    public override float currentHp()
    {
        return Hp;
    }

    // �������� ���� ��ġ��ŭ �ؽ�Ʈ�� �����ִ� �޼���
    public override float currentHp(float _hp)
    {
        GameManager.Instance.numberText.TakeDamage(
            Mathf.Abs(_hp), GameManager.Instance.numberText.gameObject, textPos);
        Hp += _hp;
        return Hp;
    }

    // ���������� ���� ���� ü���� �ø��� ���� ����ϴ� �޼���
    public override float HpUp(float _hp)
    {
        Hp += _hp;
        defaultHp += _hp;
        return Hp;
    }
}
