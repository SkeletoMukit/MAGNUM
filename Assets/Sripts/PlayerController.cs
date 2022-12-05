using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    #region Variables
    public Rigidbody rbPlayer;

    public Transform trPlayer;
    public Transform trCamera;
    public Transform trBulletSpawn;

    private float ZAxisInput;
    private float ZAxisVelocity;
    private byte ZAxisCanSpeedUp;

    private float XAxisInput;
    private float XAxisVelocity;
    private byte XAxisCanSpeedUp;

    private float speed;
    public int speedGround;
    public int speedAir;
    public int speedRun;

    public Vector3 velocity;
    public float drag;

    public float maxSpeedGround;
    public float maxSpeedAir;
    public float maxSpeedRun;
    private float maxSpeed;
    private float maxSpeedCombined;

    public static float mouseSens = 1F;

    private float XMouseInput;
    public float XMouseSens = 1F;

    private float YMouseInput;
    public float YMouseSens = 1F;

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
    public int ammoMax;
    public static int ammoRemaining;
    public float reloadTimeMax = 0.8F;
    private float reloadTimeRemainig;
    private bool reloading = false;
    public TextMeshProUGUI ammoText;

    public GameObject bloodParticle;

    public AudioSource shotAudio;

    public static short health = 100;
    public TextMeshProUGUI healthText;

    public GameObject Level;
    public GameObject PauseMenu;

    public TextMeshProUGUI time;
    #endregion

    private void OnEnable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        YMouseSens = mouseSens;
        XMouseSens = mouseSens;

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
    }

    void Start()
    {
        ammoRemaining = ammoMax;
        reloadTimeRemainig = reloadTimeMax;
        ammoText.text = ammoRemaining.ToString() + "/" + ammoMax.ToString();

        healthText.text = PlayerController.health.ToString() + " HP"; 
    }

    // Update is called once per frame
    void Update()
    {
        time.text = "Time: " + TimePlayed.timePlayed.ToString("n2");

        //check if player is touching ground
        isGrounded = groundTrigger.isGrounded;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu.SetActive(true);
            Cursor.visible = true;
            Level.SetActive(false);
        }

        //Check for health or F1 to reset
        if (health <= 0 || Input.GetKeyDown(KeyCode.F1))
        {
            TimePlayed.timePlayed = 0F;
            health = 100;
            ammoRemaining = ammoMax;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        #region Movement
        //get movement input
        ZAxisInput = Input.GetAxisRaw("Z Axis");
        XAxisInput = Input.GetAxisRaw("X Axis");

        XMouseInput = Input.GetAxisRaw("Mouse X");
        YMouseInput = Input.GetAxisRaw("Mouse Y");
        
        //horiontal camera
        trPlayer.eulerAngles += new Vector3(0f, XMouseInput * XMouseSens, 0f);

        //verical camera
        //Camera limitataions
        if ((YMouseInput > 0f ^ YMouseInput < 0f) && Mathf.Abs((Mathf.Repeat(trCamera.eulerAngles.x + 180, 360) - 180) + YMouseInput * YMouseSens) <= 90)
        {
            trCamera.eulerAngles += new Vector3(YMouseInput * YMouseSens, 0f, 0f);
        }


        //get player velocity
        ZAxisVelocity = rbPlayer.transform.InverseTransformDirection(rbPlayer.velocity).z;
        XAxisVelocity = rbPlayer.transform.InverseTransformDirection(rbPlayer.velocity).x;
        

        #region MaxSpeed
        //determine max speed player can reach
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

        //check if player is moveing in 2 directions
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
        //maxSpeedCombined = maxSpeed * 2f;
        #endregion

        #region CanSeedUp
        //player can speed up if he didnt reach max velocity
        /*if (((ZAxisInput > 0 && ZAxisVelocity > maxSpeed) || (ZAxisInput < 0 && ZAxisVelocity < maxSpeed * -1)) || (isGrounded && Mathf.Abs(ZAxisVelocity) + Mathf.Abs(XAxisVelocity) > maxSpeedCombined))
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
        }*/
        if (ZAxisInput != 0 && XAxisInput != 0)
        {
            if (Mathf.Abs(ZAxisVelocity) + Mathf.Abs(XAxisVelocity) > maxSpeedCombined)
            {
                ZAxisCanSpeedUp = 0;
                XAxisCanSpeedUp = 0;
            }
            else
            {
                ZAxisCanSpeedUp = 1;
                XAxisCanSpeedUp = 1;
            }
        }
        else
        {
            if (Mathf.Abs(ZAxisVelocity) > maxSpeed)
            {
                ZAxisCanSpeedUp = 0;
            }
            else
            {
                ZAxisCanSpeedUp = 1;
            }

            if (Mathf.Abs(XAxisVelocity) > maxSpeed)
            {
                XAxisCanSpeedUp = 0;
            }
            else
            {
                XAxisCanSpeedUp = 1;
            }
        }
        
        if (XAxisInput != 0 || ZAxisInput != 0)
        {
             velocity = new Vector3(XAxisInput * speed * XAxisCanSpeedUp, 0F, ZAxisInput * speed * ZAxisCanSpeedUp);
        }
        else
        {
            velocity = new Vector3(0F, 0F, 0F);
        }

        if (isGrounded)
        {   
            velocity += new Vector3(rbPlayer.transform.InverseTransformDirection(rbPlayer.velocity).x * -1f * drag, 0F, rbPlayer.transform.InverseTransformDirection(rbPlayer.velocity).z * -1 * drag);
        }
        #endregion

        

        #region Jumping
        //get jump input with buffer
        if (Input.GetKeyDown(KeyCode.Space))
        {
            spaceBufferRemaining = spaceBufffer;
        }
        if (spaceBufferRemaining > 0)
        {
            spaceBufferRemaining -= Time.deltaTime;
        }
        
        //check if player can jump
        if (isGrounded == true && spaceBufferRemaining > 0 && jumpCoolDownRemainig <= 0)
        {
            spaceBufferRemaining = 0;
            jump = true;
        }
        
        //cooldown on jumping
        if (jump == true)
        {
            jumpCoolDownRemainig = jumpCoolDown;
        }
        if (jumpCoolDownRemainig > 0)
        {
            jumpCoolDownRemainig -= Time.deltaTime;
        }
        
        //adding force to player, ending jump
        if (jump == true)
        {
            rbPlayer.AddRelativeForce(new Vector3(0f, 225f, 0f));
            jump = false;
        }
        #endregion
        #endregion

        #region Shooting
        if (Input.GetKeyDown("r") && reloading == false)
        {
            reloading = true;
        }
        if (reloading == true)
        {
            reloadTimeRemainig -= Time.deltaTime;
        }
        if (reloadTimeRemainig <= 0)
        {
            ammoRemaining = ammoMax;
            ammoText.text = ammoRemaining.ToString() + "/" + ammoMax.ToString();
            reloading = false;
            reloadTimeRemainig = reloadTimeMax;
        }

        if (rateOfFire > 0)
        {
            rateOfFireRemainig -= Time.deltaTime;
        }
        if (Input.GetMouseButton(0) && rateOfFireRemainig <= 0 && ammoRemaining > 0 && reloading == false)
        {
            ammoRemaining--;
            ammoText.text = ammoRemaining.ToString() + "/" + ammoMax.ToString();
            rateOfFireRemainig = rateOfFire;
            Instantiate(projectile, trBulletSpawn.position, trBulletSpawn.rotation);
            shotAudio.Play(0);
        }
        #endregion
    }

    private void FixedUpdate()
    {
        rbPlayer.AddRelativeForce(velocity);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            TakeDamage(collision.gameObject.GetComponent<Projectile>().Damage);
            Destroy(collision.gameObject);     
        }
    }

    public void TakeDamage(short damage)
    {
        if (damage != 0)
        {
            health -= damage;
            Instantiate(bloodParticle, transform.position, transform.rotation);
            healthText.text = PlayerController.health.ToString() + " HP";
        } 
    }
}
