using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryController : MonoBehaviour
{

    public List<DollState> SelectedRank_Dolls = new List<DollState>();
    public string result_name;

    void Start()
    {
        //캐릭터 티어별 확률 리스트 생성(2(60)%,3(20%),4(13%),5(7%))
    }

    public void ViewTierList() {

    }

    public void RandomDraw() {
        int result = Random.Range(0, 100);

        if(result < 7) {
            //5성 주기
            RandomSelect(5);
            print("5성 갯또다제");
        }
        else if(result >= 7 && result < 20) {
            //4성 주기
            RandomSelect(4);
            print("4성 갯또다제");

        }
        else if(result >=20 && result < 40) {
            //3성 주기
            RandomSelect(3);
            print("3성 갯또다제");

        }
        else {
            //2성 주기
            RandomSelect(2);
            print("2성 갯또다제");

        }
    }
    void RandomSelect(int rank) {
        SelectedRank_Dolls.Clear();
        for(int i = 0; i < GetData.instance.List_DollState.Count; i++) {
            if (GetData.instance.List_DollState[i].rank == rank)
                SelectedRank_Dolls.Add(GetData.instance.List_DollState[i]);
        }

        result_name = SelectedRank_Dolls[Random.Range(0, SelectedRank_Dolls.Count)].name;

        for(int i = 0; i < GetData.instance.List_DollData.Count; i++) {
            if (result_name.Equals(GetData.instance.List_DollData[i].name)) {
                print(GetData.instance.List_DollData[i].name);
                GetData.instance.List_DollData[i].level++;
                GetData.instance.SaveDollDataFile();
                GetData.instance.Instantiate_Doll_ButtonList();
                return;
            }
        }
    }
}
