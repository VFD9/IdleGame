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
    [SerializeField] private Vector3 offset;
    TextMesh damageText;
    Color alpha;

    void Start()
    {
        damageText = GetComponent<TextMesh>();
        alpha = damageText.color;
        Invoke("DestroyObject", destroyTime);
    }

    void Update()
    {
        DmgTextPos();
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

    public void TakeName(string _name, float num, GameObject dmgObj, Vector3 target, Color color, int fontsize = 20)
    {
        dmgObj = Instantiate(dmgObj, target + new Vector3(0, 1, 0), Quaternion.identity);
        damageText = dmgObj.GetComponent<TextMesh>();
        damageText.text = "+" + Math.Truncate(num).ToString() + " " + _name;
        damageText.color = color;
        damageText.fontSize = fontsize;
        damageText.name = _name;
        alpha = damageText.color;
    }

    void DmgTextPos()
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
