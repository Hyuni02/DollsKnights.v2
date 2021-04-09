using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBuff : SkillBase
{
    GameObject Target;
    GameObject explosive;
    public int[] increase_dmg;
    public int[] increase_speed;
    public int[] increase_armor;
    public int[] increase_rof;
    public int[] increase_acc;
    public int[] increase_eva;
    public int[] increase_crit;

    public override void SkillActive() {
        print("use skill : " + GetSkillName() + "    LV." + skilllevel);
        //배치된 모든 인형들에게 버프 부여
        skill_cool_timer = GetCoolDown();
        skill_duration_timer = GetDuration();
        for (int i = 0; i < InGameManager.instance.Spawned_Dolls.Count; i++) {
            //버프 소환
            GameObject buff = Instantiate(InGameManager.instance.buff);
            buff.GetComponent<Buff>().Initialized(GetSkillName(), GetDuration(), GetComponent<SkillBase>().skill_icon, gameObject, InGameManager.instance.Spawned_Dolls[i], increase_dmg[skilllevel], increase_speed[skilllevel], increase_armor[skilllevel], increase_rof[skilllevel], increase_acc[skilllevel], increase_eva[skilllevel],
                increase_crit[skilllevel], false, 0);
        }
    }
    public override void SkillDescribe() {
        base.SkillDescribe();

        skill_describe = skill_describe_form;

        if (increase_dmg.Length > 0)
            skill_describe = skill_describe.Replace("_dmg", increase_dmg[skilllevel].ToString());

        if (increase_speed.Length > 0)
            skill_describe = skill_describe.Replace("_speed", increase_speed[skilllevel].ToString());

        if (increase_armor.Length > 0)
            skill_describe = skill_describe.Replace("_armor", increase_armor[skilllevel].ToString());

        if (increase_rof.Length > 0)
            skill_describe = skill_describe.Replace("_rof", increase_rof[skilllevel].ToString());

        if (increase_acc.Length > 0)
            skill_describe = skill_describe.Replace("_acc", increase_acc[skilllevel].ToString());

        if (increase_eva.Length > 0)
            skill_describe = skill_describe.Replace("_eva", increase_eva[skilllevel].ToString());

        if(increase_crit.Length > 0)
            skill_describe = skill_describe.Replace("_crit", increase_crit[skilllevel].ToString());
    }
}
