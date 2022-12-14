using UnityEngine;
// Note this line, if it is left out, the script won't know that the class 'Path' exists and it will throw compiler errors
// This line should always be present at the top of scripts which use pathfinding
using Pathfinding;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

[HelpURL("http://arongranberg.com/astar/docs/class_partial1_1_1_astar_a_i.php")]
public class EnemyRunnerAI : MonoBehaviour
{
    public Transform targetPosition;

    private Seeker seeker;
    private CharacterController controller;

    public Path path;

    public float speedDefault = 6;
    public float speed = 6;

    public float nextWaypointDistance = 3;

    private int currentWaypoint = 0;

    public bool reachedEndOfPath;

    public float searchPathDelayMax;
    private float searchPathDelay;

    public float agroDistance;

    Vector3 velocity;

    public Collider cl;
    public short health = 20;

    public GameObject bloodParticle;
    public GameObject bloodParticleDeath;

    public GameObject sprite;

    public void Start()
    {
        speedDefault = speedDefault * Random.Range(0.8F, 1.2F);
        speed = speedDefault;

        seeker = GetComponent<Seeker>();
        // If you are writing a 2D game you should remove this line
        // and use the alternative way to move sugggested further below.
        controller = GetComponent<CharacterController>();

        // Start a new path to the targetPosition, call the the OnPathComplete function
        // when the path has been calculated (which may take a few frames depending on the complexity)
        //seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
    }

    public void OnPathComplete(Path p)
    {
        //Debug.Log("A path was calculated. Did it fail with an error? " + p.error);

        if (!p.error)
        {
            path = p;
            // Reset the waypoint counter so that we start to move towards the first point in the path
            currentWaypoint = 0;
        }
    }

    public void Update()
    {
        if (health <= 0)
        {
            Instantiate(bloodParticleDeath, sprite.transform.position, transform.rotation);
            ScreenCapture.CaptureScreenshot("Assets/pauseMenuScreenShoot.png");
            Destroy(this.gameObject);
        }


        searchPathDelay += Time.deltaTime;
        if (searchPathDelay > searchPathDelayMax && Vector3.Distance(seeker.transform.position, targetPosition.position) < agroDistance)
        {
            seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
            searchPathDelay = 0;
        }

        if (path == null)
        {
            // We have no path to follow yet, so don't do anything
            return;
        }
        
        // Check in a loop if we are close enough to the current waypoint to switch to the next one.
        // We do this in a loop because many waypoints might be close to each other and we may reach
        // several of them in the same frame.
        reachedEndOfPath = false;
        // The distance to the next waypoint in the path
        float distanceToWaypoint;
        while (true)
        {
            // If you want maximum performance you can check the squared distance instead to get rid of a
            // square root calculation. But that is outside the scope of this tutorial.
            distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if (distanceToWaypoint < nextWaypointDistance)
            {
                // Check if there is another waypoint or if we have reached the end of the path
                if (currentWaypoint + 1 < path.vectorPath.Count)
                {
                    currentWaypoint++;
                }
                else
                {
                    // Set a status variable to indicate that the agent has reached the end of the path.
                    // You can use this to trigger some special code if your game requires that.
                    reachedEndOfPath = true;
                    break;
                }
            }
            else
            {
                break;
            }
        }

        // Slow down smoothly upon approaching the end of the path
        // This value will smoothly go from 1 to 0 as the agent approaches the last waypoint in the path.
        var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint / nextWaypointDistance) : 1f;

        // Direction to the next waypoint
        // Normalize it so that it has a length of 1 world unit
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        // Multiply the direction by our desired enemyAiScript to get a velocity
        velocity = dir * speed * speedFactor;


        // Move the agent using the CharacterController component
        // Note that SimpleMove takes a velocity in meters/second, so we should not multiply by Time.deltaTime
        controller.SimpleMove(velocity);
        transform.LookAt(targetPosition);

        // If you are writing a 2D game you should remove the CharacterController code above and instead move the transform directly by uncommenting the next line
        // transform.position += velocity * Time.deltaTime;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            health -= collision.gameObject.GetComponent<Projectile>().Damage;
            if (collision.gameObject.GetComponent<Projectile>().Damage != 0)
            {
                Instantiate(bloodParticle, sprite.transform.position, transform.rotation);
            }
            Destroy(collision.gameObject);
        }
    }
}