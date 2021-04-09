using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FinalState))]
public class OriginalState : MonoBehaviour {
    public int level;
    //public int rank, hp, damage, accuracy, evasion, rateoffire, armor, speed, armorpen;
    //public float _hp, _damage, _accuracy, _evasion, _rateoffire, _armor, critrate;
    public int maxHP;
    public DollState dollstate;
    public EnemyState enemystate;

    //temp
    public void SetState() {
        FinalState fs = GetComponent<FinalState>();
        if (GetComponent<DollController>() != null) {
            fs.accuracy = dollstate.accuracy + (int)(dollstate._accuracy * (level - 1));
            fs.armor = dollstate.armor + (int)(dollstate._armor * (level - 1));
            fs.armorpen = dollstate.armorpen;
            fs.critrate = dollstate.critrate;
            fs.damage = dollstate.damage + (int)(dollstate._damage * (level - 1));
            fs.evasion = dollstate.evasion + (int)(dollstate._evasion * (level - 1));
            fs.hp = dollstate.hp + (int)(dollstate._hp * (level - 1));
            fs.rateoffire = dollstate.rateoffire + (int)(dollstate._rateoffire * (level - 1));
            fs.speed = dollstate.speed;
            fs.range = dollstate.range;
            fs.block = dollstate.block;
            fs.ammo = dollstate.ammo;
            fs.cost = dollstate.cost + (int)(dollstate._cost * (level - 1));
        }
        else {
            fs.accuracy = enemystate.accuracy + (int)(enemystate._accuracy * (level - 1));
            fs.armor = enemystate.armor + (int)(enemystate._armor * (level - 1));
            fs.armorpen = enemystate.armorpen;
            fs.damage = enemystate.damage + (int)(enemystate._damage * (level - 1));
            fs.evasion = enemystate.evasion + (int)(enemystate._evasion * (level - 1));
            fs.hp = enemystate.hp + (int)(enemystate._hp * (level - 1));
            fs.rateoffire = enemystate.rateoffire + (int)(enemystate._rateoffire * (level - 1));
            fs.speed = enemystate.speed;
            fs.range = enemystate.range;
            fs.cost = enemystate.cost + (int)(enemystate._cost * (level - 1));
            fs.part = enemystate.part + (int)(enemystate._part * (level - 1));
        }
        maxHP = fs.hp;

        BuffContainer bf = GetComponent<BuffContainer>();
        GetComponent<CharacterBase>().stun = false;
        GetComponent<CharacterBase>().forceShield = false;
        if (bf.BuffList.Count > 0) {
            for(int i = 0; i < bf.BuffList.Count; i++) {
                Buff buff = bf.BuffList[i].GetComponent<Buff>();
                fs.accuracy = Mathf.RoundToInt((1 + buff.accuracy * 0.01f) * fs.accuracy);
                fs.armor = Mathf.RoundToInt((1 + buff.armor * 0.01f) * fs.armor);
                fs.damage = Mathf.RoundToInt((1 + buff.dmg * 0.01f) * fs.damage);
                fs.evasion = Mathf.RoundToInt((1 + buff.evasion * 0.01f) * fs.evasion);
                fs.speed = Mathf.RoundToInt((1 + buff.speed * 0.01f) * fs.speed);
                fs.rateoffire = Mathf.RoundToInt((1 + buff.rateoffire * 0.01f) * fs.rateoffire);
                fs.critrate = Mathf.RoundToInt((1 + buff.critrate * 0.01f) * fs.critrate);
                GetComponent<CharacterBase>().stun |= buff.stun;
                GetComponent<CharacterBase>().forceShield |= buff.forceshield;
            }
        }
    }

}