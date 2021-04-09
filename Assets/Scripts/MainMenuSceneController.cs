using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ScreenType { lobby, list, formation, factory, level }

public class MainMenuSceneController : MonoBehaviour
{
    public static MainMenuSceneController instance;
    public ScreenType screenType;
    [Header("Lobby")]
    public GameObject Image_Partner;
    public GameObject Image_Background;
    public GameObject Panel_Setting;
    [Header("Level Select")]
    //public GameObject Button_LevelSelect;
    public GameObject Panel_LevelSelect;
    [Header("Doll List")]
    //public GameObject Button_DollList;
    public GameObject Panel_DollList;
    [Header("Formation")]
    //public GameObject Button_Formation;
    public GameObject Panel_Formation;
    [Header("Factory")]
    public GameObject Button_Factory;

    private void Awake() {
        instance = this;
    }

    void Start()
    {
        //==Todo    배경 이미지 설정
        //==Todo    부관 이미지 설정

        //패널 초기화
        Close_Panel_All();
    }

    void Update()
    {
        if (GameManager.instance.Index_SelectedEchlons.Count == 0)
            UIContainer_LevelSelect.instance.Button_Start.interactable = false;
        else
            UIContainer_LevelSelect.instance.Button_Start.interactable = true;
    }

    public void Refresh_Doll_ButtonList(int index = 0) {
        for (int i = 0; i < GetData.instance.List_DollButton.Count; i++) {
            GetData.instance.List_DollButton[i].SetActive(false);
        }
        switch (screenType) {
            case ScreenType.list:
                //print("Refresh Doll Button List(Doll List)");
                for (int i = 0; i < GetData.instance.List_DollButton.Count; i++) {
                    GetData.instance.List_DollButton[i].SetActive(true);
                }
                break;
            case ScreenType.formation:
                //print("Refresh Doll Button List(Formation)");
                for(int i = 0; i < GetData.instance.List_DollButton.Count; i++) {
                    for(int j = 0; j < GetData.instance.List_DollData.Count; j++) {
                        if (GetData.instance.List_DollButton[i].GetComponent<Button_DollInfo>().Name.Equals(GetData.instance.List_DollData[j].name)) {
                            GetData.instance.Refresh_DollButton(GetData.instance.List_DollButton[i], GetData.instance.List_DollData[j]);
                        }
                    }
                }
                FormationController.instance.List_Echlon_Dolls.Clear();
                for(int i = 0; i < GetData.instance.List_DollButton.Count; i++) {
                    if(GetData.instance.List_DollButton[i].GetComponent<Button_DollInfo>().echlon != index) {
                        GetData.instance.List_DollButton[i].SetActive(true);
                    }
                    else {
                        FormationController.instance.List_Echlon_Dolls.Add(GetData.instance.List_DollButton[i]);
                    }
                }
                break;
            default:
                Debug.LogError("Wrong Way to Refresh Doll Button List");
                break;
        }
    }

    public void Select_Echlon(int index) {
        if (index == 0) {
            Debug.LogError("Wrong Echlon index");
            return;
        }
        FormationController.instance.Set_Echlon(index);
        Emphasize_Button(index - 1);
        Refresh_Doll_ButtonList(index);
        FormationController.instance.Refresh_Formation_Image();
        FormationController.instance.Refresh_Formation_Pos_Image();
    }
    void Emphasize_Button(int index) {
        for(int i = 0; i < UIContainer_Formation.instance.Button_Echlon.Length; i++) {
            UIContainer_Formation.instance.Button_Echlon[i].GetComponent<Image>().color = Color.white;
        }
        UIContainer_Formation.instance.Button_Echlon[index].GetComponent<Image>().color = Color.green;
    }

    public void SetScreenType(ScreenType _screenType) {
        screenType = _screenType;
    }
    public void Close_Panel_All() {
        Close_Panel_DollList();
        Close_Panel_Formation();
        Close_Panel_LevelSelect();
        Close_Panel_Setting();
    }
    public void Open_Panel_DollList() {
        SetScreenType(ScreenType.list);
        UIContainer_DollList.instance.Panel_DollInfo.SetActive(false);
        Refresh_Doll_ButtonList();
        GameManager.instance.Open_DollListContainer();
        Panel_DollList.SetActive(true);
    }
    public void Show_DollInfo() {
        UIContainer_DollList.instance.Panel_DollInfo.SetActive(true);
    }
    public void Close_Panel_DollList() {
        SetScreenType(ScreenType.lobby);
        GameManager.instance.Close_DollListContainer();
        Panel_DollList.SetActive(false);
    }
    public void Open_Panel_Formation() {
        SetScreenType(ScreenType.formation);
        GameManager.instance.Open_DollListContainer();
        Select_Echlon(1);
        Panel_Formation.SetActive(true);
    }
    public void Close_Panel_Formation() {
        SetScreenType(ScreenType.lobby);
        GameManager.instance.Close_DollListContainer();
        Panel_Formation.SetActive(false);
    }
    public void Open_Panel_LevelSelect() {
        Close_Panel_LevelInfo();
        SetScreenType(ScreenType.level);
        GetData.instance.Refresh_EchlonButton();
        Panel_LevelSelect.SetActive(true);
    }
    public void Close_Panel_LevelSelect() {
        SetScreenType(ScreenType.lobby);
        Panel_LevelSelect.SetActive(false); 
    }
    public void Open_Panel_LevelInfo() {
        UIContainer_LevelSelect.instance.Panel_LevelInfo.SetActive(true);
        GameManager.instance.Open_EchlonListContainer();
    }
    public void Close_Panel_LevelInfo() {
        UIContainer_LevelSelect.instance.Panel_LevelInfo.SetActive(false);
        GameManager.instance.Close_EchlonListContainer();
        GameManager.instance.Reset_SelectedEchlons();
    }
    public void Close_EchlonListContainer() {
        GameManager.instance.Close_EchlonListContainer();
    }
    public void Open_Panel_Setting() {
        Panel_Setting.SetActive(true);
    }
    public void Close_Panel_Setting() {
        Panel_Setting.SetActive(false);
    }
}
