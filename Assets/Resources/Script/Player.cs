using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum PlayerState
{
    Idle, Run, Attack, Death
}

public class Player : Object
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform invenParent;
    [SerializeField] private Vector3 startPoint;
    [SerializeField] private Vector3 endPoint;
    [SerializeField] private PlayerState currentState;
    float prevHp;
    float prevdefaultHp;

    void Start()
    {
        prevHp = hp;
        prevdefaultHp = defaultHp;
        currentState = PlayerState.Run;
        objectAnimator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        // Update�� ���� ��� �ε�ε� �����鼭 �̵��ϱ� ������ FixedUpdate�� �־���
        ObjectMove();
    }

    void Update()
    {
        objectAnimator.SetFloat("attackspeed", attackSpeed);
        ChangeState(currentState);
        Attack();
        ChangeDefault();
        Hpbar();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            collision.transform.SetParent(invenParent);
            collision.gameObject.SetActive(false);
            // �÷��̾�� �浹�� ��ü ������ �޾ƿ´�.
            GameManager.Instance.inventory.GetItem(collision.gameObject.GetComponent<Item>());
            // ������ ������ �޾ƿ����� �κ��丮 ������ �����Ѵ�.
            GameManager.Instance.inventory.SlotItemsUI();
        }
    }

    void Hpbar()
    {
        if (prevHp != hp || prevdefaultHp != defaultHp)
        {
            GameManager.Instance.hpBar.fillAmount = hp / defaultHp;
            GameManager.Instance.currentHp.text = Math.Truncate(hp).ToString();
            GameManager.Instance.fullHp.text = Math.Truncate(defaultHp).ToString();
        }
    }

    void ObjectMove()
    {
        if (targetCollider == null && transform.position != endPoint)
        {
            transform.position = Vector2.MoveTowards(
                transform.position, endPoint, moveSpeed * Time.deltaTime);
        }
        else if (transform.position == endPoint)
            ChangeState(PlayerState.Idle);
    }

    void setState(PlayerState _state)
    {
        currentState = _state;
    }

    void ChangeState(PlayerState _state)
    {
        switch (_state)
        {
            case PlayerState.Idle:
                objectAnimator.SetBool("attack", false);
                objectAnimator.SetBool("idle", true);
                Invoke("StageUp", 2.0f);
                break;
            case PlayerState.Run:
                GameManager.Instance.userSpeed = GameManager.Instance.userSpeed != moveSpeed ? moveSpeed : GameManager.Instance.userSpeed;
                atkLoop = 0;
                objectAnimator.SetBool("attack", false);
                objectAnimator.SetBool("idle", false);
                targetCollider = GetComponent<DetectCollider>().EnemyInfo();
                // targetCollider�� ������ ���� �� ������Ʈ�� ������ ���̹Ƿ� �����ϰ� �ƴ϶�� �ٽ� �޸���.
                _state = targetCollider != null ? PlayerState.Attack : PlayerState.Run;
                break;
            case PlayerState.Attack:
                objectAnimator.SetBool("attack", true);
                objectAnimator.SetBool("idle", false);
                break;
            case PlayerState.Death:
                Invoke("ResetGame", 2.0f);
                moveSpeed = 0.0f;
                objectAnimator.SetBool("attack", false);
                objectAnimator.SetBool("idle", false);
                objectAnimator.SetBool("death", true);
                break;
            default:
                break;
        }
        setState(_state);
    }

    public override void Attack()
    {
        if (hp <= 0)
        {
            ChangeState(PlayerState.Death);
            Death();
            return;
        }
        AnimatorStateInfo stateInfo = objectAnimator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Attack"))
        {
            // �ִϸ��̼��� ������ ��� 1�̻��� �ð��� ����
            float normalizedTime = objectAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            // normalizedTime���� �Ҽ����� ���� Mathf.Floor(normalizedTime)�� ���� �Ҽ����� ����
            float normalizedTimeInProcess = normalizedTime - Mathf.Floor(normalizedTime);

            // ���� �����̸鼭 targetCollider�� ���� ü���� 0������ ���
            if (normalizedTime >= 0.0f && targetCollider.GetComponent<IObject>().CurrentHp() <= 0)
            {
                targetCollider.EmptyCollider2D();
                targetCollider = null;
                ChangeState(PlayerState.Run);
                objectAnimator.Play("Run");
                return;
            }

            // ���� Ƚ���� ó���� 0�̸� �� �� ������ ������ atkLoop�� 1�� ������
            /* normalizedTimeInProcess�� ������ 0.8f �̻���ʹ� ��� �������� ����ϰ�
            normalizedTime > atkLoop�� ������ ���� ��Ǻ��� �������� �� ���� ���ͼ� �ǵ��� ���� �ʰ� �ȴ�.*/
            if (normalizedTimeInProcess >= 0.8f && normalizedTime > atkLoop)
            {
                atkLoop += 1;
                targetCollider.GetComponent<IAttack>().GetAttackDamage(atk); // targetCollider�� ü���� ���ݷ¸�ŭ ��� �޼��带 ȣ��
                attackSound.Play();
            }
        }
    }

    public float getMoveSpeed(float _speed)
    {
        moveSpeed += _speed;
        return moveSpeed;
    }

    public float getAttackSpeed(float _atkspd)
    {
        attackSpeed += _atkspd;
        return attackSpeed;
    }

    public override void GetAttackDamage(float dmg)
    {
        if (hp > 0)
            hp -= dmg;
    }

    public override float CurrentHp()
    {
        return hp;
    }

    public override float CurrentHp(float _hp)
    {
        GameManager.Instance.numberText.TakeHeal(
            _hp, GameManager.Instance.numberText.gameObject, textPos, new Color(0.0f, 255.0f, 0.0f, 255.0f));

        if (hp + _hp > defaultHp)
            _hp -= hp + _hp - defaultHp;

        hp += _hp;
        return hp;
    }

    public override float HpUp(float _hp)
    {
        hp += _hp;
        defaultHp += _hp;
        return hp;
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

    void StageUp()
    {
        if (currentState.Equals(PlayerState.Idle))
        {
            ResetPosition();
            ObjectSpawn.Instance.StageUp();
        }
    }

    void ResetGame()
    {
        if (currentState.Equals(PlayerState.Death))
        {
            ResetPosition();
            ObjectSpawn.Instance.StageDown();
        }
    }

    void ResetPosition()
    {
        transform.position = startPoint;
        currentState = PlayerState.Run;
        objectAnimator.SetBool("death", false);
        moveSpeed = GameManager.Instance.userSpeed;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }
}
