using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Image_bufficon : MonoBehaviour
{
    public void done(float duration) {
        StartCoroutine(destroy(duration));
    }

    public IEnumerator destroy(float duraton) {
        yield return new WaitForSeconds(duraton);
        gameObject.SetActive(false);
    }
}
