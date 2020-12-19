using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFrosty : MonoBehaviour
{
    void Update()
    {
        this.transform.Rotate(0.0f, 40f * Time.deltaTime, 0.0f, Space.World);
    }
}
