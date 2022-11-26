using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using DG.Tweening;
//using UnityEngine.Rendering.PostProcessing;
//using Cinemachine;

public class Car_Move : MonoBehaviour
{
    private SpawnPointManager spawnPointManager;

    public Transform carModel;
    public Transform carNormal;
    public Rigidbody box;

    float speed, currentSpeed;
    float rotate, currentRotate;

    [Header("Parameters")]
    public float acceleration = 20f;
    public float steering = 80f;
    public float gravity = 10f;
    public LayerMask layerMask;

    [Header("Model Parts")]
    public Transform frontWheels;
    public Transform backWheels;
    //public Transform steeringWheel;

    public void Awake()
    {
        spawnPointManager = FindObjectOfType<SpawnPointManager>();
    }

    public void ApplyAcceleration(float input)
    {
        speed = acceleration * input;
        currentSpeed = Mathf.SmoothStep(currentSpeed, speed, Time.deltaTime * 12f);
        speed = 0f;
        currentRotate = Mathf.Lerp(currentRotate, rotate, Time.deltaTime * 4f);
        rotate = 0f;
    } 
    
    public void AnimateCar(float input)
    {
        carModel.localEulerAngles = Vector3.Lerp(carModel.localEulerAngles, new Vector3(0, 0, carModel.localEulerAngles.z), .2f);

        frontWheels.localEulerAngles = new Vector3(0, (input * 2), frontWheels.localEulerAngles.z);
        frontWheels.localEulerAngles += new Vector3(0, 0, 0); //ez is itt mi a fasz idk
        backWheels.localEulerAngles += new Vector3(0, 0, 0);

    }

    public void Respawn()
    {
        Vector3 pos = spawnPointManager.SelectRandomSpawnpoint();
        box.MovePosition(pos);
        transform.position = pos - new Vector3(0, 0.4f, 0);
    }

    public void FixedUpdate()
    {
        box.AddForce(carModel.transform.forward * currentSpeed, ForceMode.Acceleration);

        //Gravity
        box.AddForce(Vector3.down * gravity, ForceMode.Acceleration);

        //Follow Collider
        transform.position = box.transform.position - new Vector3(0, 0.4f, 0);

        //Steering
        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, transform.eulerAngles.y + currentRotate, 0), Time.deltaTime * 5f);

        Physics.Raycast(transform.position + (transform.up * .1f), Vector3.down, out RaycastHit hitOn, 1.1f, layerMask);
        Physics.Raycast(transform.position + (transform.up * .1f), Vector3.down, out RaycastHit hitNear, 2.0f, layerMask);

        //Normal Rotation
        carNormal.up = Vector3.Lerp(carNormal.up, hitNear.normal, Time.deltaTime * 8.0f);
        carNormal.Rotate(0, transform.eulerAngles.y, 0);
    }

    public void Steer(float steeringSignal)
    {
        int steerDirection = steeringSignal > 0 ? 1 : -1; 
        float steeringStrength = Mathf.Abs(steeringSignal);

        rotate = (steering * steerDirection) * steeringStrength;
    }

}
