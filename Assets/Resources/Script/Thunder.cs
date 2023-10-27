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

        // ����ġ�� �ִϸ��̼��� ������ ��ü�� ���� ��� �� ������
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            boxCollider.enabled = true;
            gameObject.SetActive(false);
            Invoke("DestroyObject", destroyTime);
            return;
        }
    }

    // ������ 2d �ڽ� �ݶ��̴��� �޾� ũ�⸦ ���ϰ�, �浹�� ���� �� �浹�� ��ü�� ü�¿� ������ ��
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            collision.gameObject.GetComponent<IObject>().CurrentHp(-damage);
            boxCollider.enabled = false;
        }
    }

    // �Ѳ����� 5���� ������ ��ȯ�ϴ� �޼���
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
