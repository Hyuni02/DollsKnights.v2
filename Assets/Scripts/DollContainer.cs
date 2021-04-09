using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollContainer : MonoBehaviour
{
    public static DollContainer instance;
    public List<GameObject> Dolls;

    private void Awake() {
        instance = this;
    }
}
