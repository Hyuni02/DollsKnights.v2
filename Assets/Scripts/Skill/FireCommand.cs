using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCommand : SkillBase
{
    GameObject Target;
    public int[] increase_dmg = { 12, 13, 14, 15, 16, 17, 18, 20, 21, 22 };

    public override void SkillActive() {
        print("use skill : " + GetSkillName() + "    LV." + skilllevel);
        //배치된 모든 인형들에게 버프 부여
        skill_cool_timer = GetCoolDown();
        skill_duration_timer = GetDuration();
        for (int i = 0; i < InGameManager.instance.Spawned_Dolls.Count; i++) {
            //버프 소환
            GameObject buff_M1873 = Instantiate(InGameManager.instance.buff);
            buff_M1873.GetComponent<Buff>().Initialized(GetSkillName(), GetDuration(), GetComponent<SkillBase>().skill_icon, gameObject, InGameManager.instance.Spawned_Dolls[i], increase_dmg[skilllevel], 0, 0, 0, 0, 0, 0, false, 0);
        }
    }
    public override void SkillDescribe() {
        base.SkillDescribe();

        skill_describe = skill_describe_form.Replace("_dmg", increase_dmg[skilllevel].ToString());
    }
}
