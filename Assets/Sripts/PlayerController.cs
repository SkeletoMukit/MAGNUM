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
    public int ammoMax;
    private static int ammoRemaining;
    public float reloadTimeMax = 0.8F;
    private float reloadTimeRemainig;
    private bool reloading = false;
    public TextMeshProUGUI ammoText;

    public GameObject bloodParticle;

    public AudioSource shotAudio;

    public static short health = 100;
    public TextMeshProUGUI healthText;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        ammoRemaining = ammoMax;
        reloadTimeRemainig = reloadTimeMax;
        ammoText.text = ammoRemaining.ToString() + "/" + ammoMax.ToString();

        healthText.text = PlayerController.health.ToString() + " HP";
        //Application.targetFrameRate = 144;
    }

    // Update is called once per frame
    void Update()
    {
        //Check for health
        if (health <= 0)
        {
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
        //determine max enemyAiScript player can reach
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
        #endregion

        #region CanSeedUp
        //player can enemyAiScript up if he didnt reach max velocity
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
        #endregion
        
        //check if player is touching ground
        isGrounded = groundTrigger.isGrounded;

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
            rbPlayer.AddRelativeForce(new Vector3(0f, 300f, 0f));
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
        if (XAxisInput != 0 || ZAxisInput != 0)
        {
            rbPlayer.AddRelativeForce(new Vector3(XAxisInput * speed * XAxisCanSpeedUp, 0F, ZAxisInput * speed * ZAxisCanSpeedUp));
        }
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
