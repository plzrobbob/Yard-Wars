using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CC_Kyle : MonoBehaviour
{
    [SerializeField] CharacterController Controller;
    [SerializeField] Camera PlayerCamera;
    [SerializeField] Transform HeadTarget;
    [SerializeField] Transform ChestTarget;

    [SerializeField] float Speed = 3.0f;
    [SerializeField] float RotateSpeed = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        if(Controller == null)
        {
            Debug.LogWarning("Please assign Controller in Inspector");
            Controller = GetComponent<CharacterController>();
        }

        if (PlayerCamera == null)
        {
            Debug.LogWarning("Please assign Player Camera in Inspector");
            PlayerCamera = GetComponentInChildren<Camera>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        CameraControl();
        CharacterMovement();
    }

    void FixedUpdate()
    {
        //Debug.Log(Controller.velocity);
    }

    void CharacterMovement()
    {
        //Character Movement
        transform.Rotate(0, Input.GetAxis("Horizontal") * RotateSpeed, 0);

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        float curSpeed = Speed * Input.GetAxis("Vertical");

        Controller.SimpleMove(forward * curSpeed);
    }

    void CameraControl()
    {
        float testSpeed = 20.0f;

        //Yaw
        if(Input.GetKey(KeyCode.E))
        {
            //PlayerCamera.transform.RotateAround(transform.position, Vector3.up, RotateSpeed);
            HeadTarget.RotateAround(transform.position, Vector3.up, testSpeed * Time.deltaTime);
            ChestTarget.RotateAround(transform.position, Vector3.up, testSpeed * Time.deltaTime);
        }
        else if(Input.GetKey(KeyCode.Q))
        {
            //PlayerCamera.transform.RotateAround(transform.position, Vector3.up, -RotateSpeed);
            HeadTarget.RotateAround(transform.position, Vector3.up, -testSpeed * Time.deltaTime);
            ChestTarget.RotateAround(transform.position, Vector3.up, -testSpeed * Time.deltaTime);
        }
        
        //Pitch
        if (Input.GetKey(KeyCode.Z))
        {
            //PlayerCamera.transform.RotateAround(transform.position, Vector3.up, RotateSpeed);
            HeadTarget.RotateAround(transform.position, Vector3.right, testSpeed * Time.deltaTime);
            ChestTarget.RotateAround(transform.position, Vector3.right, testSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.C))
        {
            //PlayerCamera.transform.RotateAround(transform.position, Vector3.up, -RotateSpeed);
            HeadTarget.RotateAround(transform.position, Vector3.right, -testSpeed * Time.deltaTime);
            ChestTarget.RotateAround(transform.position, Vector3.right, -testSpeed * Time.deltaTime);
        }

    }
}
