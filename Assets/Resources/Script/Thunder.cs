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

        // ����ġ�� �ִϸ��̼��� ������ ��ü�� ������
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            gameObject.SetActive(false);
            boxCollider.enabled = true;
        }
    }

    // ������ 2d �ڽ� �ݶ��̴��� �޾� ũ�⸦ ���ϰ�, �浹�� ���� �� �浹�� ��ü�� ü�¿� ������ ��
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Monster"))
        {
            collision.gameObject.GetComponent<IObject>().currentHp(-damage);
            boxCollider.enabled = false;
        }
    }

    public void SummonThunder(GameObject thunderObj, Transform targetPos)
    {
        //thunderObj = Instantiate(thunderObj, GameManager.Instance.thunderParent);
        //thunderObj.transform.position = new Vector3(targetPos.position.x, targetPos.position.y - 0.4f, 0.0f);
        //thunderObj.name = "Thunder";

        for (int i = 0; i < 3; ++i)
        {
            thunderObj = Instantiate(thunderObj, GameManager.Instance.thunderParent);
            thunderObj.transform.position = new Vector3(targetPos.position.x + (1.5f * i), targetPos.position.y - 0.4f, 0.0f);
            thunderObj.name = "Thunder";
        }
    }

    public void SummonThunder(GameObject thunderObj, float _x, float _y)
    {
        thunderObj = Instantiate(thunderObj, GameManager.Instance.thunderParent);
        thunderObj.transform.position = new Vector3(_x, _y - 0.4f, 0.0f);
        thunderObj.name = "Thunder";

        //for (int i = 0; i < 2; ++i)
        //{
        //    thunderObj = Instantiate(thunderObj, GameManager.Instance.thunderParent);
        //    thunderObj.transform.position = new Vector3(_x + (1.5f * i), _y - 0.4f, 0.0f);
        //    thunderObj.name = "Thunder";
        //}
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
}
