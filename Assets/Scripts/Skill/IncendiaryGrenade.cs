using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IncendiaryGrenade : SkillBase
{
    GameObject target;
    public Sprite image;
    float range = 1.5f;
    public float maxHeight = 3;

    public float[] c_dmg = { 3, 3.3f, 3.7f, 4, 4.3f, 4.7f, 5, 5.3f, 5.7f, 6 };
    public float[] c_dmg2 = { 0.5f, 0.6f, 0.6f, 0.7f, 0.7f, 0.8f, 0.8f, 0.9f, 0.9f, 1};
    float dmg;
    float dmg2;


    public override void SkillActive() {
        target = GetComponent<CharacterBase>().Target;
        if (target == null)
            return;

        print("use skill : " + GetSkillName() + "    LV." + skilllevel);
        skill_cool_timer = GetCoolDown();
        skill_duration_timer = GetDuration();

        //투척물 소환
        GetComponent<CharacterBase>().state = CharacterBase.State.s;
        Invoke("ThrowMolotov", 1.5f);
    }

    void ThrowMolotov() {
        if (GetComponent<CharacterBase>().placed == false
            || GetComponent<CharacterBase>().stun == true
            || !gameObject.activeSelf
            || GetComponent<CharacterBase>().state == CharacterBase.State.die)
            return;

        print("throw molotov");
        GameObject molotov = Instantiate(InGameManager.instance.projectile);
        molotov.GetComponent<Projectile>().Caster = gameObject;
        dmg = GetComponent<FinalState>().damage * c_dmg[skilllevel];
        dmg2 = GetComponent<FinalState>().damage * c_dmg2[skilllevel];
        molotov.GetComponent<Projectile>().ExplosionSetting(range, dmg);
        molotov.GetComponent<Projectile>().DeployFieldSetting(range, GetDuration(), dmg2, gameObject);
        molotov.GetComponent<Projectile>().LaunchProjectile(image, GetComponent<DollController>().skillPoint, target.transform, maxHeight, true, true);
    }

    public override void Effect(GameObject _target) {
        _target.GetComponent<CharacterBase>().GetAttacked((int)dmg, -1);
    }

    public override void SkillDescribe() {
        base.SkillDescribe();

        skill_describe = skill_describe.Replace("_c_dmg2", c_dmg2[skilllevel].ToString());
        skill_describe = skill_describe.Replace("_c_dmg", c_dmg[skilllevel].ToString());
        skill_describe = skill_describe.Replace("_duration", GetDuration().ToString());
    }
}
