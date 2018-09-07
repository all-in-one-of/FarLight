using UnityEngine;
using System.Collections;

public class unit_spaceship_controller_camera : unit_spaceship_controller {

	[HideInInspector] public unit_spaceship_controller_input controllerInput;

	public Transform transformTarget;
	public Transform transformCamera;

	[HideInInspector] public Vector3 transformCameraPosition;
	[HideInInspector] public Quaternion transformCameraRotation;


	void Start() {

        controllerInput = _unit.GetComponentInChildren<unit_spaceship_controller_input>();

        IdentifyTransform();
    }


	void LateUpdate() {

		UpdateTransform();
		UpdateTransformLerp();
        UpdateDirection();
	}


	public void IdentifyTransform() {

        transformCameraPosition = transformCamera.position;
        transformCameraRotation = transformCamera.rotation;
	}

	public void UpdateTransform() {

        transformCamera.position = transformCameraPosition;
        transformCamera.rotation = transformCameraRotation;
	}

	public void UpdateTransformLerp() {

        transformCamera.position = Vector3.Lerp(transformCamera.position, transform.position, 5f * Time.deltaTime);
        transformCameraPosition = transformCamera.position;

        transformCamera.rotation = Quaternion.Lerp(transformCamera.rotation, transform.rotation, 5f * Time.deltaTime);
        transformCameraRotation = transformCamera.rotation;
	}

	void UpdateDirection() {

        controllerInput._status._direction = transformTarget.position - _unit.transform.position;
	}

}
