using UnityEngine;
using System.Collections;

[System.Serializable]
public class unit_spaceship_controller_input_data {

	public Vector3 _movement;
	public Vector3 _direction = Vector3.forward;
	public float _transmission;

}

public class unit_spaceship_controller_input : unit_spaceship_controller {
	
	//	output data...
	public unit_spaceship_controller_input_data _status;
	//	input data...
	public unit_spaceship_controller_input_data	_data;


	public static int _data_transmission_step = 1;
	public static int _data_transmission_max = 2;
	public static int _data_transmission_min = -1;

	void Update() {

		update_data();
	}


	void update_data() {
		//	это для мультиплеера...
		_data = _status;
	}


}
