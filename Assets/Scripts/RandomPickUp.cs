using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PickUp {
    public string Event_Name;
    public int cost;
    public Sprite Image_EventThumbNail;
    public Sprite Image_ButtonThumbNail;
    public List<parts> List_Tier;

    public int pickup_Count;
    public bool pickup_Limit;
    public int limit;

    public PickUp(string _name, int _pickup_Count) {
        Event_Name = _name;
        pickup_Count = _pickup_Count;
    }
}
[System.Serializable]
public struct parts {
    public int possibility;
    public int tier;
    public List<GameObject> List_Doll;
}

public class RandomPickUp : MonoBehaviour
{
    public static RandomPickUp instance;

    public GameObject Button_prefab;

    [SerializeField]
    public List<PickUp> PickUps;
    PickUp Selected_Event;

    public GameObject Picked_Doll;

    private void Awake() {
        instance = this;
    }

    void Start()
    {
        GetData.instance.Check_PickUpData();

        List_Events();
        Select_Event("일반제조");
    }

    //이벤트 리스트 나열(버튼 생성)
    void List_Events() {
        for(int i = 0; i < PickUps.Count; i++) {
            GameObject button = Instantiate(Button_prefab, FactoryUiContainer.instance.content_Button_PickUp.transform);
            FactoryUiContainer.instance.List_EventButtons.Add(button);
            button.GetComponentInChildren<Text>().text = PickUps[i].Event_Name;
            //button.GetComponent<Image>().sprite = PickUps[i].Image_ButtonThumbNail;
            button.GetComponent<Button_Event>().eventName = PickUps[i].Event_Name;

            if (PickUps[i].pickup_Limit && PickUps[i].limit <= GetData.instance.List_PickUpData[i].pickup_Count) {
                button.SetActive(false);
            }
        }
    }

    //이벤트 선택(이름)
    public void Select_Event(string eventName) {
        for (int i = 0; i < PickUps.Count; i++) {
            if (PickUps[i].Event_Name.Equals(eventName)) {
                Selected_Event = PickUps[i];
                break;
            }
        }

        print("Selected Event : " + Selected_Event.Event_Name);
        //FactoryUiContainer.instance.Image_ThumbNail.sprite = Selected_Event.Image_EventThumbNail;
        ViewTierList();
        CheckToken();
    }

    GameObject[] temps;
    void ViewTierList() {
        temps = GameObject.FindGameObjectsWithTag("tierviewer");
        foreach(var t in temps) {
            Destroy(t);
        }

        for(int i = 0; i < Selected_Event.List_Tier.Count; i++) {
            GameObject tier = Instantiate(FactoryUiContainer.instance.Prefab_Tier, FactoryUiContainer.instance.content_TierList);
            tier.GetComponent<content_Tier>().Text_Tier.text = Selected_Event.List_Tier[i].tier.ToString();
            for(int j = 0; j < Selected_Event.List_Tier[i].List_Doll.Count; j++) {
                Image image = Instantiate(tier.GetComponent<content_Tier>().prefab, tier.GetComponent<content_Tier>().content_Dolls);
                image.gameObject.SetActive(true);
                image.sprite = Selected_Event.List_Tier[i].List_Doll[j].GetComponent<DollController>().Sprite_Doll_face;
            }
        }
    }

    void CheckToken() {
        if(GetData.instance.Token >= Selected_Event.cost) {
            FactoryUiContainer.instance.Button_PickUp.interactable = true;
        }
        else {
            FactoryUiContainer.instance.Button_PickUp.interactable = false;
        }
    }

    //뽑기
    int index = 0;
    public void PickUp() {
        GetData.instance.Token -= Selected_Event.cost;
        int pick_tier = Random.Range(0, 100);
        index = Selected_Event.List_Tier[0].possibility;
        for(int i = 0; i < Selected_Event.List_Tier.Count; i++) {
            if(index > pick_tier) {
                int pick_doll = Random.Range(0, Selected_Event.List_Tier[i].List_Doll.Count);
                print(Selected_Event.List_Tier[i].tier + " : " + Selected_Event.List_Tier[i].List_Doll[pick_doll].name);

                break;
            }
            else {
                index += Selected_Event.List_Tier[i + 1].possibility;
            }
        }


        GetData.instance.List_PickUpData.Find(x => x.Event_Name.Equals(Selected_Event.Event_Name)).pickup_Count++;
        GetData.instance.SavePickUpDataFile();
        GetData.instance.SavePlayerInfoFile();

        //뽑은 인형 보여주기


        SceneManager.LoadScene("FactoryScene");
    }

}
