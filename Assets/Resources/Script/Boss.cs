using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Object
{
    [SerializeField] private GameObject spell;
    float random;

    void Start()
    {
        giveGold += 550;
        objectAnimator = GetComponent<Animator>();
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
            else
            {
                if (random > 0.9f)
                {
                    objectAnimator.SetBool("cast", true);
                    CastState();
                }
                else
                    AttackState();
            }
        }
        else
        {
            objectAnimator.SetBool("death", true);
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
        if (objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            // 애니메이션이 루프일 경우 1이상의 시간을 가짐
            float normalizedTime = objectAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            // normalizedTime에서 소수점을 버린 Mathf.Floor(normalizedTime)을 빼면 소수점만 남음
            float normalizedTimeInProcess = normalizedTime - Mathf.Floor(normalizedTime);

            if (normalizedTimeInProcess > 0.625f && normalizedTime > atkLoop)
            {
                atkLoop += 1;
                targetCollider.GetComponent<IAttack>().GetAttackDamage(atk); // targetCollider의 체력을 공격력만큼 깎는 메서드를 호출
                SoundManager.Instance.attackSounds[1].audio.Play();
                random = Random.Range(0.3f, 1.0f);

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
    }

    void CastState()
    {
        /* AttackState와 CastState에 if문을 넣지 않으면 
        루프가 아닌 CastState의 normalizedTime의 값이 1.0f보다 커져서 보스가 공격할 때 소리가 나지않는 현상이 발생 */
        if (objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Cast"))
        {
            float normalizedTime = objectAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;

            if (normalizedTime >= 1.0f)
            {
                SoundManager.Instance.attackSounds[6].audio.Play();
                GameObject spellObj = Instantiate(spell, new Vector3(
                    transform.position.x - 0.11f, spell.transform.position.y, spell.transform.position.z),
                    Quaternion.identity);
                spellObj.name = spell.name;
                // random 값에 Random.Range를 설정하지 않으면 spellObj가 하나만 만들어지는 것이 아닌 많이 만들어짐
                random = Random.Range(0.3f, 1.0f);
                objectAnimator.SetBool("cast", false);
                // atkLoop를 0으로 초기화하지 않으면 보스가 공격해도 AttackState의 normalizedTime만큼 소리가 나지 않음
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

    public override float CurrentHp()
    {
        return hp;
    }

    public override float CurrentHp(float _hp)
    {
        GameManager.Instance.numberText.TakeDamage(
            Mathf.Abs(_hp), GameManager.Instance.numberText.gameObject, textPos);
        hp += _hp;
        return hp;
    }

    public override float HpUp(float _hp)
    {
        hp += _hp;
        defaultHp += _hp;
        return hp;
    }
}
