using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFocus : SkillBase
{
    GameObject Target;
    public int[] increase_dmg = { 40, 43, 47, 50, 53, 57, 60, 63, 67, 70 };

    public override void SkillActive() {
        print("use skill : " + GetSkillName() + "    LV." + skilllevel);
        //배치된 모든 인형들에게 버프 부여
        skill_cool_timer = GetCoolDown();
        skill_duration_timer = GetDuration();
        //버프 소환
        GameObject buff_g36 = Instantiate(InGameManager.instance.buff);
        buff_g36.GetComponent<Buff>().Initialized(GetSkillName(), GetDuration(), GetComponent<SkillBase>().skill_icon, gameObject, gameObject, increase_dmg[skilllevel], 0, 0, 0, 0, 0, 0, false, 0);
    }
    public override void SkillDescribe() {
        base.SkillDescribe();

        skill_describe = skill_describe_form.Replace("_dmg", increase_dmg[skilllevel].ToString());
    }
}
