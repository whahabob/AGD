using UnityEngine;
using System.Collections;

public class AIStalker : MonoBehaviour
{
    private Vector3 player;
    private Vector3 playerDirection;
    private Vector3 lastSighted;
    private Vector3 sightDirection;
    private float xDif;
    private float zDif;
    public float speed;
    private float defaultSpeed;
    private int wall;
    private UnityEngine.AI.NavMeshAgent agent;

    public float wanderRadius;
    public float wanderTimer;
    private float timer;


    public float maxPatience;
    public float patience;

    Rigidbody myRigidbody;
    public GameObject patienceBar;
    public GameObject patienceIndicator;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.velocity = new Vector3(1, 0, 0);
        wall = 1 << 8;
        patience = maxPatience;
        defaultSpeed = speed;

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //Random rnd = new Random();
        player = GameObject.FindGameObjectWithTag("Player").transform.position;
        xDif = player.x - transform.position.x;
        zDif = player.z - transform.position.z;
        playerDirection = new Vector3(xDif, 0, zDif);
        xDif = lastSighted.x - transform.position.x;
        zDif = lastSighted.z - transform.position.z;
        sightDirection = new Vector3(xDif, 0, zDif);
    }

    void FixedUpdate()
    {
        if (!Physics.Raycast(transform.position, playerDirection, 3, wall))
        {
            lastSighted = player;
        }
        if (!Physics.Raycast(transform.position, sightDirection, 3, wall))
        {
            agent.SetDestination(lastSighted);
        }
        else {
            Wander();
        }

    }
    void Wander()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }

    public Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        UnityEngine.AI.NavMeshHit navHit;
        UnityEngine.AI.NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }

    public void changePatience(int patienceToLose)
    {

        patience += patienceToLose;

        if (patience > maxPatience)
            patience = maxPatience;
        else if (patience <= 0)
            Destroy(gameObject);
    }


    public void Slow(bool relative, float percentage, float time)
    {
        if (relative)
            speed *= percentage;
        else
            speed = defaultSpeed * percentage;

        Invoke("RestoreSpeed", time);
    }

    public void RestoreSpeed()
    {
        speed = defaultSpeed;
    }
}