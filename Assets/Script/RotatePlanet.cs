using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlanet : MonoBehaviour {

	public float angleSpeed = 0.01f;
    private Transform planet;
    private Vector3 rotate;

    void Awake()
    {
        planet = GetComponent<Transform>();
        rotate = new Vector3(0f, angleSpeed, 0f);
    }

    void Update ()
	{
        planet.Rotate(rotate * Time.deltaTime);
    }
}
