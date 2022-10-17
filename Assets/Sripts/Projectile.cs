using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody rb;
    public float power = 20F;

    public float timeAlive;
    private float timeCount = 0F;

    // Start is called before the first frame update
    void Start()
    {
        rb.AddRelativeForce(new Vector3(0F,0F,power));
    }

    // Update is called once per frame
    void Update()
    {
        timeCount += Time.deltaTime;
        
        if(timeCount > timeAlive)
        {
            Destroy(this.gameObject);
        }

        if (rb.velocity.x == 0 && rb.velocity.y == 0 && rb.velocity.z == 0)
        {
            rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
        }
    }
}
