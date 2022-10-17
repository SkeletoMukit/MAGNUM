using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rbPlayer;

    public Transform trPlayer;
    public Transform trCamera;

    private float ZAxisInput;
    private float ZAxisVelocity;
    private int ZAxisCanSpeedUp;

    private float XAxisInput;
    private float XAxisVelocity;
    private int XAxisCanSpeedUp;

    private float speed;
    public int speedGround;
    public int speedAir;
    public int speedRun;

    public float maxSpeedGround;
    public float maxSpeedAir;
    public float maxSpeedRun;
    private float maxSpeed;
    private float maxSpeedCombined;

    private float XMouseInput;
    public float XMouseSens;

    private float YMouseInput;
    public float YMouseSens;

    public GroundTrigger groundTrigger;
    private bool isGrounded;

    private bool jump;
    public float spaceBufffer;
    private float spaceBufferRemaining;
    public float jumpCoolDown;
    private float jumpCoolDownRemainig;

    public GameObject projectile;

    public float rateOfFire;
    private float rateOfFireRemainig;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        //Application.targetFrameRate = 144;
    }

    // Update is called once per frame
    void Update()
    {
        ZAxisInput = Input.GetAxisRaw("Z Axis");
        XAxisInput = Input.GetAxisRaw("X Axis");

        XMouseInput = Input.GetAxisRaw("Mouse X");
        YMouseInput = Input.GetAxisRaw("Mouse Y");

        trPlayer.eulerAngles += new Vector3(0f, XMouseInput * XMouseSens, 0f);

        if ((YMouseInput < 0f && (trCamera.eulerAngles.x > 280 || trCamera.eulerAngles.x < 90)) || (YMouseInput > 0f && (trCamera.eulerAngles.x < 80 || trCamera.eulerAngles.x > 270)))
        {
            trCamera.eulerAngles += new Vector3(YMouseInput * YMouseSens,0f, 0f);
        }


        ZAxisVelocity = rbPlayer.transform.InverseTransformDirection(rbPlayer.velocity).z;
        XAxisVelocity = rbPlayer.transform.InverseTransformDirection(rbPlayer.velocity).x;
        

        if (isGrounded == true && Input.GetAxisRaw("run") > 0)
        {
            maxSpeed = maxSpeedRun;
            speed = speedRun;
        }
        else if (isGrounded == true)
        {
            maxSpeed = maxSpeedGround;
            speed = speedGround;
        }
        else
        {
            maxSpeed = maxSpeedAir;
            speed = speedAir;
        }


        if (XAxisInput != 0 && ZAxisInput != 0)
        {
            maxSpeedCombined = maxSpeed * 1.5f;
            maxSpeed *= 0.75F;
            speed *= 0.75F;
            
        }
        else
        {
            maxSpeedCombined = maxSpeed*1.25f;
        }

        

        if (((ZAxisInput > 0 && ZAxisVelocity > maxSpeed) || (ZAxisInput < 0 && ZAxisVelocity < maxSpeed * -1)) || (isGrounded && Mathf.Abs(ZAxisVelocity) + Mathf.Abs(XAxisVelocity) > maxSpeedCombined))
        {
            ZAxisCanSpeedUp = 0;
        }
        else
        {
            ZAxisCanSpeedUp = 1;
        }

        if (((XAxisInput > 0 && XAxisVelocity > maxSpeed) || (XAxisInput < 0 && XAxisVelocity < maxSpeed * -1)) || (isGrounded && Mathf.Abs(ZAxisVelocity) + Mathf.Abs(XAxisVelocity) > maxSpeedCombined))
        {
            XAxisCanSpeedUp = 0;
        }
        else
        {
            XAxisCanSpeedUp = 1;
        }


        isGrounded = groundTrigger.isGrounded;


        if(Input.GetKeyDown(KeyCode.Space))
        {
            spaceBufferRemaining = spaceBufffer;
        }
        if (spaceBufferRemaining > 0)
        {
            spaceBufferRemaining -= Time.deltaTime;
        }

        if (isGrounded == true && spaceBufferRemaining > 0 && jumpCoolDownRemainig <= 0)
        {
            spaceBufferRemaining = 0;
            jump = true;
        }

        if (jump == true)
        {
            jumpCoolDownRemainig = jumpCoolDown;
        }
        if (jumpCoolDownRemainig > 0)
        {
            jumpCoolDownRemainig -= Time.deltaTime;
        }

        if (jump == true)
        {
            rbPlayer.AddRelativeForce(new Vector3(0f, 300f, 0f));
            jump = false;
        }


        if (rateOfFire > 0)
        {
            rateOfFireRemainig -= Time.deltaTime;
        }
        if (Input.GetMouseButton(0) && rateOfFireRemainig <= 0)
        {
            rateOfFireRemainig = rateOfFire;
            Instantiate(projectile, trCamera.position, trCamera.rotation);
        }


    }

    private void FixedUpdate()
    {
        rbPlayer.AddRelativeForce(new Vector3(XAxisInput * speed * XAxisCanSpeedUp, 0F, ZAxisInput * speed * ZAxisCanSpeedUp));
    }
}
