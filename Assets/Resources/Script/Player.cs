using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum playerState
{
    Idle, Run, Attack, Death
}

public class Player : Object, IAttack
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform invenParent;
    [SerializeField] private Vector3 StartPoint;
    [SerializeField] private Vector3 EndPoint;
    [SerializeField] private playerState currentState;
    float prevHp;
    float prevdefaultHp;

    void Start()
    {
        prevHp = Hp;
        prevdefaultHp = defaultHp;
        currentState = playerState.Run;
        objectAnimator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
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
            GameManager.Instance.inventory.GetItem(collision.gameObject.GetComponent<Item>());
            GameManager.Instance.inventory.SlotItemsUI();
        }
    }

    void Hpbar()
    {
        if (prevHp != Hp || prevdefaultHp != defaultHp)
        {
            GameManager.Instance.Hpbar.fillAmount = Hp / defaultHp;
            GameManager.Instance.currentHp.text = Math.Truncate(Hp).ToString();
            GameManager.Instance.fullHp.text = Math.Truncate(defaultHp).ToString();
        }
    }

    void ObjectMove()
    {
        if (targetCollider == null && transform.position != EndPoint)
        {
            transform.position = Vector2.MoveTowards(
                transform.position, EndPoint, moveSpeed * Time.deltaTime);
        }
        else if (transform.position == EndPoint)
            currentState = playerState.Idle;
    }

    void setState(playerState _state)
    {
        currentState = _state;
    }

    void ChangeState(playerState _state)
    {
        switch (_state)
        {
            case playerState.Idle:
                objectAnimator.SetBool("attack", false);
                objectAnimator.SetBool("idle", true);
                Invoke("StageUp", 2.0f);
                break;
            case playerState.Run:
                GameManager.Instance.userSpeed = moveSpeed;
                objectAnimator.SetBool("attack", false);
                objectAnimator.SetBool("idle", false);
                targetCollider = GetComponent<DetectCollider>().ColliderInfo();
                _state = targetCollider != null ? (targetCollider.gameObject.CompareTag("Monster") ? playerState.Attack : playerState.Run) : playerState.Run;
                break;
            case playerState.Attack:
                objectAnimator.SetBool("attack", true);
                objectAnimator.SetBool("idle", false);
                break;
            case playerState.Death:
                Invoke("resetGame", 2.0f);
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

    public void Attack()
    {
        if (Hp > 0)
        {
            if (objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                float normalizedTime = objectAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                float normalizedTimeInProcess = normalizedTime - Mathf.Floor(normalizedTime);

                if (normalizedTimeInProcess >= 0.8f &&
                    normalizedTime > atkLoop)
                {
                    atkLoop += 1;
                    targetCollider.GetComponent<IAttack>().GetAttackDamage(Atk); // �� ��ü���� �ǰݴ��� ��ü�� ���� �޼��带 ȣ����
                    attackSound.Play();
                }
                else if (targetCollider.GetComponent<IObject>().currentHp() <= 0)
                {
                    atkLoop = 0;
                    currentState = playerState.Run;
                    objectAnimator.Play("Run");
                    targetCollider = null;
                }
            }
        }
        else
        {
            currentState = playerState.Death;
            Death();
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

    public void GetAttackDamage(float dmg)
    {
        if (Hp > 0)
            Hp -= dmg;
    }

    public override float currentHp()
    {
        return Hp;
    }

    public override float currentHp(float _hp)
    {
        GameManager.Instance.numberText.TakeHeal(
            _hp, GameManager.Instance.numberText.gameObject, textPos, new Color(0.0f, 255.0f, 0.0f, 255.0f));

        if (Hp + _hp > defaultHp)
            _hp -= Hp + _hp - defaultHp;

        Hp += _hp;
        return Hp;
    }

    public override float HpUp(float _hp)
    {
        Hp += _hp;
        defaultHp += _hp;
        return Hp;
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

    void StageUp()
    {
        if (currentState.Equals(playerState.Idle))
        {
            resetPos();
            ObjectSpawn.Instance.StageUp();
        }
    }

    void resetGame()
    {
        if (currentState.Equals(playerState.Death))
        {
            resetPos();
            ObjectSpawn.Instance.StageDown();
        }
    }

    void resetPos()
    {
        transform.position = StartPoint;
        currentState = playerState.Run;
        objectAnimator.SetBool("death", false);
        moveSpeed = GameManager.Instance.userSpeed;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
    }
}
