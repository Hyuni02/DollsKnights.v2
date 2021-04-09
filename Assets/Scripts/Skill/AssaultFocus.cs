using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultFocus : SkillBase
{
    public int[] increase_rof = { 25, 27, 29, 32, 34, 36, 38, 41 };


    public override void SkillActive() {
        print("use skill : " + GetSkillName() + "    LV." + skilllevel);
        skill_cool_timer = GetCoolDown();
        skill_duration_timer = GetDuration();

        GameObject buff_rof = Instantiate(InGameManager.instance.buff);
        buff_rof.GetComponent<Buff>().Initialized(GetSkillName(), GetDuration(), GetComponent<SkillBase>().skill_icon, gameObject, gameObject, 0, 0, 0, increase_rof[skilllevel], 0, 0, 0, false, 0);
    }

    public override void SkillDescribe() {
        base.SkillDescribe();

        skill_describe = skill_describe.Replace("_rof", increase_rof[skilllevel].ToString());
    }
}
