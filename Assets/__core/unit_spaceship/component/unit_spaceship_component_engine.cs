using UnityEngine;
using System.Collections;

public class unit_spaceship_component_engine : unit_spaceship_component {

    [HideInInspector] public unit_spaceship_controller_input controllerInput;
    public float dataForce = 1000f;

    void Start()
    {
        controllerInput = _unit.GetComponentInChildren<unit_spaceship_controller_input>();
    }


    void FixedUpdate()
    {
        if (controllerInput._data._transmission > 0)
        {

            _unit.rigidBody.AddRelativeForce(Vector3.forward * (dataForce * (controllerInput._data._transmission / unit_spaceship_controller_input._data_transmission_max)));
        }
    }

}
