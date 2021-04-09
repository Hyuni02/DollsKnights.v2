using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGController : DollController
{
    public bool reloading = false;

    public override void SetState() {
        if (fs.hp <= 0)
            state = State.die;

        if (uac.animation.isCompleted) {
            now_animation = null;
            attacking = false;
            if (reloading) {
                reloading = false;
                fs.ammo = GetComponent<OriginalState>().dollstate.ammo;
            }
        }

        if (fs.ammo <= 0) {
            state = State.reload;
        }

        if (attacking || dying || reloading)
            return;

        if (stun) {
            Getstun();
            now_animation = "stun";
            float t = 1 / (fs.rateoffire * 0.02f);
            Timer_attack = t;
            return;
        }

        switch (state) {
            case State.attack:
                if (Timer_attack <= 0 && Target != null && Target.GetComponent<FinalState>().hp > 0) {
                    attacking = true;
                    attack();
                    float t = 1 / (fs.rateoffire * 0.02f);
                    PlayAnimation(state.ToString(), t, 1);
                    Timer_attack = t;
                }
                break;
            case State.die:
                dying = true;
                Invoke("die", 0.7f);
                PlayAnimation(state.ToString(), 1, 1);
                break;
            case State.move:
                move();
                PlayAnimation(state.ToString(), 1, 1);
                break;
            case State.wait:
                wait();
                PlayAnimation(state.ToString());
                break;
            case State.reload:
                if (!reloading) {
                    reload();
                    PlayAnimation(state.ToString(),1,1);
                }
                break;
            case State.victory:
                PlayAnimation(state.ToString(), 1, 1);
                break;
            case State.victoryloop:
                PlayAnimation(state.ToString());
                break;
        }

    }

    GameObject temp_target;

    public override void SearchTarget() {

        if (!gameObject.activeSelf)
            return;

        attackable = false;
        //print("Check Target" + gameObject.name);
        for (int i = 0; i < InGameManager.instance.Spawned_Enemies.Count; i++) {
            if (GetDistance(InGameManager.instance.Spawned_Enemies[i]) <= fs.range
                && InGameManager.instance.Spawned_Enemies[i].activeSelf) {
                attackable |= true;

                if (temp_target == null)
                    temp_target = InGameManager.instance.Spawned_Enemies[0];

                if (InGameManager.instance.Spawned_Enemies[i].GetComponent<FinalState>().hp
                    < temp_target.GetComponent<FinalState>().hp) {
                    temp_target = InGameManager.instance.Spawned_Enemies[i];
                }
                else {
                    if (temp_target.GetComponent<FinalState>().hp <= 0) {
                        temp_target = InGameManager.instance.Spawned_Enemies[i];
                    }
                }
            }
            else {
                SetTarget();
                attackable |= false;
            }
        }

        SetTarget(temp_target);
    }

    void reload() {
        reloading = true;
    }

    public override void attack() {
        base.attack();
        fs.ammo--;
    }
}
