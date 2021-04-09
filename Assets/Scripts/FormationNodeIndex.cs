using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FormationNodeIndex : MonoBehaviour
{
    public Vector2 index;

    public void Start() {
        GetComponent<Button>().onClick.AddListener(delegate { Clicked(); });
    }

    public void Clicked() {
        FormationController.instance.Clicked_Node = gameObject;
    }
}
