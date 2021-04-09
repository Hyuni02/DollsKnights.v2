using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    public Text text;
    public enum Type { miss, hit, crit, block, shield}
    public Type type;

    public void SetIndicator(Type type, int dmg, Vector3 pos) {
        text = transform.GetChild(0).GetComponent<Text>();
        text.text = dmg.ToString();
        transform.position = pos;

        switch (type) {
            case Type.miss:
                text.color = Color.gray;
                text.text = "miss";
                break;
            case Type.hit:
                text.color = Color.black;
                break;
            case Type.crit:
                text.color = Color.red;
                break;
            case Type.block:
                text.color = Color.yellow;
                break;
            case Type.shield:
                text.color = Color.white;
                break;
        }
        StartCoroutine(destroy());
    }

    IEnumerator destroy() {
        yield return new WaitForSeconds(0.7f);
        InGameManager.instance.ReturnIndicator(gameObject.GetComponent<DamageIndicator>());
    }
}
