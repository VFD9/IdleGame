using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum playerState
{
    Idle, Run, Attack, Death
}

public class Player : Object
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector3 StartPoint;
    [SerializeField] private Vector3 EndPoint;
    [SerializeField] private playerState currentState;
    Color healColor;

    void Start()
    {
        healColor = new Color(0.0f, 255.0f, 0.0f, 255.0f);
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

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(meleePos.position, boxSize);
    }*/

    void Hpbar()
    {
        GameManager.Instance.Hpbar.fillAmount = Hp / defaultHp;
        GameManager.Instance.currentHp.text = Math.Truncate(Hp).ToString();
        GameManager.Instance.fullHp.text = Math.Truncate(defaultHp).ToString();
    }

    void ObjectMove()
    {
        if (detectCollider == null && transform.position != EndPoint)
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
                ObjectPool.Instance.DestroyChild();
                objectAnimator.SetBool("attack", false);
                objectAnimator.SetBool("idle", true);
                Invoke("StageUp", 2.0f);
                break;
            case playerState.Run:
                GameManager.Instance.userSpeed = moveSpeed;
                objectAnimator.SetBool("attack", false);
                objectAnimator.SetBool("idle", false);
                detectCollider = Physics2D.OverlapBox(meleePos.position, boxSize, 0);
                _state = detectCollider != null && detectCollider.CompareTag("Monster") ? playerState.Attack : playerState.Run;
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

    public override void Attack()
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
                    detectCollider.GetComponent<IObject>().AttackDamage(Atk); // 이 객체말고 피격당한 객체가 가진 메서드를 호출함
                    attackSound.Play();
                }
                else if (detectCollider.GetComponent<IObject>().currentHp() <= 0)
                {
                    atkLoop = 0;
                    currentState = playerState.Run;
                    objectAnimator.Play("Run");
                    detectCollider = null;
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

    public override void AttackDamage(float dmg)
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
        numberText.GetComponent<DamageText>().TakeHeal(_hp, numberText, textPos, healColor);

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

    public override float currentAtk()
    {
        return Atk;
    }

    public override float currentAtk(float addAtk)
    {
        Atk += addAtk;
        return Atk;
    }

    void StageUp()
    {
        if (currentState == playerState.Idle)
        {
            resetPos();
            GameManager.Instance.StageUp();
        }
    }

    void resetGame()
    {
        if (currentState == playerState.Death)
        {
            resetPos();
            GameManager.Instance.StageDown();
        }
    }

    void resetPos()
    {
        transform.position = StartPoint;
        currentState = playerState.Run;
        objectAnimator.SetBool("death", false);
        moveSpeed = GameManager.Instance.userSpeed;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        ObjectPool.Instance.DestroyChild();
    }
}
