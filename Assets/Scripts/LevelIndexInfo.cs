using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelIndexInfo : MonoBehaviour
{
    [SerializeField]
    private int index = 0;
    bool open;

    private void Awake() {
        GetComponent<Button>().onClick.AddListener(delegate { Set_SelectedLevel(index); });
    }

    void Set_SelectedLevel(int index) {
        GameManager.instance.Set_SelectedLevel(index);
        MainMenuSceneController.instance.Open_Panel_LevelInfo();
    }

    public void SetTitle(string name) {
        Text title = GetComponentInChildren<Text>();
        title.text = name;
    }

    public void SetIndex(int i) {
        index = i;
    }

    public void SetActive(bool _open) {
        open = _open;
        if (_open) {
            GetComponent<Button>().interactable = true;
        }
        else {
            GetComponent<Button>().interactable = false;
        }
    }
}
