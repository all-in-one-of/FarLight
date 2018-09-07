using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissionControl : MonoBehaviour {

    public Material material;

	void Start () {
        material.EnableKeyword("_EMISSON");
    }
}
