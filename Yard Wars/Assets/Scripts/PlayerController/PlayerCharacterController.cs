using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    public CharacterController CharController;

    public float Gravity = -9.81f;
    public float MoveSpeed;
    public float JumpHeight = 3f;
    private float GroundDistance = 0.4f;
    private float fallmult = 2.5f; //increase gravity pull for better feel
    private float height;
    public float SlopeRayLength = 1f;
    public float SlopeForce = 1f;
    public float TurnSpeed = 3f;
    public Transform InvisibleCameraOrigin;
    public float VerticalRotMin = -80;
    public float VerticalRotMax = 80;

    public Transform GroundCheck;

    public LayerMask GroundMask;

   // public GameObject gun;

    public GameObject camera;

    public GameObject aim_placeholder;

    private Vector3 Velocity;

    public bool Grounded;
    private bool CanJmp;
    public bool ThirdPesronCamera;
    public bool IsOnSlope;

    public PlaceDefense m_placeDefense;

    public Animator Player_Animator;



    public PlayerCharacterController pcc;
    public GameObject StunVFX;


    //Stuff Cameron added
    public float EditedSpeed;
    public float UnEditedSpeed;

    public bool test;

    //This is for sliding
    public Vector3 tempSlidingDirection;
    public Vector3 SlidingDirection;
    public bool SlidingBool;
    public GameObject Builder2TurnOff;

    // Start is called before the first frame update
    void Start()
    {
        m_placeDefense = this.GetComponentInChildren<PlaceDefense>();
        CharController = GetComponent<CharacterController>();
        height = CharController.height;
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        UnEditedSpeed = MoveSpeed;
        SlidingBool = false;
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
        
       // Movement();
       // ThirdPersonCameraLookDirection();
        gunrot();
        CameraRotate();

        if (m_placeDefense.placing)
        {
        //    gun.SetActive(false);
        }
        else
        {
          //  gun.SetActive(true);
        }

        if(test)
        {
            Stunned(4.0f);
        }

        if (SlidingBool)
        {
            Sliding();
        }
        else
        {
            Movement();
            Jump();
        }
    }

    void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if ((x != 0 || z != 0) && Grounded)
        {
            Player_Animator.SetBool("Walking", true);
        }
        else
        {
            Player_Animator.SetBool("Walking", false);
        }

        //transform.right and transform.forward uses local coords instead of world coords
        Vector3 move = transform.right * x + transform.forward * z;

        //Clamps the maximum magnitude to fix the issue where the player can press two movement input keys and increase their speed
        move = Vector3.ClampMagnitude(move, 1f);
        if (move.magnitude > 0)
        {
            tempSlidingDirection = move;
        }

        CharController.Move(Velocity * Time.deltaTime);//used for jumping and falling
        CharController.Move(move * MoveSpeed * Time.deltaTime);//used for moving

        Player_Animator.SetFloat("y", z);
        Player_Animator.SetFloat("x", x);



    }
    void CameraRotate()
    {
        var rotInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        var rot = transform.eulerAngles;
        rot.y += rotInput.x * TurnSpeed;
        transform.rotation = Quaternion.Euler(rot);

        if (InvisibleCameraOrigin != null)
        {
            rot = InvisibleCameraOrigin.localRotation.eulerAngles;
            rot.x -= rotInput.y * TurnSpeed;
            if (rot.x > 180)
                rot.x -= 360;
            rot.x = Mathf.Clamp(rot.x, VerticalRotMin, VerticalRotMax);
            InvisibleCameraOrigin.localRotation = Quaternion.Euler(rot);
        }
    }

    void Jump()
    {

        if (Grounded && Input.GetAxis("Jump") == 0)
        {
            Velocity.y = 0f;
            if (Player_Animator.GetBool("Jump"))
            {
                Player_Animator.SetBool("Landing", true);
            }
            Player_Animator.SetBool("Jump", false);
        }
        if (Input.GetAxis("Jump") != 0 && CanJmp && Grounded)//get input for jump
        {
            Velocity.y = Mathf.Sqrt(JumpHeight * -2f * Gravity);//calculate velocity
            CanJmp = false;
            Player_Animator.SetBool("Landing", false);
            Player_Animator.SetBool("Jump", true);
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

        if (Input.GetAxis("Jump") != 0 && !SlidingBool)
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
       // gun.transform.LookAt(aim_placeholder.transform);
    }








    //I AM CLAIMING THIS SECTION OF THE SCRIPT. I UNDERSTAND THIS IS BENS BUT I HAVE ALTERED THE DEAL. PRAY I DO NOT ALTER IT FURTHER.
    //    
    //    Signed,
    //          The muffled yelling from someone sounding similar to cameron from the safety of his Compile Bear Bunker.
    //  

    ///<summary> For both slowed and Faster
    ///float percentage is the percentage you want it to be, example if you want to decrease them by 30%, 
    ///You would set percentage to 0.7f
    ///Time is self explanatory how many seconds do you want them to be slowed by?
    /// </summary>
    public void Slowed(float percentage, float time)
    {
        EditedSpeed = UnEditedSpeed * percentage;
        MoveSpeed = EditedSpeed;
        Invoke("resetSpeed", time);
    }

    public void Faster(float percentage, float time)
    {
        EditedSpeed = UnEditedSpeed * percentage;
        MoveSpeed = EditedSpeed;
        Invoke("resetSpeed", time);

    }

    void resetSpeed()
    {
        MoveSpeed = UnEditedSpeed;
        Debug.Log("SpeedReset");
        Debug.Log(MoveSpeed);
    }

 

    public void Stunned(float time)
    {
        Debug.Log(gameObject.name + " is currently stunned for " + time + " seconds!");
        pcc.enabled = false;
        StunVFX.SetActive(true);

        Invoke("Unstunned", time);
        Player_Animator.SetBool("Stunned", true);
    }

    void Unstunned()
    {
        StunVFX.SetActive(false);

        pcc.enabled = true;
        Player_Animator.SetBool("Stunned", false);
    }

    /// <summary>
    /// I need to make it so it can tell that there is nothing
    /// </summary>

    void Sliding()
    {
        CharController.Move(Velocity * Time.deltaTime);
        CharController.Move(tempSlidingDirection * MoveSpeed * Time.deltaTime);
        Ray ray;
        RaycastHit hit;
        if (!Builder2TurnOff)
        {
            SlidingBool = false;
            Debug.Log("Hey so like Builder Ability 2 Marbles is totally gone. It should be gone.");
        }
        ray = new Ray(gameObject.transform.position, tempSlidingDirection);
        if (Physics.Raycast(gameObject.transform.position, tempSlidingDirection, out hit, 1))//cast the ray 1 unit at the specified direction
        {
            //This lets them bounce off whatever they hit, as it should be
            {
                print("Raycast hits wall");
                //find new ray direction
                Vector3 inDirection = Vector3.Reflect(ray.direction, hit.normal);
                Debug.DrawRay(hit.point, inDirection * 8, Color.magenta);
                Debug.Log(inDirection + "In Direction and then temp sliding direction" + tempSlidingDirection);

                tempSlidingDirection = inDirection.normalized * tempSlidingDirection.magnitude;
            }
        }
    }
}