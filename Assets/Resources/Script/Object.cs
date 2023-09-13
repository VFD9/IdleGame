using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Object : MonoBehaviour, IObject
{
    [SerializeField] protected string objectName;
    [SerializeField] protected float Hp;
    [SerializeField] protected float Atk;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected AudioSource attackSound;
    [SerializeField] protected AudioSource deadSound;
    [SerializeField] protected GameObject numberText;
    [SerializeField] protected Transform textPos;
    [SerializeField] protected DropItem itembox;

    protected int atkLoop;
    protected int giveGold;
    protected float defaultHp;
    protected float defaultAtk;
    protected Animator objectAnimator;
    protected DetectCollider GetCollider;

    void Start()
    {
        defaultHp = Hp;
        defaultAtk = Atk;
        atkLoop = 0;
    }

    protected void ChangeDefault()
    {
        if (Hp > defaultHp && Hp < 100000)
            defaultHp = Hp;

        if (Atk > defaultAtk && Atk < 10000)
            defaultAtk = Atk;
    }

    public void Death()
    {
        GetCollider = null;
        Hp = 0;
        atkLoop = 0;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine(afterDeath());

        if (gameObject.CompareTag("Monster"))
        {
            objectAnimator.SetBool("attack", false);
            objectAnimator.SetBool("death", true);

            if (objectAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.0f)
            {
                GameManager.Instance.curGold[0] += giveGold;
                deadSound.Play();
                itembox.DropTable(transform.position);
            }
        }
    }

    public IEnumerator afterDeath()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(1.5f);

        if (objectAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            if (objectAnimator.CompareTag("Monster"))
            {
                yield return waitForSeconds;
                Hp = defaultHp;
                ObjectPool.Instance.PullObject(gameObject);
            }
            else if (objectAnimator.CompareTag("Player"))
            {
                yield return waitForSeconds;
                Hp = defaultHp;
            }
        }
    }

    public abstract float currentHp();
    public abstract float currentHp(float _hp);
    public abstract float HpUp(float _hp);
}
