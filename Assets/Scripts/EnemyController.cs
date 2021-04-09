using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BuffContainer))]
public class EnemyController : CharacterBase {
    public enum Type { melee, range }
    public Type type;
    public bool blocked = false;
    public GameObject Blocker;

    bool arrive = false;

    //public override void Start() {

    //}

    //public override void Update() {
        
    //}

    public override void UpdateState() {
        if (state == State.die)
            return;

        switch (type) {
            case Type.melee:
                if (blocked) 
                    state = State.attack;
                else 
                    state = State.move;

                break;

            case Type.range:
                if (blocked) {
                    if (now_animation == "move") {
                        state = State.wait;
                        PlayAnimation("wait");
                    }
                    state = State.attack;
                }
                else {
                    if (Timer_attack <= 0 && !attacking && Target != null) {
                        state = State.attack;
                    }
                    else {
                        state = State.move;
                    }
                }
                break;
        }

        CheckBlocked();
        CheckArrive();
    }

    public override void die() {
        InGameManager.instance.EliminatedEnemyCount++;
        InGameUIContainer.instance.UpdateEnemyCount();
        base.die();
        InGameManager.instance.Eliminated(fs.cost, fs.part);
    }

    //목표 지점 도달 확인
    void CheckArrive() {
        if (!arrive && RouteToMove.Count == 0) {
            InGameManager.instance.RemainLife--;
            InGameManager.instance.EliminatedEnemyCount++;
            InGameUIContainer.instance.UpdateRemainLife();
            InGameUIContainer.instance.UpdateEnemyCount();

            arrive = true;
            InGameManager.instance.LifeLossAlert();

            if (InGameManager.instance.RemainLife <= 0)
                InGameManager.instance.Defeat();

            gameObject.SetActive(false);
        }
    }
    //저지 당함 확인
    void CheckBlocked() {
        if (Blocker == null)
            blocked = false;
        else {
            if (Blocker.activeSelf == true) {
                SetTarget(Blocker);
                blocked = true;
            }
            else
                Blocker = null;

            if (GetDistance(Blocker) > 0.7f && !attacking)
                Blocker = null;
        }
    }
    //피격
    public override void GetAttacked(int dmg, int acc, float critrate = 0, int armorpen = 0) {
        base.GetAttacked(dmg, acc, critrate, armorpen);

        Slider_HPBar.value = fs.hp;
    }

    public override void Getstun() {
        uac.animation.Stop();
    }
}
