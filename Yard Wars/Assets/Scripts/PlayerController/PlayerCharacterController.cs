using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    public CharacterController CharController;

    public float Gravity = -9.81f;
    public float MoveSpeed = 5;
    public float JumpHeight = 3f;
    private float GroundDistance = 0.4f;
    private float fallmult = 2.5f; //increase gravity pull for better feel
    private float height;
    public float SlopeRayLength = 1f;
    public float SlopeForce = 1f;

    public Transform GroundCheck;

    public LayerMask GroundMask;

    public GameObject gun;

    public GameObject camera;

    public GameObject aim_placeholder;

    private Vector3 Velocity;

    public bool Grounded;
    private bool CanJmp;
    public bool ThirdPesronCamera;
    public bool IsOnSlope;


    // Start is called before the first frame update
    void Start()
    {
        CharController = GetComponent<CharacterController>();
        height = CharController.height;
        camera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Grounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask, QueryTriggerInteraction.Ignore);
        if (!Grounded)
        {
            Grounded = CharController.isGrounded;
        }
        if (Grounded)
        {
            CanJmp = true;
        }
        OnSlope();
        Jump();
        Movement();
        ThirdPersonCameraLookDirection();
        gunrot();
    }

    void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //transform.right and transform.forward uses local coords instead of world coords
        Vector3 move = transform.right * x + transform.forward * z;

        //Debug.Log(Velocity);
        CharController.Move(Velocity * Time.deltaTime);
        CharController.Move(move * MoveSpeed * Time.deltaTime);
    }

    void Jump()
    {
        if (Grounded && Input.GetAxis("Jump") == 0)
        {
            Velocity.y = 0f;
        }
        if (Input.GetAxis("Jump") != 0 && CanJmp && Grounded)//get input for jump
        {
            Velocity.y = Mathf.Sqrt(JumpHeight * -2f * Gravity);//calculate velocity
            CanJmp = false;
        }
        if (!Grounded)
        {
            Velocity += Vector3.up * Gravity * (fallmult - 1) * Time.deltaTime; // increases fall gravity for better feel
        }
    }
    void ThirdPersonCameraLookDirection()
    {
        Vector3 fwd = transform.position - Camera.main.transform.position; //gets the position of the palyer in relation to the camera
        fwd.y = 0;//sets the position for the y axis to zero so rotation does not change the rotation in the y axis
        fwd = fwd.normalized;//normalize for cleaner rotation
        Quaternion rot = Quaternion.LookRotation(fwd, Vector3.up);//change the vector3 into a quaternion for rotation
        transform.rotation = rot;//apply rotation
    }

    void OnSlope()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (Input.GetAxis("Jump") != 0)
        {
            IsOnSlope = false;
            Jump();
            return;
        }

        //Debug.DrawRay(transform.position, Vector3.down, Color.blue, 10f);

        if (Physics.Raycast(transform.position, Vector3.down, out var hit, height / 2 * SlopeRayLength))
        {
            if (hit.normal != Vector3.up)
            {
                IsOnSlope = true;
            }
        }

        if ((z != 0 || x != 0) && IsOnSlope)
        {
            CharController.Move(Vector3.down * height / 2 * SlopeForce * Time.deltaTime);
        }
    }

    void gunrot()
    {
        //Vector3 v = gun.transform.rotation.eulerAngles;
        //gun.transform.rotation = Quaternion.Euler(camera.transform.rotation.eulerAngles.x, 0, 0);
        gun.transform.LookAt(aim_placeholder.transform);
    }
}