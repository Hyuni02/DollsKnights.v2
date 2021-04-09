using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Doll Container")]
    public Transform DollListContainer;
    public Transform DollList_content;
    public GameObject Button_DollList_default;
    [Header("Level Container")]
    public Transform EchlonListContainer;
    public Transform EchlonList_content;
    public GameObject Button_LevelList_default;

    [Header("Ready for Combat")]
    public int Index_SelectedLevel;
    public List<GameObject> Index_SelectedEchlons = new List<GameObject>();


    private void Awake() {
        instance = this;
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        Close_DollListContainer();
        Close_EchlonListContainer();
    }

    public void Set_SelectedLevel(int index) {
        Index_SelectedLevel = index;
        ShowLevelInfo(index);
    }
    int maxEchlonCount = 0;
    void ShowLevelInfo(int index) {
        LevelInfo levelinfo = LevelContainer.instance.Levels[index].GetComponent<LevelInfo>();
        UIContainer_LevelSelect uIContainer_LevelSelect = UIContainer_LevelSelect.instance;

        uIContainer_LevelSelect.Text_title.text = levelinfo.title;
        //mapPreview
        maxEchlonCount = levelinfo.max_echlon_count;
        uIContainer_LevelSelect.Text_MaxEchlon.text = "Max Echlon : " + levelinfo.max_echlon_count;
    }
    public void Set_SelectedEchlons(GameObject echlon, bool add) {
        if (add) {
            if (Index_SelectedEchlons.Count >= maxEchlonCount) {
                Index_SelectedEchlons[maxEchlonCount - 1].GetComponent<Button_EchlonInfo>().Clicked();
                Index_SelectedEchlons.RemoveAt(maxEchlonCount - 1);
            }
            Index_SelectedEchlons.Add(echlon);
        }
        else
            Index_SelectedEchlons.Remove(echlon);
        }
    public void Reset_SelectedEchlons() {
        Index_SelectedEchlons.Clear();
    }

    public void Open_DollListContainer() {
        DollListContainer.gameObject.SetActive(true);
    }
    public void Close_DollListContainer() {
        DollListContainer.gameObject.SetActive(false);
    }
    public void Open_EchlonListContainer() {
        EchlonListContainer.gameObject.SetActive(true);
    }
    public void Close_EchlonListContainer() {
        EchlonListContainer.gameObject.SetActive(false);
    }
}
