using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour {

    public float speed = 0.01f;
    public Vector3 axis = Vector3.up;

    private void Update()
    {
        transform.Rotate(axis, Time.smoothDeltaTime * speed);
    }
}
