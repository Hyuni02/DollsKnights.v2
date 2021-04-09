using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiPersonnelGrenade : SkillBase
{
    public GameObject target;
    public Sprite image;
    public float range = 1.5f;
    public float maxHeight;
    public float[] cofficient_dmg = { 5, 5.8f, 6.6f, 7.3f, 8.1f, 9.6f, 10.4f, 11.2f, 12f };
    float dmg;

    public override void SkillActive() {
        target = GetComponent<CharacterBase>().Target;
        if (target == null)
            return;

        print("use skill : " + GetSkillName() + "    LV." + skilllevel);
        skill_cool_timer = GetCoolDown();
        skill_duration_timer = GetDuration();

        //투척물 소환
        GetComponent<CharacterBase>().state = CharacterBase.State.s;
        Invoke("LaunchGrenade", 1.1f);
    }

    void LaunchGrenade() {
        if (GetComponent<CharacterBase>().placed == false
            || GetComponent<CharacterBase>().stun == true
            || !gameObject.activeSelf
            || GetComponent<CharacterBase>().state == CharacterBase.State.die)
            return;

        print("launch grenade");
        GameObject grenade = Instantiate(InGameManager.instance.projectile);
        grenade.GetComponent<Projectile>().Caster = gameObject;
        dmg = GetComponent<FinalState>().damage * cofficient_dmg[skilllevel];
        grenade.GetComponent<Projectile>().ExplosionSetting(range, dmg);
        grenade.GetComponent<Projectile>().LaunchProjectile(image, GetComponent<DollController>().skillPoint, target.transform, maxHeight, true, false);
    }

    public override void Effect(GameObject _target) {
        _target.GetComponent<CharacterBase>().GetAttacked((int)dmg, -1);
    }

    public override void SkillDescribe() {
        base.SkillDescribe();

        skill_describe = skill_describe.Replace("_c_dmg", cofficient_dmg[skilllevel].ToString());
    }
}
