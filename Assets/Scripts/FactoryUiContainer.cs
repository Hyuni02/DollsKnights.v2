using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactoryUiContainer : MonoBehaviour
{
    public static FactoryUiContainer instance;

    public Text Text_Token;
    public Button Button_PickUp;
    public GameObject content_Button_PickUp;
    public GameObject Panel_TierList;
    public Image Image_ThumbNail;
    public List<GameObject> List_EventButtons;
    public Transform content_TierList;
    public List<GameObject> List_TierList;
    public GameObject Prefab_Tier;

    void Awake() {
        instance = this;
    }

    private void Start() {
        ViewToken();
    }

    void ViewToken() {
        GetData.instance.LoadPlayerInfoFile();
        Text_Token.text = GetData.instance.Token.ToString();
    }
}
