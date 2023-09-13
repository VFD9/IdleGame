using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DamageText : MonoBehaviour
{
    // 텍스트가 위로 뜨면서 사라지게할 float 변수 3개
    [SerializeField] private float moveSpeed;
    [SerializeField] private float alphaSpeed;
    [SerializeField] private float destroyTime;

    // 이 스크립트를 가지고 있는 객체를 변화시킬 변수들 선언
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

    // 피격당한 객체가 TakeDamage 메서드를 호출
    public void TakeDamage(float num, GameObject dmgObj, Transform target)
    {
        dmgObj = Instantiate(dmgObj, target.position + offset, Quaternion.identity);
        damageText = dmgObj.GetComponent<TextMesh>();
        damageText.text = Math.Truncate(num).ToString();
        damageText.name = "Damage";
        alpha = damageText.color;
    }

    // 피격당한 객체가 회복할 때 TakeHeal 메서드를 호출
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
