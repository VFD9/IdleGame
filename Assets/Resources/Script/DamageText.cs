using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DamageText : MonoBehaviour
{
    // �ؽ�Ʈ�� ���� �߸鼭 ��������� float ���� 3��
    [SerializeField] private float moveSpeed;
    [SerializeField] private float alphaSpeed;
    [SerializeField] private float destroyTime;

    // �� ��ũ��Ʈ�� ������ �ִ� ��ü�� ��ȭ��ų ������ ����
    TextMesh damageText;
    Vector3 offset;
    Color alpha;

    void Start()
    {
        offset = new Vector3(0.0f, 0.5f, 0.0f);
        damageText = GetComponent<TextMesh>();
        alpha = damageText.color;
        Invoke("DestroyObject", destroyTime);
    }

    void Update()
    {
        FixedPos();
    }

    // �ǰݴ��� ��ü�� TakeDamage �޼��带 ȣ��
    public void TakeDamage(float num, GameObject dmgObj, Transform target)
    {
        dmgObj = Instantiate(dmgObj, target.position + offset, Quaternion.identity);
        damageText = dmgObj.GetComponent<TextMesh>();
        damageText.text = Math.Truncate(num).ToString();
        damageText.name = "Damage";
        alpha = damageText.color;
    }

    // �ǰݴ��� ��ü�� ȸ���� �� TakeHeal �޼��带 ȣ��
    public void TakeHeal(float num, GameObject dmgObj, Transform target, Color color)
    {
        dmgObj = Instantiate(dmgObj, target.position + offset, Quaternion.identity);
        damageText = dmgObj.GetComponent<TextMesh>();
        damageText.text = "+" + Math.Truncate(num).ToString();
        damageText.color = color;
        damageText.name = "Heal";
        alpha = damageText.color;
    }

    void FixedPos()
    {
        transform.Translate(new Vector3(0.0f, moveSpeed * Time.deltaTime, 0.0f));
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        damageText.color = alpha;
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
