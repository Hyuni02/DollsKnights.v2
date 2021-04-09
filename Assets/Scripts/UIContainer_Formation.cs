using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIContainer_Formation : MonoBehaviour
{
    public static UIContainer_Formation instance;

    [Header("Formation")]
    public GameObject Panel_Formation;

    [Header("Image")]
    public Image[] Image_Dolls;
    public Image[] Image_Nodes;

    [Header("Button")]
    public Button[] Button_Echlon;

    public void Awake() {
        instance = this;
    }
}
