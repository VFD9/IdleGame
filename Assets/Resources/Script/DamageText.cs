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

    // 피격당한 객체가 TakeDamage 메서드를 호출
    public void TakeDamage(float num, GameObject dmgObj, Transform target)
    {
        dmgObj = Instantiate(dmgObj, target.position + offset, Quaternion.identity);
        damageText = dmgObj.GetComponent<TextMesh>();
        // 정수만 나오도록 Math.Truncate()를 사용함
        damageText.text = Math.Truncate(num).ToString();
        damageText.name = "Damage";
        alpha = damageText.color;
    }

    // 회복과 관련된 것을 사용해 체력을 회복할 때 TakeHeal 메서드를 호출
    public void TakeHeal(float num, GameObject dmgObj, Transform target, Color color)
    {
        dmgObj = Instantiate(dmgObj, target.position + offset, Quaternion.identity);
        damageText = dmgObj.GetComponent<TextMesh>();
        // 정수만 나오도록 Math.Truncate()를 사용함
        damageText.text = "+" + Math.Truncate(num).ToString();
        damageText.color = color;
        damageText.name = "Heal";
        alpha = damageText.color;
    }

    // 회복이 아닌 아이템을 사용할 때는 다른 색으로 표시하는 ItemTypeName 메서드를 호출
    public void ItemTypeName(string _name, float num, GameObject dmgObj, Vector3 target, Color color, int fontsize = 20)
    {
        dmgObj = Instantiate(dmgObj, target + new Vector3(0, 1, 0), Quaternion.identity);
        damageText = dmgObj.GetComponent<TextMesh>();
        // 정수만 나오도록 Math.Truncate()를 사용함
        damageText.text = "+" + Math.Truncate(num).ToString() + " " + _name;
        damageText.color = color;
        damageText.fontSize = fontsize;
        damageText.name = _name;
        alpha = damageText.color;
    }

    // 적 객체가 데미지를 받았을 때 텍스트가 위로 올라가게 하는 메서드
    void DmgTextPos()
    {
        transform.Translate(new Vector3(0.0f, moveSpeed * Time.deltaTime, 0.0f));
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed);
        damageText.color = alpha;
    }

    void DestroyObject()
    {
        GameObject dmgObj = gameObject;
        Destroy(dmgObj);
        dmgObj = null;
    }
}
