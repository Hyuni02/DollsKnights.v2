using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BuffContainer))]
public class DMRController : DollController {

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

                if(temp_target == null)
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
}
