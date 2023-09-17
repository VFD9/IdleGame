using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : MonoBehaviour
{
    [SerializeField] private float destroyTime;
    [SerializeField] private float damage;
    [SerializeField] private AudioSource thunderSound;
    Animator animator;
    BoxCollider2D boxCollider;

    void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        Invoke("DestroyObject", destroyTime);
    }

    void Update()
    {
        if (gameObject.activeInHierarchy == true)
        {
            transform.position = Vector3.Lerp(transform.position, transform.position, 0.0f);
            GameManager.Instance.thunderPos.position = Vector3.Lerp(GameManager.Instance.thunderPos.position,
                GameManager.Instance.thunderPos.position, 0.0f);
        }

        // 번개치는 애니메이션이 끝나면 객체를 꺼버림
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            gameObject.SetActive(false);
            boxCollider.enabled = true;
        }
    }

    // 번개에 2d 박스 콜라이더를 달아 크기를 정하고, 충돌이 끝날 때 충돌한 객체의 체력에 영향을 줌
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            collision.gameObject.GetComponent<IObject>().currentHp(-damage);
            boxCollider.enabled = false;
        }
    }

    // TODO : 번개 공격시 넣은 값만큼의 데미지를 줄 수 있도록 수정해야함
    public void SummonThunder(GameObject thunderObj, Transform targetPos, float _damage)
    {
        for (int i = 0; i < 5; ++i)
        {
            thunderObj = Instantiate(thunderObj);
            thunderObj.transform.position = new Vector3(targetPos.position.x + (1.5f * i), targetPos.position.y - 0.4f, 0.0f);
            thunderObj.name = "Thunder";
        }
    }

    public void SummonThunder(GameObject thunderObj, float _x, float _y, float _damage)
    {
        for (int i = 0; i < 5; ++i)
        {
            thunderObj = Instantiate(thunderObj);
            thunderObj.transform.position = new Vector3(_x + (1.5f * i), _y - 0.4f, 0.0f);
            thunderObj.name = "Thunder";
        }
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }

    public float setPower(float _dmg)
    {
        damage = _dmg;
        return damage;
    }

    public float addPower(float _adddmg)
    {
        damage += _adddmg;
        return damage;
    }

    public Thunder GetThunder()
    {
        return gameObject.GetComponent<Thunder>();
    }
}
