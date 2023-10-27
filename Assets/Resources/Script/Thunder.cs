using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : MonoBehaviour
{
    [SerializeField] private float destroyTime;
    [SerializeField] private float damage;
    [SerializeField] private AudioSource thunderSound;
    [SerializeField] private BoxCollider2D boxCollider;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (gameObject.activeInHierarchy == true)
        {
            transform.position = Vector3.Lerp(transform.position, transform.position, 0.0f);
            GameManager.Instance.thunderPos.position = Vector3.Lerp(GameManager.Instance.thunderPos.position,
                GameManager.Instance.thunderPos.position, 0.0f);
        }

        // 번개치는 애니메이션이 끝나면 객체를 끄고 잠시 후 삭제함
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            boxCollider.enabled = true;
            gameObject.SetActive(false);
            Invoke("DestroyObject", destroyTime);
            return;
        }
    }

    // 번개에 2d 박스 콜라이더를 달아 크기를 정하고, 충돌이 끝날 때 충돌한 객체의 체력에 영향을 줌
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            collision.gameObject.GetComponent<IObject>().CurrentHp(-damage);
            boxCollider.enabled = false;
        }
    }

    // 한꺼번에 5개의 번개를 소환하는 메서드
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
        GameObject thunder = gameObject;
        Destroy(thunder);
        thunder = null;
    }

    public float SetPower(float _dmg)
    {
        damage = _dmg;
        return damage;
    }

    public float AddPower(float _adddmg)
    {
        damage += _adddmg;
        return damage;
    }

    public Thunder GetThunder()
    {
        return GetComponent<Thunder>();
    }
}
