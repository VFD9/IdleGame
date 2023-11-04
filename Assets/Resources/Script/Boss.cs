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
            // �ִϸ��̼��� ������ ��� 1�̻��� �ð��� ����
            float normalizedTime = objectAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            // normalizedTime���� �Ҽ����� ���� Mathf.Floor(normalizedTime)�� ���� �Ҽ����� ����
            float normalizedTimeInProcess = normalizedTime - Mathf.Floor(normalizedTime);

            if (normalizedTimeInProcess > 0.625f && normalizedTime > atkLoop)
            {
                atkLoop += 1;
                targetCollider.GetComponent<IAttack>().GetAttackDamage(atk); // targetCollider�� ü���� ���ݷ¸�ŭ ��� �޼��带 ȣ��
                SoundManager.Instance.attackSounds[1].audio.Play();
                random = Random.Range(0.3f, 1.0f);

                IObject targetObject = targetCollider.GetComponent<IObject>();
                if (targetObject.CurrentHp() <= 0)  // targetCollider�� ���� ü���� 0�� ���
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
        /* AttackState�� CastState�� if���� ���� ������ 
        ������ �ƴ� CastState�� normalizedTime�� ���� 1.0f���� Ŀ���� ������ ������ �� �Ҹ��� �����ʴ� ������ �߻� */
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
                // random ���� Random.Range�� �������� ������ spellObj�� �ϳ��� ��������� ���� �ƴ� ���� �������
                random = Random.Range(0.3f, 1.0f);
                objectAnimator.SetBool("cast", false);
                // atkLoop�� 0���� �ʱ�ȭ���� ������ ������ �����ص� AttackState�� normalizedTime��ŭ �Ҹ��� ���� ����
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
