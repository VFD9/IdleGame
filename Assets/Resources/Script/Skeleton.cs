using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Object, IObject
{
    void Start()
    {
        giveGold = Random.Range(40, 44);
        objectAnimator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        objectAnimator.SetFloat("attackspeed", attackSpeed);
        Attack();
        ChangeDefault();
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(meleePos.position, boxSize);
    }*/

    public void Attack()
    {
        if (Hp > 0)
        {
            if (objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                detectCollider = Physics2D.OverlapBox(meleePos.position, boxSize, 0);

                if (detectCollider != null && detectCollider.CompareTag("Player"))
                    objectAnimator.SetBool("attack", true);
            }
            else if (objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                float normalizedTime = objectAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                float normalizedTimeInProcess = normalizedTime - Mathf.Floor(normalizedTime);

                if (normalizedTimeInProcess > 0.7f &&
                    normalizedTime > atkLoop)
                {
                    atkLoop += 1;
                    detectCollider.GetComponent<IObject>().AttackDamage(Atk);
                    attackSound.Play();

                    if (detectCollider.gameObject.GetComponent<IObject>().currentHp() <= 0)
                    {
                        objectAnimator.SetBool("attack", false);
                        atkLoop = 0;
                        detectCollider = null;
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
        numberText.GetComponent<DamageText>().TakeDamage(dmg, numberText, textPos);
        Hp -= dmg;
    }

    // 아무 영향이 없는 현재 체력메서드
    public float currentHp()
    {
        return Hp;
    }

    // 데미지를 받은 수치만큼 텍스트를 보여주는 메서드
    public float currentHp(float _hp)
    {
        numberText.GetComponent<DamageText>().TakeDamage(Mathf.Abs(_hp), numberText, textPos);
        Hp += _hp;
        return Hp;
    }

    // 스테이지가 오를 수록 체력을 올리기 위해 사용하는 메서드
    public float HpUp(float _hp)
    {
        Hp += _hp;
        defaultHp += _hp;
        return Hp;
    }
}
