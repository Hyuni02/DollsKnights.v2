using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using Newtonsoft.Json;

[System.Serializable]
public class DollData {
    public string name;
    public int level = 0;
    public int echlon;  //0 = 무소속
    public int pos_x;   //(1,1)~(3,3)만 제대에 존재-나머지는 버프 용
    public int pos_y;   //(1,1)~(3,3)만 제대에 존재-나머지는 버프 용

    public DollData(string name, int level = 0, int echlon = 0, int pos_x = 0, int pos_y = 0) {
        this.name = name;
        this.level = level;
        this.echlon = echlon;
        this.pos_x = pos_x;
        this.pos_y = pos_y;
    }
}

[System.Serializable]
public struct DollState {
    public string name, belong, _class;
    public int rank, hp, damage, accuracy, evasion, rateoffire, armor, speed, armorpen, block, ammo, cost;
    public float _hp, _damage, _accuracy, _evasion, _rateoffire, _armor, critrate, range , _cost;
}

[System.Serializable]
public struct EnemyState {
    public string name , type;
    public int hp, damage, accuracy, evasion, rateoffire, armor, speed, armorpen, cost, part;
    public float _hp, _damage, _accuracy, _evasion, _rateoffire, _armor, range, _cost, _part;
}

[System.Serializable]
public class LevelData {
    public int index;
    public int count_try;
    public int count_clear;
    public bool open;

    public LevelData(int _index, bool _open = false, int _try = 0, int _clear = 0) {
        this.index = _index;
        this.count_clear = _clear;
        this.count_try = _try;
        this.open = _open;
    }
}

public class GetData : MonoBehaviour
{
    public static GetData instance;

    string FileName_DollStateData = "DollStateData";
    string FileName_DollData = "DollData";
    string FileName_EnemyStateData = "EnemyStateData";
    string FileName_LevelData = "LevelData";
    string FileName_PlayerInfo = "PlayerInfo";
    string FileName_PickUpData = "PickUpData";
    //string MapListData = "MapListData";

    [Header("Player Info")]
    public int Token = 0;
    
    [Header("인형 능력치")]
    [SerializeField]
    [Tooltip("인형 능력치 from DollStateData.csv")]
    public List<DollState> List_DollState = new List<DollState>();

    [Header("인형 정보")]
    [SerializeField]
    [Tooltip("인형 정보(소속 제대, 포지션)")]
    public List<DollData> List_DollData = new List<DollData>();

    [Header("적 능력치")]
    [SerializeField]
    [Tooltip("적 정보 from EnemyStateData.csv")]
    public List<EnemyState> List_EnemyState = new List<EnemyState>();

    [Header("레벨 클리어 정보")]
    [SerializeField]
    [Tooltip("레벨 클리어 정보(시도 횟수, 클리어 횟수, 진입가능 여부)")]
    public List<LevelData> List_LevelData = new List<LevelData>();

    [Header("픽업 정보")]
    [SerializeField]
    [Tooltip("픽업 정보(이벤트 이름, 공개 여부, 픽업 횟수)")]
    public List<PickUp> List_PickUpData = new List<PickUp>();

    [SerializeField]
    public List<GameObject> List_DollButton = new List<GameObject>();

    GameObject doll_prefab;
    DollData doll_data;
    
    private void Awake() {
        instance = this;
    }

    public void DataLoad()
    {
        LoadPlayerInfoFile();//플레이어 정보 가져오기

        Load_DollStateData();//DollStateData.csv 데이터 불러오기(Resources)
        Check_DollData();//DollContainer의 인형 개수 확인

        Load_EnemyStateData();//EnemyStateData.csv 데이터 불러오기(Resources)
        Check_EnemyData();//EnemyContainer의 적 개수 확인

        Instantiate_Doll_ButtonList();

        //맵데이터 불러오기
        Check_LevelData();
        
        SceneController.instance.ChangeScene(1);
    }
    void Load_DollStateData() {
        StreamReader sr = new StreamReader(Application.dataPath + "/Resources/Data/" + FileName_DollStateData + ".csv");
        bool endoffile = false;
        while (!endoffile) {
            string data_String = sr.ReadLine();
            if(data_String == null) {
                endoffile = true;
                break;
            }
            var data_values = data_String.Split(',');

            int i = 0;
            float f = 0;
            DollState dollState;
            {
                dollState.name = data_values[0];
                int.TryParse(data_values[1], out i); dollState.rank = i;
                dollState.belong = data_values[2];
                dollState._class = data_values[3];
                int.TryParse(data_values[4], out i); dollState.hp = i;
                float.TryParse(data_values[5], out f); dollState._hp = f;
                int.TryParse(data_values[6], out i); dollState.damage = i;
                float.TryParse(data_values[7], out f); dollState._damage = f;
                int.TryParse(data_values[8], out i); dollState.accuracy = i;
                float.TryParse(data_values[9], out f); dollState._accuracy = f;
                int.TryParse(data_values[10], out i); dollState.evasion = i;
                float.TryParse(data_values[11], out f); dollState._evasion = f;
                int.TryParse(data_values[12], out i); dollState.rateoffire = i;
                float.TryParse(data_values[13], out f); dollState._rateoffire = f;
                int.TryParse(data_values[14], out i); dollState.armor = i;
                float.TryParse(data_values[15], out f); dollState._armor = f;
                int.TryParse(data_values[16], out i); dollState.speed = i;
                int.TryParse(data_values[17], out i); dollState.armorpen = i;
                float.TryParse(data_values[18], out f); dollState.critrate = f;
                int.TryParse(data_values[19], out i); dollState.block = i;
                float.TryParse(data_values[20], out f); dollState.range = f;
                int.TryParse(data_values[21], out i); dollState.ammo = i;
                int.TryParse(data_values[22], out i); dollState.cost = i;
                float.TryParse(data_values[23], out f); dollState._cost = f;
            }

            List_DollState.Add(dollState);
        }
        List_DollState.RemoveAt(0);//첫 행 제거
    }
    void Check_DollData() {
        //-개수가 일치 하지 않으면 오류발생
        if (GetComponent<DollContainer>().Dolls.Count != List_DollState.Count) {
            Debug.LogError("Doll Data Count is not match with Doll Prefab Count");
            Debug.LogError("DollData : " + List_DollState.Count + "\n Doll Prefab : " + List_DollData.Count);
            return;
        }
        //개수가 일치하면 DollData.json 존재 확인
        FileInfo DollDataFile = new FileInfo(Application.streamingAssetsPath + "/" + FileName_DollData + ".json");
        //-존재하지 않으면 DollStateData를 기준으로 새로 생성
        if (!DollDataFile.Exists) {
            CreateDollDataFile();
        }
        //-존재하면 DollData.json 불러오기
        LoadDollDataFile();
    }

    void Load_EnemyStateData() {
        StreamReader sr = new StreamReader(Application.dataPath + "/Resources/Data/" + FileName_EnemyStateData + ".csv");
        bool endoffile = false;
        while (!endoffile) {
            string data_String = sr.ReadLine();
            if (data_String == null) {
                endoffile = true;
                break;
            }
            var data_values = data_String.Split(',');

            int i = 0;
            float f = 0;
            EnemyState enemyState;
            {
                enemyState.name = data_values[0];
                enemyState.type = data_values[1];
                int.TryParse(data_values[2], out i); enemyState.hp = i;
                float.TryParse(data_values[3], out f); enemyState._hp = f;
                int.TryParse(data_values[4], out i); enemyState.damage = i;
                float.TryParse(data_values[5], out f); enemyState._damage = f;
                int.TryParse(data_values[6], out i); enemyState.accuracy = i;
                float.TryParse(data_values[7], out f); enemyState._accuracy = f;
                int.TryParse(data_values[8], out i); enemyState.evasion = i;
                float.TryParse(data_values[9], out f); enemyState._evasion = f;
                int.TryParse(data_values[10], out i); enemyState.rateoffire = i;
                float.TryParse(data_values[11], out f); enemyState._rateoffire = f;
                int.TryParse(data_values[12], out i); enemyState.armor = i;
                float.TryParse(data_values[13], out f); enemyState._armor = f;
                int.TryParse(data_values[14], out i); enemyState.speed = i;
                int.TryParse(data_values[15], out i); enemyState.armorpen = i;
                float.TryParse(data_values[16], out f); enemyState.range = f;
                int.TryParse(data_values[17], out i); enemyState.cost = i;
                float.TryParse(data_values[18], out f); enemyState._cost = f;
                int.TryParse(data_values[19], out i); enemyState.part = i;
                float.TryParse(data_values[20], out f); enemyState._part = f;
            }
            List_EnemyState.Add(enemyState);
        }
        List_EnemyState.RemoveAt(0);//첫 행 제거
    }
    void Check_EnemyData() {
        //-개수가 일치 하지 않으면 오류발생
        if (GetComponent<EnemyContainer>().Enemies.Count != List_EnemyState.Count) {
            Debug.LogError("Enemy Data Count is not match with Enemy Prefab Count");
            Debug.LogError("EnemyData : " + List_EnemyState.Count + "\n Enemy Prefab : " + List_EnemyState.Count);
            return;
        }
    }

    void CreateDollDataFile() {
        for (int i = 0; i < List_DollState.Count; i++) {
            //print("Add " + List_DollState[i].name);
            List_DollData.Add(new DollData(List_DollState[i].name));
        }
        //--기본 인형 설정
        //Todo
        foreach (DollData doll in List_DollData) {
            doll.level = 1;
        }
   
        //새로운 파일 생성
        SaveDollDataFile();
        print("Create New DollData.json File");
    }
    public void LoadDollDataFile() {
        string Ddate = File.ReadAllText(Application.streamingAssetsPath + "/" + FileName_DollData + ".json");
        List_DollData = JsonConvert.DeserializeObject<List<DollData>>(Ddate);
        //print("Load DollState File");

        //캐릭터 데이터가 추가되었을 시
        if(List_DollData.Count != List_DollState.Count) {
            int length = List_DollData.Count;
            for (int i = length; i < List_DollState.Count; i++) {
                List_DollData.Add(new DollData(List_DollState[i].name));
            }
            print("Save File's Format has Updated");
            SaveDollDataFile();
        }
    }
    public void SaveDollDataFile() {
        string Ddata = JsonConvert.SerializeObject(List_DollData);
        File.WriteAllText(Application.streamingAssetsPath + "/" + FileName_DollData + ".json", Ddata);
    }

    public void Check_PickUpData() {
        //PickUpData.json 존재 확인
        FileInfo PickUpDataFile = new FileInfo(Application.streamingAssetsPath + "/" + FileName_PickUpData + ".json");
        //-존재하지 않으면 List_PickUpData를 기준으로 새로 생성
        if (!PickUpDataFile.Exists) {
            //print("No Level Data File");
            CreatePickUpDataFile();
        }
        //-존재하면 LevelData.json 불러오기
        LoadPickUpDataFile();
    }
    void CreatePickUpDataFile() {
        for (int i = 0; i < RandomPickUp.instance.PickUps.Count; i++) {
            //print("Add " + List_DollState[i].name);
            List_PickUpData.Add(new PickUp(RandomPickUp.instance.PickUps[i].Event_Name, 0));
        }

        //새로운 파일 생성
        SavePickUpDataFile();
        print("Create New PickUpData.json File");
    }
    void LoadPickUpDataFile() {
        string Pdate = File.ReadAllText(Application.streamingAssetsPath + "/" + FileName_PickUpData + ".json");
        List_PickUpData = JsonConvert.DeserializeObject<List<PickUp>>(Pdate);
        //print("Load PickUp Data File");

        //픽업 데이터가 추가되었을 시
        if (List_PickUpData.Count != RandomPickUp.instance.PickUps.Count) {
            int length = List_PickUpData.Count;
            for (int i = length; i < RandomPickUp.instance.PickUps.Count; i++) {
                List_PickUpData.Add(new PickUp(RandomPickUp.instance.PickUps[i].Event_Name, 0));
            }
            print("Save File's Format has Updated(Pick Up)");
            SavePickUpDataFile();
        }
    }
    public void SavePickUpDataFile() {
        string Pdata = JsonConvert.SerializeObject(List_PickUpData);
        File.WriteAllText(Application.streamingAssetsPath + "/" + FileName_PickUpData + ".json", Pdata);
    }

    public void Instantiate_Doll_ButtonList() {
        for(int i = List_DollButton.Count - 1; i >=0; i--) {
            Destroy(List_DollButton[i]);
        }
        List_DollButton.Clear();

        foreach (DollData doll in List_DollData) {
            if (doll.level > 0) {
                for(int i = 0; i < DollContainer.instance.Dolls.Count; i++) {
                    if(DollContainer.instance.Dolls[i].name == doll.name) {
                        doll_prefab = DollContainer.instance.Dolls[i];
                        doll_data = List_DollData[i];
                    }
                }
                GameObject dollbutton = Instantiate(GameManager.instance.Button_DollList_default);
                dollbutton.transform.SetParent(GameManager.instance.DollList_content);
                dollbutton.GetComponent<Image>().sprite = doll_prefab.GetComponent<DollController>().Sprite_Doll_face;
                dollbutton.GetComponent<Button_DollInfo>().sprite = doll_prefab.GetComponent<DollController>().Sprite_Doll;
                dollbutton.GetComponent<Button_DollInfo>().sprite_f = doll_prefab.GetComponent<DollController>().Sprite_Doll_face;
                dollbutton.GetComponent<Button_DollInfo>().Name = doll_prefab.name;
                Refresh_DollButton(dollbutton, doll_data);

                string _name = dollbutton.GetComponent<Button_DollInfo>().Name;
                dollbutton.GetComponent<Button>().onClick.AddListener(delegate { Click_DollList(_name, dollbutton); });

                List_DollButton.Add(dollbutton);
            }
        }
    }

    void Check_LevelData() {
        //LevelData.json 존재 확인
        FileInfo LevelDataFile = new FileInfo(Application.streamingAssetsPath + "/" + FileName_LevelData + ".json");
        //-존재하지 않으면 List_LevelData를 기준으로 새로 생성
        if (!LevelDataFile.Exists) {
            //print("No Level Data File");
            CreateLevelDataFile();
        }
        //-존재하면 LevelData.json 불러오기
        LoadLevelDataFile();
    }
    void CreateLevelDataFile() {
        for (int i = 0; i < LevelContainer.instance.Levels.Count; i++) {
            //print("Add " + LevelContainer.instance.Levels[i]);
            List_LevelData.Add(new LevelData(i + 1));
        }
        List_LevelData[0].open = true;

        //새로운 파일 생성
        SaveLevelDataFile();
        print("Create New LevelData.json File");
    }
    void LoadLevelDataFile() {
        string Ldate = File.ReadAllText(Application.streamingAssetsPath + "/" + FileName_LevelData + ".json");
        List_LevelData = JsonConvert.DeserializeObject<List<LevelData>>(Ldate);
        //print("Load LevelData File");

        //레벨 데이터가 추가되었을 시
        if (List_LevelData.Count != LevelContainer.instance.Levels.Count) {
            int length = List_LevelData.Count;
            for (int i = length; i < LevelContainer.instance.Levels.Count; i++) {
                List_LevelData.Add(new LevelData(i + 1));
            }
            print("Save File's Format has Updated(level data)");
            SaveLevelDataFile();
        }
    }
    public void SaveLevelDataFile() {
        for(int i = 0; i < LevelContainer.instance.Levels.Count - 1; i++) {
            if(List_LevelData[i].count_clear > 0) {
                List_LevelData[i + 1].open = true;
            }
        }

        string Ldata = JsonConvert.SerializeObject(List_LevelData);
        File.WriteAllText(Application.streamingAssetsPath + "/" + FileName_LevelData + ".json", Ldata);
    }

    void CreatePlayerInfoFile() {
        //새로운 파일 생성
        SavePlayerInfoFile();
        print("Create New PlayerInfo.json File");
    }
    public void LoadPlayerInfoFile() {
        FileInfo PlayerInfoFile = new FileInfo(Application.streamingAssetsPath + "/" + FileName_PlayerInfo + ".json");
        //-존재하지 않으면 DollStateData를 기준으로 새로 생성
        if (!PlayerInfoFile.Exists) {
            CreatePlayerInfoFile();
        }

        string Pdate = File.ReadAllText(Application.streamingAssetsPath + "/" + FileName_PlayerInfo + ".json");
        Token = JsonConvert.DeserializeObject<int>(Pdate);
        //print("Load LevelData File");
    }
    public void SavePlayerInfoFile() {
        string Pdata = JsonConvert.SerializeObject(Token);
        File.WriteAllText(Application.streamingAssetsPath + "/" + FileName_PlayerInfo + ".json", Pdata);
    }

    public void Refresh_DollButton(GameObject target ,DollData dolldata) {
        target.GetComponent<Button_DollInfo>().echlon = dolldata.echlon;
        target.GetComponent<Button_DollInfo>().pos_x = dolldata.pos_x;
        target.GetComponent<Button_DollInfo>().pos_y = dolldata.pos_y;
    }

    bool isnull;
    public void Refresh_EchlonButton() {
        for (int i = 0; i < GameManager.instance.EchlonList_content.transform.childCount; i++) {
            Destroy(GameManager.instance.EchlonList_content.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < 5; i++) {
            isnull = true;
            GameObject button = Instantiate(GameManager.instance.Button_LevelList_default);
            button.transform.SetParent(GameManager.instance.EchlonList_content.transform);
            button.GetComponent<Button_EchlonInfo>().Text_EchlonName.text = (i + 1).ToString();
            button.GetComponent<Button_EchlonInfo>().index_Echlon = i + 1;

            //이미지 넣기
            int k = 0;
            for (int j = 0; j < List_DollButton.Count; j++) {
                if (List_DollButton[j].GetComponent<Button_DollInfo>().echlon == i + 1) {
                    button.GetComponent<Button_EchlonInfo>().Image_doll[k].sprite = List_DollButton[j].GetComponent<Button_DollInfo>().sprite_f;
                    k++;
                    isnull &= false;
                }
                else {
                    isnull &= true;
                }
            }

            if (isnull)
                button.SetActive(false);
        }
    }

    int index_doll;
    public void Click_DollList(string _name, GameObject button) {
        switch (MainMenuSceneController.instance.screenType) {
            case ScreenType.list: {
                    for (int i = 0; i < List_DollData.Count; i++) {
                        if (List_DollData[i].name.Equals(_name)) {
                            index_doll = i;
                            break;
                        }
                    }
                    //레벨 가져오기
                    int level = List_DollData[index_doll].level;
                    //레벨 이용해서 계산/대입
                    //UIContainer_DollList.instance.Image_Level = 
                    UIContainer_DollList.instance.Image_Sprite.sprite = DollContainer.instance.Dolls[index_doll].GetComponent<DollController>().Sprite_Doll;
                    UIContainer_DollList.instance.Text_acc.text = "명중 " + (List_DollState[index_doll].accuracy + (int)(List_DollState[index_doll]._accuracy * (level - 1))).ToString();
                    UIContainer_DollList.instance.Text_armor.text = "장갑 " + (List_DollState[index_doll].armor + (int)(List_DollState[index_doll]._armor * (level - 1))).ToString();
                    UIContainer_DollList.instance.Text_armorpen.text = "관통 " + List_DollState[index_doll].armorpen.ToString();
                    UIContainer_DollList.instance.Text_belong.text = List_DollState[index_doll].belong;
                    UIContainer_DollList.instance.Text_class.text = List_DollState[index_doll]._class;
                    UIContainer_DollList.instance.Text_crit.text = "치명률 " + List_DollState[index_doll].critrate.ToString();
                    UIContainer_DollList.instance.Text_dmg.text = "화력 " + (List_DollState[index_doll].damage + (int)(List_DollState[index_doll]._damage * (level - 1))).ToString();
                    UIContainer_DollList.instance.Text_eva.text = "회피 " + (List_DollState[index_doll].evasion + (int)(List_DollState[index_doll]._evasion * (level - 1))).ToString();
                    UIContainer_DollList.instance.Text_hp.text = "체력 " + (List_DollState[index_doll].hp + (int)(List_DollState[index_doll]._hp * (level - 1))).ToString();
                    UIContainer_DollList.instance.Text_Level.text = level.ToString();
                    UIContainer_DollList.instance.Text_Name.text = List_DollState[index_doll].name;
                    UIContainer_DollList.instance.Text_rof.text = "사속 " + (List_DollState[index_doll].rateoffire + (int)(List_DollState[index_doll]._rateoffire * (level - 1))).ToString();
                    UIContainer_DollList.instance.Text_speed.text = "기동 " + List_DollState[index_doll].speed.ToString();
                    
                    //스킬 정보 보여주기
                    SkillBase sb = DollContainer.instance.Dolls[index_doll].GetComponent<SkillBase>();
                    UIContainer_DollList.instance.Image_Skill_Icon.sprite = sb.skill_icon;
                    UIContainer_DollList.instance.Text_Skill_CoolTime.text = "쿨타임:" + sb.skill_Cooltime[level - 1].ToString();
                    UIContainer_DollList.instance.Text_Skill_Duration.text = "지속시간:" + sb.skill_Duration[level - 1].ToString();
                    sb.SkillDescribe();
                    UIContainer_DollList.instance.Text_Skill_Explaination.text = sb.skill_describe;
                    UIContainer_DollList.instance.Text_Skill_Name.text = sb.skill_name;

                    MainMenuSceneController.instance.Show_DollInfo();
                }
                break;
            case ScreenType.formation: {
                    //print("click doll button : " + button.GetComponent<Button_DollInfo>().Name);
                    FormationController.instance.Set_Clicked_Button(button, _name);
                }
                break;
            default:
                Debug.LogError("Wrong way to click Doll List");
                break;
        }
    }
}
