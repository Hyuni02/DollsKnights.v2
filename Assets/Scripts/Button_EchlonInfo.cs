using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_EchlonInfo : MonoBehaviour
{
    public int index_Echlon;
    private bool notSelected = true;
    public Text Text_EchlonName;
    public List<Image> Image_doll;
    public Color M_notselected;
    public Color M_selected;

    private void Awake() {
        GetComponent<Button>().onClick.AddListener(delegate { Set_SelectedEchlon(gameObject, notSelected); });
    }

    void Set_SelectedEchlon(GameObject gameObject, bool add) {
        GameManager.instance.Set_SelectedEchlons(gameObject, add);
        Clicked();
    }

    public void Clicked() {
        notSelected = !notSelected;

        //색상 변경
        if (notSelected)
            GetComponent<Image>().color = M_notselected;
        else
            GetComponent<Image>().color = M_selected;
    }
}
