using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
struct SkillInfo
{
    public Button skillBtn;
    public float coolDown;
    public float resetTime;
    public float buffTime;
    public float skillPower;
    public Text coolTime;
}

public class Skill : Singleton<Skill>
{
    [SerializeField] private List<SkillInfo> skill;
    [SerializeField] private Thunder thunder;
    Color normalColor;
    Color disableColor;

    void Start()
    {
        normalColor = skill[0].skillBtn.colors.normalColor;
        disableColor = skill[0].skillBtn.colors.disabledColor;
        thunder.setPower(skill[1].skillPower);

        for (int i = 0; i < skill.Count; ++i)
            skill[i].coolTime.text = skill[i].coolDown.ToString();
    }

    public void useSkill(string _name)
    {
        foreach (SkillInfo element in skill)
        {
            if (EventSystem.current.IsPointerOverGameObject() == element.skillBtn.gameObject &&
                element.skillBtn.name == _name)
            {
                element.skillBtn.GetComponent<Image>().color = disableColor;
                element.skillBtn.enabled = false;
                StartCoroutine(runningSkill(element.skillBtn, element.coolTime, element.skillPower,
                    element.coolDown, element.resetTime, element.buffTime));

                if (element.skillBtn.name == "PowerUp") 
                    GameManager.Instance.player.currentAtk(element.skillPower);
                else if (element.skillBtn.name == "Thunder")
                    thunder.SummonThunder(thunder.gameObject, GameManager.Instance.thunderPos);
            }
        }
    }

    IEnumerator runningSkill(Button _Obj, Text _timeText, float _power, float _time, float resetTime, float _buffTime)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(1.0f);

        // while문 위쪽에 스킬 관련 코드를 작성하면 스킬이 한 번 더 써지는 버그가 있어서 useSkill 메서드에 작성함
        while (_Obj.enabled == false)
        {
            if (_buffTime > 0 && _Obj.name == "Heal") 
                GameManager.Instance.player.currentHp(_power);

            _timeText.gameObject.SetActive(true);
            yield return waitForSeconds;

            _buffTime--;
            _timeText.text = _time > 0 ? ((int)--_time).ToString() : 0.ToString();

            if (_buffTime == 0)
            {
                if (_Obj.name == "PowerUp") 
                    GameManager.Instance.player.currentAtk(-_power);
            }

            if (_time == 0)
            {
                Coroutine runningCoroutine = StartCoroutine(runningSkill(_Obj, _timeText, _power, _time, resetTime, _buffTime));
                _timeText.text = resetTime.ToString();
                _Obj.gameObject.GetComponent<Image>().color = normalColor;
                _Obj.enabled = true;
                _timeText.gameObject.SetActive(false);
                StopCoroutine(runningCoroutine);
            }
        }
    }

    public Thunder GetThunder()
    {
        return thunder;
    }
}
