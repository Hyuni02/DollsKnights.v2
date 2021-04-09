using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIContainer_LevelSelect : MonoBehaviour
{
    public static UIContainer_LevelSelect instance;

    public Text Text_Token;
    public GameObject Button_Prefab;
    [Header("Level List")]
    public GameObject Levellist_content;
    public GameObject Image_WorldMap;
    [Header("Level Info")]
    public GameObject Panel_LevelInfo;
    public Text Text_title;
    public Text Text_MaxEchlon;
    public GameObject Image_MapPreview;
    public GameObject Panel_Reward;

    public Button Button_Start;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        for(int i = 0; i < LevelContainer.instance.Levels.Count; i++) {
            GameObject button = Instantiate(Button_Prefab, Levellist_content.transform);
            button.GetComponent<LevelIndexInfo>().SetTitle(LevelContainer.instance.Levels[i].GetComponent<LevelInfo>().name);
            button.GetComponent<LevelIndexInfo>().SetIndex(LevelContainer.instance.Levels[i].GetComponent<LevelInfo>().index_level);
            button.GetComponent<LevelIndexInfo>().SetActive(GetData.instance.List_LevelData[i].open);
        }

        ViewToken();
    }

    void ViewToken() {
        GetData.instance.LoadPlayerInfoFile();
        Text_Token.text = GetData.instance.Token.ToString();
    }
}
