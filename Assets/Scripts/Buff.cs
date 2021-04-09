using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    public string buff_name;
    public float duration;
    public Sprite icon;
    public GameObject caster;//시전자
    public GameObject target;//타겟
    public int accuracy;//명중%
    public int armor;//장갑%
    public int critrate;//치명률-%
    public int dmg;//공격력%
    public int evasion;//회피%
    public int speed;//이속%
    public int rateoffire;//공속-초당

    [Header("지속 딜/힐")]
    public bool continuous;
    public int HP;//체력
    public float tick = 0.33f;

    [Header("스턴")]
    public bool stun = false;
    public float stun_duration;

    [Header("역장")]
    public bool forceshield = false;
    public float shield_duration;

    public void Initialized(string name, float _duration, Sprite _icon, GameObject _caster, GameObject _target,
            int _dmg = 0, int _speed = 0, int _armor = 0, int _rateoffire = 0, int _accuracy = 0, int _evasion = 0, 
            int cRate = 0, bool conti = false, int hp = 0, bool _stun = false, float _stun_duration = 0, bool _forceshield = false, float _shield_duration = 0) {

        if (!_target.activeSelf) {
            transform.parent = GameObject.Find("InGameManager").transform;
            gameObject.SetActive(false);
            return;
        }

        transform.name = "buff : " + name;
        transform.parent = _target.transform;
        buff_name = name;
        duration = _duration;
        icon = _icon;
        caster = _caster;
        target = _target;
        dmg = _dmg;
        speed = _speed;
        armor = _armor;
        rateoffire = _rateoffire;
        accuracy = _accuracy;
        evasion = _evasion;
        critrate = cRate;

        continuous = conti;
        HP = hp;

        stun = _stun;
        stun_duration = _stun_duration;

        forceshield = _forceshield;
        shield_duration = _shield_duration;

        AddBuff();
        Invoke("RemoveBuff", duration);
    }

    void AddBuff() {
        target.GetComponent<BuffContainer>().BuffList.Add(gameObject);
        target.GetComponent<OriginalState>().SetState();
        target.GetComponent<BuffContainer>().UpdateBuffViewer(gameObject);
        //InGameManager.instance.OpenPanel_DollInfo(InGameManager.instance.SelectedDoll);
    }

    void RemoveBuff() {
        target.GetComponent<BuffContainer>().BuffList.Remove(gameObject);
        target.GetComponent<OriginalState>().SetState();
        if (InGameUIContainer.instance.Panel_DollInfo.activeSelf == true) {
            InGameUIContainer.instance.Open_Panel_DollInfo(InGameManager.instance.SelectedDoll);
        }
        //InGameManager.instance.OpenPanel_DollInfo(InGameManager.instance.SelectedDoll);
        transform.parent = GameObject.Find("InGameManager").transform;
        gameObject.SetActive(false);
    }
}