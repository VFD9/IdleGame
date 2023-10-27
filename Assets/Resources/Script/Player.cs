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
        // Update에 넣을 경우 부들부들 떨리면서 이동하기 때문에 FixedUpdate에 넣었음
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
            // 플레이어와 충돌한 객체 정보를 받아온다.
            GameManager.Instance.inventory.GetItem(collision.gameObject.GetComponent<Item>());
            // 아이템 정보를 받아왔으니 인벤토리 슬롯을 갱신한다.
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
                // targetCollider가 존재할 때는 적 오브젝트가 감지된 것이므로 공격하고 아니라면 다시 달린다.
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
            // 애니메이션이 루프일 경우 1이상의 시간을 가짐
            float normalizedTime = objectAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            // normalizedTime에서 소수점을 버린 Mathf.Floor(normalizedTime)을 빼면 소수점만 남음
            float normalizedTimeInProcess = normalizedTime - Mathf.Floor(normalizedTime);

            // 공격 상태이면서 targetCollider의 현재 체력이 0이하일 경우
            if (normalizedTime >= 0.0f && targetCollider.GetComponent<IObject>().CurrentHp() <= 0)
            {
                targetCollider.EmptyCollider2D();
                targetCollider = null;
                ChangeState(PlayerState.Run);
                objectAnimator.Play("Run");
                return;
            }

            // 공격 횟수는 처음엔 0이며 한 번 공격할 때마다 atkLoop에 1을 더해줌
            /* normalizedTimeInProcess만 있으면 0.8f 이상부터는 계속 데미지를 계산하고
            normalizedTime > atkLoop만 있으면 공격 모션보다 데미지가 더 빨리 나와서 의도와 맞지 않게 된다.*/
            if (normalizedTimeInProcess >= 0.8f && normalizedTime > atkLoop)
            {
                atkLoop += 1;
                targetCollider.GetComponent<IAttack>().GetAttackDamage(atk); // targetCollider의 체력을 공격력만큼 깎는 메서드를 호출
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
