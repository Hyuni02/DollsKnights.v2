using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceShield : SkillBase
{

    public override void SkillActive() {
        print("use skill : " + GetSkillName() + "    LV." + skilllevel);
        skill_cool_timer = GetCoolDown();
        skill_duration_timer = GetDuration();

        GameObject buff_shield = Instantiate(InGameManager.instance.buff);
        buff_shield.GetComponent<Buff>().Initialized(GetSkillName(), GetDuration(), GetComponent<SkillBase>().skill_icon, gameObject, gameObject, 0, 0, 0, 0, 0, 0, 0, false, 0, false, 0, true, GetDuration());
    }

    public override void SkillDescribe() {
        base.SkillDescribe();
    }
}
