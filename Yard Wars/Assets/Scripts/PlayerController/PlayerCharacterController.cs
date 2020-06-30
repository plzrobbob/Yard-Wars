using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    public CharacterController CharController;

    public float Gravity = -9.81f;
    private float fallmult = 2.5f; //increase gravity pull for better feel
    private float MoveSpeed = 5;
    
    private Quaternion transformrotation;
    public Vector3 moveRotation = Vector3.zero;
    private Vector3 moveDirection;
    private Vector3 Velocity;


    // Start is called before the first frame update
    void Start()
    {
        CharController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
        LookDirection();
    }

    void Movement() 
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //transform.right and transform.forward uses local coords instead of world coords
        Vector3 move = transform.right * x + transform.forward * z;

        if (!CharController.isGrounded)
        {
            Velocity += Vector3.up * Gravity * (fallmult - 1) * Time.deltaTime;
        }

        CharController.Move(move * MoveSpeed * Time.deltaTime);

        //applies forces on the y axis from jumping or gravity or jetpack
        //-9.81m/s * t * t
        CharController.Move(Velocity * Time.deltaTime);
    }

    void LookDirection()//use for 3rd person look direction
    {                                                                                               //only rotates while player is moving    WHY?
        moveRotation = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        
        
        if (moveRotation.magnitude > 0)
        {
            Vector3 fwd = transform.position - Camera.main.transform.position;
            fwd.y = 0;
            fwd = fwd.normalized;
            if (fwd.magnitude > 0)
            {
                Quaternion inputFrame = Quaternion.LookRotation(fwd, Vector3.up);
                moveRotation = inputFrame * moveRotation;
                if (moveRotation.magnitude > 0)
                {
                    moveRotation *= MoveSpeed;
                    transformrotation = Quaternion.LookRotation(moveRotation.normalized, Vector3.up);
                    transform.rotation = transformrotation;     //use this to slerp rtation Quaternion.Slerp(transform.rotation, transformrotation, Time.deltaTime * 10f);
                }
            }
        }
        
        if (moveRotation.magnitude <= 0)
        {
            Vector3 fwd = transform.position - Camera.main.transform.position;
            fwd.y = 0;
            fwd = fwd.normalized;
            if (fwd.magnitude >= 0)
            {
                Quaternion inputFrame = Quaternion.LookRotation(fwd, Vector3.up);
                if (moveRotation.magnitude <= 0)
                {
                    transform.rotation = inputFrame;
                }
            }
        }
        




        moveDirection.y = 0;
        moveDirection.x = moveRotation.x;
        moveDirection.z = moveRotation.z;
        CharController.Move(moveDirection * Time.deltaTime);
    }
}