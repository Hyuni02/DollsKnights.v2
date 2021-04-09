using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerfectThrillPark : SkillBase
{
    public float[] duration = { };
    public float[] duration2 = { };
    public float[] c_dmg = { };
    public float[] c_dmg2 = { };
    GameObject target;

    public override void SkillActive() {
        SearchTarget();

        if (target == null)
            return;

        print("use skill : " + GetSkillName() + "    LV." + skilllevel);
        skill_cool_timer = GetCoolDown();
        skill_duration_timer = GetDuration();


    }

    void SearchTarget() {
        //타겟 검색
    }

    public override void SkillDescribe() {
        base.SkillDescribe();

        skill_describe = skill_describe.Replace("_duration2", duration2[skilllevel].ToString());
        skill_describe = skill_describe.Replace("_duration", duration[skilllevel].ToString());
        skill_describe = skill_describe.Replace("_c_dmg2", c_dmg2[skilllevel].ToString());
        skill_describe = skill_describe.Replace("_c_dmg", c_dmg[skilllevel].ToString());
    }
}
