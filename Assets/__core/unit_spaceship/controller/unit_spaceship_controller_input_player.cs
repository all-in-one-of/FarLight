using UnityEngine;
using System.Collections;

public class unit_spaceship_controller_input_player : unit_spaceship_controller {

	[HideInInspector] public unit_spaceship_controller_input controllerInput;
	

	void Start ()
    {
        controllerInput = _unit.GetComponentInChildren<unit_spaceship_controller_input>();
    }


	void Update()
    {

		update_transmission();
		update_horizontal();
		update_vertical();
		update_rotation();
	}


	void update_transmission() {

		if (Input.GetAxis("Mouse ScrollWheel")>0) {

			controllerInput._status._transmission+=unit_spaceship_controller_input._data_transmission_step;

			controllerInput._status._transmission=Mathf.Min(unit_spaceship_controller_input._data_transmission_max, controllerInput._status._transmission);
		}

		if (Input.GetAxis("Mouse ScrollWheel")<0) {

			controllerInput._status._transmission-=unit_spaceship_controller_input._data_transmission_step;

			controllerInput._status._transmission=Mathf.Max(unit_spaceship_controller_input._data_transmission_min, controllerInput._status._transmission);
		}

	}

	void update_horizontal() {


		if (Input.GetKeyDown(KeyCode.D)) {

			controllerInput._status._movement.x=1;

			return;
		}

		if (Input.GetKeyUp(KeyCode.D)) {

			if (Input.GetKey(KeyCode.A)) {

				controllerInput._status._movement.x=-1;

				return;
			}

			controllerInput._status._movement.x=0;

			return;
		}

		if (Input.GetKeyDown(KeyCode.A)) {

			controllerInput._status._movement.x=-1;

			return;
		}

		if (Input.GetKeyUp(KeyCode.A)) {

			if (Input.GetKey(KeyCode.D)) {

				controllerInput._status._movement.x=1;

				return;
			}

			controllerInput._status._movement.x=0;

			return;
		}

	}

	void update_vertical() {

		if (Input.GetKeyDown(KeyCode.W)) {

			controllerInput._status._movement.y=1;

			return;
		}

		if (Input.GetKeyUp(KeyCode.W)) {

			if (Input.GetKey(KeyCode.S)) {

				controllerInput._status._movement.y=-1;

				return;
			}

			controllerInput._status._movement.y=0;

			return;
		}

		if (Input.GetKeyDown(KeyCode.S)) {

			controllerInput._status._movement.y=-1;

			return;
		}

		if (Input.GetKeyUp(KeyCode.S)) {

			if (Input.GetKey(KeyCode.W)) {

				controllerInput._status._movement.y=1;

				return;
			}

			controllerInput._status._movement.y=0;

			return;
		}

	}

	void update_rotation() {

		if (Input.GetKeyDown(KeyCode.E)) {

			controllerInput._status._movement.z=1;

			return;
		}

		if (Input.GetKeyUp(KeyCode.E)) {

			if (Input.GetKey(KeyCode.Q)) {

				controllerInput._status._movement.z=-1;

				return;
			}

			controllerInput._status._movement.z=0;

			return;
		}

		if (Input.GetKeyDown(KeyCode.Q)) {

			controllerInput._status._movement.z=-1;

			return;
		}

		if (Input.GetKeyUp(KeyCode.Q)) {

			if (Input.GetKey(KeyCode.E)) {

				controllerInput._status._movement.z=1;

				return;
			}

			controllerInput._status._movement.z=0;

			return;
		}

	}
}
