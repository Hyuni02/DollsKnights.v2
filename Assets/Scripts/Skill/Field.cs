using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public float range;
    public float duration;
    public float tick = 0.33f;
    public float dmg;
    public GameObject caster;

    List<GameObject> targets = new List<GameObject>();
    bool findEnemy = false;

    void Start()
    {
        if (caster.GetComponent<DollController>() != null)
            findEnemy = true;

        InvokeRepeating("Field_Active", tick, tick);
        Invoke("Disable", duration);
    }

    void Field_Active()
    {
        targets.Clear();
        if (findEnemy) {
            for(int i = 0; i < InGameManager.instance.Spawned_Enemies.Count; i++) {
                if(Vector3.Distance(transform.position, InGameManager.instance.Spawned_Enemies[i].transform.position) <= range) {
                    targets.Add(InGameManager.instance.Spawned_Enemies[i]);
                }
            }
        }
        else {
            for (int i = 0; i < InGameManager.instance.Spawned_Dolls.Count; i++) {
                if (Vector3.Distance(transform.position, InGameManager.instance.Spawned_Dolls[i].transform.position) <= range) {
                    targets.Add(InGameManager.instance.Spawned_Dolls[i]);
                }
            }
        }

        for(int i = 0; i < targets.Count; i++) {
            targets[i].GetComponent<CharacterBase>().GetAttacked((int)dmg, -1);
        }
    }
    void Disable() {
        CancelInvoke("Field_Active");
        gameObject.SetActive(false);
    }
}
