using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContainer : MonoBehaviour
{
    public static EnemyContainer instance;
    public List<GameObject> Enemies;

    private void Awake() {
        instance = this;
    }
}
