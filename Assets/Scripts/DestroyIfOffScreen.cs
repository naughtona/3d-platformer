using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIfOffScreen : MonoBehaviour
{
    private float destroyTime = 2.0f;
    void Start() {
        Destroy(this.gameObject, destroyTime);
    }
}
