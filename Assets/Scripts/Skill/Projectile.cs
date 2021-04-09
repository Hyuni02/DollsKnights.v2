using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Projectile : MonoBehaviour {
    SpriteRenderer Image_projectile;
    float maxHeight;
    Vector3 startPos;
    Vector3 endPos;
    bool explosion = false;
    bool deployField = false;
    float initDistance;
    public GameObject Caster;

    [Header("Explosion")]
    public float range;
    public float dmg;

    [Header("Field")]
    public float field_range;
    public float duration;
    public float field_dmg;

    public void LaunchProjectile(Sprite sprite, Transform _startPos, Transform _endPos, float _maxHeight, bool _explosion, bool _deployField) {
        Image_projectile = GetComponentInChildren<SpriteRenderer>();
        Image_projectile.sprite = sprite;
        maxHeight = _maxHeight;
        startPos = _startPos.position;
        explosion = _explosion;
        deployField = _deployField;

        endPos = _endPos.position;
        initDistance = Vector3.Distance(startPos, endPos);
        transform.position = startPos;
    }

    public void ExplosionSetting(float _range, float _dmg) {
        range = _range;
        //dmg = _dmg;
    }
    public void DeployFieldSetting(float _range, float _duration, float _dmg, GameObject caster) {
        field_dmg = _dmg;
        field_range = _range;
        duration = _duration;
        Caster = caster;
    }

    GameObject temp;
    void ReachEndPos() {
        if (explosion) {
            //범위 내의 적 찾기
            List<GameObject> List_Targets = new List<GameObject>();
            for(int i = 0; i < InGameManager.instance.Spawned_Enemies.Count; i++) {
                temp = InGameManager.instance.Spawned_Enemies[i];
                if (temp.activeSelf) {
                    if(Vector3.Distance(transform.position, temp.transform.position) <= range) {
                        List_Targets.Add(temp);
                    }
                }
            }

            //찾은 적에게 효과 주기
            for (int i = 0; i < List_Targets.Count; i++) {
                Caster.GetComponent<SkillBase>().Effect(List_Targets[i]);
            }
        }

        if (deployField) {
            GameObject field = Instantiate(InGameManager.instance.field);
            field.transform.position = transform.position;

            field.GetComponent<Field>().duration = duration;
            field.GetComponent<Field>().dmg = field_dmg;
            field.GetComponent<Field>().range = range;
            field.GetComponent<Field>().caster = Caster;
        }
    }

    private void Update() {
        transform.position = Vector3.Slerp(transform.position, endPos, 0.05f);

        if(Vector3.Distance(transform.position, endPos) >= initDistance * 0.5f) {
            Image_projectile.transform.localPosition += new Vector3(0, maxHeight * 3.5f * Time.deltaTime, 0);
        }
        else {
            if(Image_projectile.transform.localPosition.y > 0)
                Image_projectile.transform.localPosition -= new Vector3(0, maxHeight * 3.5f * Time.deltaTime, 0);
        }

        if (Mathf.Abs(Vector3.Distance(transform.position, endPos)) <= 0.1f) {
            ReachEndPos();
            gameObject.SetActive(false);
        }
    }

}
