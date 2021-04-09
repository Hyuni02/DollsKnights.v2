using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HawkAssault : SkillBase
{
    int[] decrease_rof = { 20, 22, 23, 25, 27, 28, 30, 32, 33, 35 };
    int[] increase_dmg = { 80, 91, 102, 113, 124, 136, 147, 158, 169, 180 };


    public override void SkillActive() {
        print("use skill : " + GetSkillName() + "    LV." + skilllevel);
        //버프 부여
        skill_cool_timer = GetCoolDown();
        skill_duration_timer = GetDuration();

        GameObject buff_iws = Instantiate(InGameManager.instance.buff);
        buff_iws.GetComponent<Buff>().Initialized(GetSkillName(), GetDuration(), GetComponent<SkillBase>().skill_icon, gameObject, gameObject, increase_dmg[skilllevel], 0, 0, decrease_rof[skilllevel] * (-1), 0, 0, 0, false, 0);

    }
    public override void SkillDescribe() {
        base.SkillDescribe();

        skill_describe = skill_describe.Replace("_dmg", increase_dmg[skilllevel].ToString());
        skill_describe = skill_describe.Replace("_rof", decrease_rof[skilllevel].ToString());
    }
}
