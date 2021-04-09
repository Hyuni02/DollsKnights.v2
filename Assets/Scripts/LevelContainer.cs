using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelContainer : MonoBehaviour
{
    public static LevelContainer instance;

    public List<GameObject> Levels;

    private void Awake() {
        instance = this;
    }
}
