using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StunGrenade : SkillBase
{
    public GameObject target;
    public Sprite image;
    public float range = 1.5f;
    public float maxHeight;

    public override void SkillActive() {
        target = GetComponent<CharacterBase>().Target;
        if (target == null)
            return;

        print("use skill : " + GetSkillName() + "    LV." + skilllevel);
        skill_cool_timer = GetCoolDown();
        skill_duration_timer = GetDuration();

        //투척물 소환
        GetComponent<CharacterBase>().state = CharacterBase.State.s;
        Invoke("ThrowStunGrenade", 1.5f);
    }

    void ThrowStunGrenade() {
        if (GetComponent<CharacterBase>().placed == false 
            || GetComponent<CharacterBase>().stun == true 
            || !gameObject.activeSelf
            || GetComponent<CharacterBase>().state == CharacterBase.State.die)
            return;

        print("throw stun");
        GameObject stunGrenade = Instantiate(InGameManager.instance.projectile);
        stunGrenade.GetComponent<Projectile>().Caster = gameObject;
        stunGrenade.GetComponent<Projectile>().ExplosionSetting(range, 0);
        stunGrenade.GetComponent<Projectile>().LaunchProjectile(image, GetComponent<DollController>().skillPoint, target.transform, maxHeight, true, false);
    }

    public override void SkillDescribe() {
        base.SkillDescribe();
    }

    public override void Effect(GameObject _target) {
        GameObject buff = Instantiate(InGameManager.instance.buff);
        buff.GetComponent<Buff>().Initialized(GetSkillName(), GetDuration(), GetComponent<SkillBase>().skill_icon, gameObject, _target, 0, 0, 0, 0, 0, 0, 0, false, 0, true, GetDuration());
    }
}
