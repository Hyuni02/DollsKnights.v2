using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIContainer_DollList : MonoBehaviour
{
    public static UIContainer_DollList instance;

    [Header("Doll Info")]
    public GameObject Panel_DollInfo;
    [Header("Image")]
    public Image Image_Sprite;
    public Image Image_Level;
    public Image Image_Skill_Icon;
    [Header("Text")]
    public Text Text_Name;
    public Text Text_Level;
    public Text Text_belong;
    public Text Text_class;
    public Text Text_hp;
    public Text Text_dmg;
    public Text Text_acc;
    public Text Text_eva;
    public Text Text_rof;
    public Text Text_armor;
    public Text Text_speed;
    public Text Text_armorpen;
    public Text Text_crit;
    public Text Text_Skill_Duration;
    public Text Text_Skill_Name;
    public Text Text_Skill_CoolTime;
    public Text Text_Skill_Explaination;

    public void Awake() {
        instance = this;
    }
}
