using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormationController : MonoBehaviour {
    public static FormationController instance;

    [SerializeField]
    int Index_Echlon, Index_Pos_X, Index_Pos_Y;
    [SerializeField]
    public GameObject Clicked_Button, Clicked_Node;
    public string Clicked_Button_Name;
    public Sprite btn_c_grey;
    public Sprite btn_c_emerald;

    DollData target_add;
    public List<GameObject> List_Echlon_Dolls = new List<GameObject>();

    private void Awake() {
        instance = this;
    }

    void Update() {
        if (MainMenuSceneController.instance.screenType != ScreenType.formation)
            return;

        //제대에서 인형 빼기
        if(Clicked_Button == null && Clicked_Node  != null) {
            Index_Pos_X = (int)Clicked_Node.GetComponent<FormationNodeIndex>().index.x;
            Index_Pos_Y = (int)Clicked_Node.GetComponent<FormationNodeIndex>().index.y;

            Remove_Doll();
            Initialize();
            Set_Null();
        }

        //제대에 인형 넣기/교체
        if (Clicked_Button != null && Clicked_Node != null) {
            Index_Pos_X = (int)Clicked_Node.GetComponent<FormationNodeIndex>().index.x;
            Index_Pos_Y = (int)Clicked_Node.GetComponent<FormationNodeIndex>().index.y;

            //선택한 좌표에 인형이 존재하면 제대를 0으로 변경
            Remove_Doll();

            if (List_Echlon_Dolls.Count == 5) {
                print("Max Count in Echlon is 5 Dolls");
            }
            else {
                //선택한 인형을 선택한 좌표에 넣기
                //인형 데이터 수정/저장
                for (int i = 0; i < GetData.instance.List_DollData.Count; i++) {
                    if (GetData.instance.List_DollData[i].name.Equals(Clicked_Button_Name)) {
                        target_add = GetData.instance.List_DollData[i];
                        break;
                    }
                }
                Set_Doll_Formation(target_add, Index_Echlon, Index_Pos_X, Index_Pos_Y);
                //print("set " + Clicked_Button_Name);

                Initialize();
            }
            Set_Null();
        }
    }

    public void Set_Echlon(int index) {
        Index_Echlon = index;
    }
    public void Set_Clicked_Button(GameObject button, string _name) {
        Clicked_Button = button;
        Clicked_Button_Name = _name;
    }
    public DollData Find_Doll(int echlon, int x, int y) {
        for(int i =0; i < GetData.instance.List_DollData.Count; i++) {
            DollData target = GetData.instance.List_DollData[i];
            if (target.echlon == echlon && target.pos_x == x && target.pos_y == y) {
                return target;
            }
        }
        return null;
    }
    public void Remove_Doll() {
        DollData target_remove = Find_Doll(Index_Echlon, Index_Pos_X, Index_Pos_Y);
        if (target_remove != null) {
            Set_Doll_Formation(target_remove, 0, 0, 0);
            //print("remove " + target_remove.name);
        }
        Initialize();
    }
    public void Set_Doll_Formation(DollData target, int echlon, int x, int y) {
        target.echlon = echlon;
        target.pos_x = x;
        target.pos_y = y;
    }
    public void Refresh_Formation_Image() {
        for (int i = 0; i < UIContainer_Formation.instance.Image_Dolls.Length; i++) {
            UIContainer_Formation.instance.Image_Dolls[i].sprite = btn_c_emerald;
        }

        for(int i=0; i < List_Echlon_Dolls.Count; i++) {
            UIContainer_Formation.instance.Image_Dolls[i].sprite = List_Echlon_Dolls[i].GetComponent<Button_DollInfo>().sprite_f;
        }
        //print("refresh image");
    }
    public void Refresh_Formation_Pos_Image() {
        for(int i = 0; i < UIContainer_Formation.instance.Image_Nodes.Length; i++) {
            UIContainer_Formation.instance.Image_Nodes[i].sprite = btn_c_grey;
        }

        for(int i = 0; i < List_Echlon_Dolls.Count; i++) {
            for(int j = 0; j < UIContainer_Formation.instance.Image_Nodes.Length; j++) {
                if(UIContainer_Formation.instance.Image_Nodes[j].GetComponent<FormationNodeIndex>().index.x == List_Echlon_Dolls[i].GetComponent<Button_DollInfo>().pos_x 
                    && UIContainer_Formation.instance.Image_Nodes[j].GetComponent<FormationNodeIndex>().index.y == List_Echlon_Dolls[i].GetComponent<Button_DollInfo>().pos_y) {
                    UIContainer_Formation.instance.Image_Nodes[j].sprite = List_Echlon_Dolls[i].GetComponent<Button_DollInfo>().sprite_f;
                }
            }
        }
        //print("refresh formation");
    }

    public void Initialize() {
        //인형 리스트 버튼 새로고침
        MainMenuSceneController.instance.Refresh_Doll_ButtonList(Index_Echlon);

        //저장
        GetData.instance.SaveDollDataFile();

        Refresh_Formation_Image();
        Refresh_Formation_Pos_Image();
    }
    public void Set_Null() {
        Clicked_Button = null;
        Clicked_Node = null;
        Clicked_Button_Name = null;
    }

}
