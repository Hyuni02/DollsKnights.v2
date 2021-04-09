using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BuffContainer))]
public class DollController : CharacterBase
{
    public Sprite Sprite_Doll;
    public Sprite Sprite_Doll_face;

    public List<GameObject> Blocked_Enemies;
    public uint stunFrame = 0;
    public Transform skillPoint;
    private void Awake() {
        if(Sprite_Doll == null || Sprite_Doll_face == null) {
            Debug.LogError(Name + "'s Sprite is null");
        }
    }

    public override void UpdateState() {
        if (state == State.die)
            return;

        switch (GetComponent<SkillBase>().type) {
            case SkillBase.Type.boost:
                if(RouteToMove.Count == 0) {
                    if (attackable) 
                        state = State.attack;
                    else
                        state = State.wait;
                }
                else {
                    state = State.move;
                }
                break;

            case SkillBase.Type.action:
                if (stun)
                    return;
                if (state == State.s) {
                    if (uac.animation.isCompleted) {
                        state = State.wait;
                    }
                    else {
                        PlayAnimation("s", 1, 1);
                    }
                }
                else {
                    if (RouteToMove.Count == 0) {
                        if (attackable)
                            state = State.attack;
                        else
                            state = State.wait;
                    }
                    else {
                        state = State.move;
                    }
                }
                break;
        }

        Blocking();
    }

    void Blocking() {
        if (fs.block > Blocked_Enemies.Count) {
            for (int i = 0; i < InGameManager.instance.Spawned_Enemies.Count; i++) {
                if (GetDistance(InGameManager.instance.Spawned_Enemies[i]) < 0.7f
                    && InGameManager.instance.Spawned_Enemies[i].activeSelf) {
                    //중복 확인
                    if (InGameManager.instance.Spawned_Enemies[i].GetComponent<EnemyController>().Blocker == null) {
                        InGameManager.instance.Spawned_Enemies[i].GetComponent<EnemyController>().Blocker = gameObject;
                        Blocked_Enemies.Add(InGameManager.instance.Spawned_Enemies[i]);
                    }
                }
            }
        }

        if (Blocked_Enemies.Count > 0) {
            for (int i = Blocked_Enemies.Count - 1; i >= 0; i--) {
                if (!Blocked_Enemies[i].activeSelf || GetDistance(Blocked_Enemies[i]) > 0.7f)
                    Blocked_Enemies.Remove(Blocked_Enemies[i]);
            }
        }
        if (Blocked_Enemies.Count > 0) 
            SetTarget(Blocked_Enemies[0]);
    }

    public override void GetAttacked(int dmg, int acc, float critrate = 0, int armorpen = 0) {
        base.GetAttacked(dmg, acc, critrate, armorpen);

        Slider_HPBar.value = fs.hp;
    }

    public override void die() {
        Retreat();
    }

    public void Retreat() {
        //모든 버프 제거
        gameObject.SetActive(false);
        placed = false;
    }

    public override void Getstun() {
        uac.animation.GotoAndStopByFrame("die", stunFrame);
    }
}
