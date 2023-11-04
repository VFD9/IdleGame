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
        AnimatorStateInfo stateInfo = objectAnimator.GetCurrentAnimatorStateInfo(0);

        if (hp > 0)
        {
            if (stateInfo.IsName("Idle"))
                IdleState();
            else if (stateInfo.IsName("Attack"))
                AttackState();
        }
        else
        {
            Death();
            return;
        }
    }

    void IdleState()
    {
        targetCollider = GetComponent<DetectCollider>().EnemyInfo();

        if (targetCollider != null && targetCollider.gameObject.CompareTag("Player"))
            objectAnimator.SetBool("attack", true);
    }

    void AttackState()
    {
        // 애니메이션이 루프일 경우 1이상의 시간을 가짐
        float normalizedTime = objectAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        // normalizedTime에서 소수점을 버린 Mathf.Floor(normalizedTime)을 빼면 소수점만 남음
        float normalizedTimeInProcess = normalizedTime - Mathf.Floor(normalizedTime);

        // 공격 횟수는 처음엔 0이며 한 번 공격할 때마다 atkLoop에 1을 더해줌
        /* normalizedTimeInProcess만 있으면 0.85f 이상부터는 계속 데미지를 계산해 한 번의 공격에 플레이어가 죽게 되고
        normalizedTime > atkLoop만 있으면 공격 모션보다 데미지가 더 빨리 나와서 의도와 맞지 않게 된다.*/
        if (normalizedTimeInProcess > 0.85f && normalizedTime > atkLoop)
        {
            atkLoop += 1;
            targetCollider.GetComponent<IAttack>().GetAttackDamage(atk); // targetCollider의 체력을 공격력만큼 깎는 메서드를 호출
            SoundManager.Instance.attackSounds[4].audio.Play();

            IObject targetObject = targetCollider.GetComponent<IObject>();
            if (targetObject.CurrentHp() <= 0)  // targetCollider의 현재 체력이 0일 경우
            {
                targetCollider.EmptyCollider2D();
                targetCollider = null;
                objectAnimator.SetBool("attack", false);
                atkLoop = 0;
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

    // 아무 영향이 없는 현재 체력메서드
    public override float CurrentHp()
    {
        return hp;
    }

    // 데미지를 받은 수치만큼 텍스트를 보여주는 메서드
    public override float CurrentHp(float _hp)
    {
        GameManager.Instance.numberText.TakeDamage(
            Mathf.Abs(_hp), GameManager.Instance.numberText.gameObject, textPos);
        hp += _hp;
        return hp;
    }

    // 스테이지가 오를 수록 체력을 올리기 위해 사용하는 메서드
    public override float HpUp(float _hp)
    {
        hp += _hp;
        defaultHp += _hp;
        return hp;
    }
}
