using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeInfo : MonoBehaviour
{
    public enum Type { low, high, enemyspawn, command}
    public Type type;
    [SerializeField]
    private Material[] materials;
    public bool placeable;
    public bool heilport;
    public Vector2 index;

    //[ContextMenu("Set Node Type")]
    public void SetNodeType() {
        switch (type) {
            case Type.low:
                GetComponent<MeshRenderer>().material = materials[0];
                transform.localScale = new Vector3(1, 0.1f, 1);
                break;
            case Type.high:
                GetComponent<MeshRenderer>().material = materials[1];
                transform.localScale = new Vector3(1, 1, 1);
                break;
            case Type.command:
                GetComponent<MeshRenderer>().material = materials[2];
                transform.localScale = new Vector3(1, 2, 1);
                break;
            case Type.enemyspawn:
                GetComponent<MeshRenderer>().material = materials[3];
                transform.localScale = new Vector3(1, 2, 1);
                break;
            default:
                Debug.LogError("Wrong Node Type : " + gameObject);
                break;
        }
    }
}
