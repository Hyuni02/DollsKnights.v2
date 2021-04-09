using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DragonBones;

public abstract class CharacterBase : MonoBehaviour {
    public enum State { wait, move, attack, die, victory, victoryloop, s, reload }
    public State state;
    public bool stun = false;
    public bool forceShield = false;
    //[HideInInspector]
    public string Name, now_animation;
    public GameObject Node_StandOn;
    [HideInInspector]
    public bool attacking = false, dying = false;
    public bool placed = false;
    public bool attackable;

    public  float Timer_attack;
    public GameObject Target;

    public List<UnityEngine.Transform> RouteToMove;

    [HideInInspector]
    public UnityArmatureComponent uac;
    [HideInInspector]
    public FinalState fs;
    OriginalState os;
    SkillBase sb;

    [HideInInspector]
    public Slider Slider_HPBar, Slider_SkillBar;
    Image skill_fill;
    [HideInInspector]
    public GameObject BuffIconViewer;

    public virtual void Start() {
        uac = GetComponentInChildren<UnityArmatureComponent>();
        fs = GetComponent<FinalState>();
        sb = GetComponent<SkillBase>();

        os = GetComponent<OriginalState>();
        os.SetState();
        Slider_HPBar = transform.Find("Canvas_InGameDoll").transform.Find("Slider_HPBar").GetComponent<Slider>();
        Slider_HPBar.maxValue = os.maxHP;
        Slider_HPBar.value = os.maxHP;
        Slider_SkillBar = transform.Find("Canvas_InGameDoll").transform.Find("Slider_SkillBar").GetComponent<Slider>();
        skill_fill = transform.Find("Canvas_InGameDoll").transform.Find("Slider_SkillBar").transform.Find("Fill Area").transform.Find("Fill").GetComponent<Image>();
        BuffIconViewer = transform.Find("Canvas_InGameDoll").transform.Find("BuffIconViewer").gameObject;

        //Check_Node_Stand();
        Timer_attack = 1;
        InvokeRepeating("SearchTarget", 0, 0.1f);
    }

    public virtual void Update() {
        SetState();
        UpdateState();
        UpdateBar();

        if (Timer_attack > 0)
            Timer_attack -= Time.deltaTime;
    }

    public void SetRoute(List<UnityEngine.Transform> route) {
        RouteToMove = new List<UnityEngine.Transform>();
        for(int i = 0; i <route.Count; i++) {
            RouteToMove.Add(route[i]);
        }
        Node_StandOn = RouteToMove[RouteToMove.Count - 1].gameObject;
    }

    public virtual void SetState() {
        if (fs.hp <= 0)
            state = State.die;

        if (uac.animation.isCompleted) {
            now_animation = null;
            attacking = false;
        }

        if (attacking || dying)
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
            case State.victory:
                PlayAnimation(state.ToString(), 1, 1);
                break;
            case State.victoryloop:
                PlayAnimation(state.ToString());
                break;
        }

    }

    public void PlayAnimation(string anim, float timescale = 1, int playtime = 0) {
       if (state.ToString().Equals(now_animation))
            return;

        uac.animation.timeScale = Mathf.Max(timescale, 1);
        now_animation = anim;
        uac.animation.Play(anim, playtime);
    }

    public virtual void attack() {
        //print(gameObject.name + " attacked " + Target.name);
        SetFaceDir(Target.transform.position.x);
        Target.GetComponent<CharacterBase>().GetAttacked(fs.damage, fs.accuracy, fs.critrate, fs.armorpen);
    }
    public virtual void move() {
        if (RouteToMove.Count > 0) {
            float dif_x = RouteToMove[0].transform.position.x - transform.position.x;
            float dif_z = RouteToMove[0].transform.position.z - transform.position.z;
            SetFaceDir(RouteToMove[0].transform.position.x);

            Vector3 dir = new Vector3(dif_x, 0, dif_z);
            if (dir.magnitude <= 0.02f) {
                transform.position = new Vector3(RouteToMove[0].transform.position.x, transform.position.y, RouteToMove[0].transform.position.z);
                RouteToMove.RemoveAt(0);
                //Check_Node_Stand();
            }
            else {
                transform.Translate(dir.normalized * fs.speed * 0.06f * Time.deltaTime, Space.World);
            }
        }
    }
    public virtual void wait() {
        SetFaceDir(InGameManager.instance.Map.GetComponent<LevelInfo>().Dir_Standard.transform.position.x);
    }
    public virtual void die() {
        gameObject.SetActive(false);
    }
    public abstract void Getstun();

    //void Check_Node_Stand() {
    //    RaycastHit hit;
    //    if (Physics.Raycast(transform.position + new Vector3(0,0.15f,0), Vector3.down * 0.2f, out hit)) {
    //        if (hit.collider.GetComponent<NodeInfo>()) {
    //            Node_StandOn = hit.collider.gameObject;
    //        }
    //    }
    //}

    public void SetFaceDir(float Target_PosX) {
        if (Target_PosX > transform.position.x)
            uac.armature.flipX = false;
        else if (Target_PosX == transform.position.x)
            return;
        else
            uac.armature.flipX = true;
    }

    public virtual void SearchTarget() {
        if (!gameObject.activeSelf)
            return;

        attackable = false;
        //print("Check Target" + gameObject.name);
        if(GetComponent<DollController>() != null) {
            for(int i = 0; i < InGameManager.instance.Spawned_Enemies.Count; i++) {
                if (GetDistance(InGameManager.instance.Spawned_Enemies[i]) <= fs.range
                    && InGameManager.instance.Spawned_Enemies[i].activeSelf) {
                    attackable = true;
                    SetTarget(InGameManager.instance.Spawned_Enemies[i]);
                    return;
                }
                else {
                    SetTarget();
                    attackable = false;
                }
            }
        }
        else {
            for (int i = 0; i < InGameManager.instance.Spawned_Dolls.Count; i++) {
                if (GetDistance(InGameManager.instance.Spawned_Dolls[i]) <= fs.range
                     && InGameManager.instance.Spawned_Dolls[i].activeSelf) {
                    attackable = true;
                    SetTarget(InGameManager.instance.Spawned_Dolls[i]);
                    return;
                }
                else {
                    SetTarget();
                    attackable = false;
                }
            }
        }
    }

    public abstract void UpdateState();

    public float GetDistance(GameObject target) {
        if (target == null)
            return 0;

        float distance = Vector3.Distance(target.transform.position, gameObject.transform.position);
        return distance;
    }
    public void SetTarget(GameObject target = null) {
        Target = target;
    }
    public virtual void GetAttacked(int dmg, int acc, float critrate = 0, int armorpen = 0) {
        DamageIndicator indicator = InGameManager.instance.GetIndicator();
        indicator.transform.position = transform.position;
        if (forceShield) {
            indicator.SetIndicator(DamageIndicator.Type.shield, 0, transform.position);
            return;
        }

        //회피-명중 계산
        if(acc == -1) {
            fs.hp -= dmg;
            indicator.SetIndicator(DamageIndicator.Type.hit, dmg, transform.position);
            return;
        }
        //-명중 시
        if (acc > Random.Range(0, acc + fs.evasion)) {
            //장갑 판정
            if (fs.armor > 0) {
                if (armorpen <= fs.armor) {
                    fs.hp -= 1;
                    indicator.SetIndicator(DamageIndicator.Type.block, dmg, transform.position);
                    return;
                }
                else {
                    int blockeddmg = dmg - fs.armor;
                    fs.hp -= blockeddmg;
                    indicator.SetIndicator(DamageIndicator.Type.block, blockeddmg, transform.position);
                    return;
                }
            }

            //치명타 판정
            if(critrate * 100 > Random.Range(0, 100)) {
                int critdmg = (int)(dmg * 1.5f);
                fs.hp -= critdmg;
                indicator.SetIndicator(DamageIndicator.Type.crit, critdmg, transform.position);
                return;
            }

            fs.hp -= dmg;
            indicator.SetIndicator(DamageIndicator.Type.hit, dmg, transform.position);
        }
        //-회피 시
        else {
            indicator.SetIndicator(DamageIndicator.Type.miss, dmg, transform.position);
        }

    }

    public void UpdateBar() {
        Slider_HPBar.value = fs.hp;

        if(GetComponent<DollController>() != null) {
            //사용중
            //-빨강색
            //-최대값 : 지속시간
            if (sb.skill_duration_timer > 0) {
                skill_fill.color = Color.red;
                Slider_SkillBar.maxValue = sb.GetDuration();
                Slider_SkillBar.value = sb.skill_duration_timer;
                return;
            }
            //충전중
            //-하늘색
            //-최대값 : 쿨타임
            else if (sb.skill_cool_timer > 0 && sb.skill_duration_timer <= 0) {
                skill_fill.color = Color.cyan;
                Slider_SkillBar.maxValue = sb.GetCoolDown();
                Slider_SkillBar.value = sb.GetCoolDown() - sb.skill_cool_timer;
                return;
            }
            //충전 완료
            //-주황색
            //-최대값 : 쿨타임
            else {
                skill_fill.color = Color.blue;
                Slider_SkillBar.value = Slider_SkillBar.maxValue;
                return;
            }
        }
        else {
            Slider_SkillBar.value = 0;
        }
    }

    private void OnDrawGizmosSelected() {
        if (!Application.isPlaying)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, fs.range);
    }
}