using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AscensionController : MonoBehaviour
{
    public GameObject ascensionEffectTemplate;
    
    void Start() {
        GameObject ascensionEffect = Instantiate(this.ascensionEffectTemplate);
        ascensionEffect.transform.position = this.transform.position;
    }
}
