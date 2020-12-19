using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public GameObject coin;
    void Update() {
        this.transform.Rotate(0.0f, 2.0f, 0.0f, Space.World);
    }

    public void DestroyMe() {
        if (this.gameObject != null) 
            Destroy(this.gameObject);
    }
}
