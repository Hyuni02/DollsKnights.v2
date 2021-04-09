using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_Event : MonoBehaviour
{
    public string eventName;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(delegate { SetEvent(); });
    }

    void SetEvent() {
        RandomPickUp.instance.Select_Event(eventName);
    }
}
