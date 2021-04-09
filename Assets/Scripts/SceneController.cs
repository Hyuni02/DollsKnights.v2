using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;

    int Index_DataInitializeScene = 0;
    int Index_MainMenuScene = 1;
    int Index_FactoryScene = 2;
    int Index_InGameScene = 3;

    private void Awake() {
        instance = this;
    }

    public void ChangeScene(int index) {
        SceneManager.LoadScene(index);
    }
}
