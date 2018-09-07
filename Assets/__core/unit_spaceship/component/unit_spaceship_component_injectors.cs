using UnityEngine;
using System.Collections;

public class unit_spaceship_component_injectors : unit_spaceship_component {

	[HideInInspector] public unit_spaceship_controller_input controllerInput;

	public float forceBack = 1000f;
	public float forceHorizontal = 500f;
	public float forceVertical = 500f;
	public float rotateDirection = 120f;
	//public float _data_rotate_forward = 90f;

	void Start() {

		controllerInput = _unit.GetComponentInChildren<unit_spaceship_controller_input>();
	}


	void FixedUpdate()
	{
		UpdateDirection();
		UpdateForce();
		UpdateBack();

	}

	
	void UpdateDirection() {

		if (controllerInput._data._direction == transform.forward) {

			return;
		}

		Quaternion direction = Quaternion.LookRotation(controllerInput._data._direction, transform.up);
		Quaternion forward = Quaternion.AngleAxis(-controllerInput._data._movement.z * rotateDirection * Time.fixedDeltaTime, transform.forward) * direction;

		_unit.rigidBody.MoveRotation(Quaternion.RotateTowards(_unit.rigidBody.rotation, forward, rotateDirection * Time.fixedDeltaTime));
	}

	void UpdateForce() {

		if (controllerInput._data._movement.x != 0) {

			_unit.rigidBody.AddRelativeForce(Vector3.right * controllerInput._data._movement.x * forceHorizontal);
		}

		if (controllerInput._data._movement.y != 0) {

			_unit.rigidBody.AddRelativeForce(Vector3.up * controllerInput._data._movement.y * forceVertical);
		}

	}

	void UpdateBack() {

		if (controllerInput._data._transmission < 0) {

			_unit.rigidBody.AddRelativeForce(Vector3.back * forceBack);
		}

	}

}
