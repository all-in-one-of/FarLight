using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour {

    private Transform m_camera;
    public float scaleFactor = 0.005f;

    private void Awake()
    {
        m_camera = Camera.main.transform;
    }

    private void Update ()
    {
        Vector3 vectorBetween = transform.position - m_camera.position;
        float scale = vectorBetween.magnitude * scaleFactor;

        transform.localScale = new Vector3(scale, scale, scale);
        transform.LookAt(m_camera);
    }
}
