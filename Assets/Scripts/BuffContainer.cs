using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffContainer : MonoBehaviour {
    public List<GameObject> BuffList = new List<GameObject>();
    int length = 0;

    CharacterBase cb;

    private void Start() {
        cb = GetComponent<CharacterBase>();
    }

    public void UpdateBuffViewer(GameObject buff) {
            Image icon = Instantiate(InGameManager.instance.bufficon, cb.BuffIconViewer.transform);
            icon.sprite = buff.GetComponent<Buff>().icon;
            icon.GetComponent<Image_bufficon>().done(buff.GetComponent<Buff>().duration);
    }
}
