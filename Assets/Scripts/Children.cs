using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Children : MonoBehaviour
{
    public NavMeshAgent agent;
    public float distance;
    public float posX;
    public float posZ;
    bool changeLocation;
    Vector3 Rlocation;
    GameObject Player;
    Vector3 posPlayer;
    Vector3 newLocation;
    public bool m_allowedMove;

    private int wanderRadius;

    private GameObject Manager;
    public float m_speed;
    public Material m_caught;






    public float multiplyBy;


    void Start()
    {
        m_allowedMove = true;
        wanderRadius = 100;
        changeLocation = false;
        multiplyBy = 0.2f;
        m_speed = Random.Range(6f, 9.5f);
      
        //palt de navmeshagent voor movement en maakt een nieuwe random locatie aan om naar toe tegaan
        agent = gameObject.GetComponent<NavMeshAgent>();

        //Rlocation = new Vector3(Random.Range(-70, 50), 0.377809f, Random.Range(-50, 60));
        Rlocation = RandomNavSphere(transform.position, wanderRadius, -1);
        agent.SetDestination(Rlocation);

        //vind de player 
        Player = GameObject.Find("Player");
        Manager = GameObject.Find("Main_Camera");
        StartCoroutine("ChangeCoords");

    }

    // Update is called once per frame
    void Update()
    {
        //pakt de player transform
        posPlayer = Player.GetComponent<Transform>().position;

        //loopt naar de nieuwe positie
      //  agent.SetDestination(Rlocation);

        //berekent hoe ver de player van he kind vandaan is 
        distance = Vector3.Distance(transform.position, posPlayer);
        if(distance < 25 && m_allowedMove)
        {
                      
            move();
            Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
            Debug.DrawRay(transform.position, forward, Color.green);
            agent.Stop();
            changeLocation = true;

            if (changeLocation)
            {
                posX = Random.Range(-4, 4);
                posZ = Random.Range(-4, 4);
                Rlocation = RandomNavSphere(transform.position, wanderRadius, -1);
                agent.SetDestination(Rlocation);
                changeLocation = false;
            }



        }
        else
        {
     
            agent.SetDestination(Rlocation);
            agent.Resume();

        }

        // In case the customer falls off the map or spawns on a building
        if (transform.position.y <= -2 || transform.position.y >= 10)
        {
            transform.position = new Vector3(Random.Range(-70, 50), -0.125f, Random.Range(-50, 60));
        }

    }
    // nieuwe positie aanmaken
    void move()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - posPlayer).normalized;
        transform.Translate(0, 0, m_speed * Time.deltaTime * 1);
    }
    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            //gameObject.GetComponent<Renderer>().material.color = Color.black;
            m_allowedMove = false;
            agent.Stop();
            gameObject.GetComponentInChildren<Renderer>().material = new Material(m_caught);
            //Manager.GetComponent<ChildManager>().IncreaseSpeed();
            //send msg to GameManager
        }

        if(coll.gameObject.tag == "Building")
        {
            transform.position = new Vector3(Random.Range(-70, 50), -0.125f, Random.Range(-50, 60));
        }

    }

    IEnumerator ChangeCoords()
    {
        /*
         posX = Random.Range(-4, 4);
         posZ = Random.Range(-4, 4);
         Rlocation = new Vector3(posX, -3.87f, posZ);
         agent.SetDestination(Rlocation);
         */
        Rlocation = RandomNavSphere(transform.position, wanderRadius, -1);
        agent.SetDestination(Rlocation);
        yield return new WaitForSeconds(9);
        
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }



}
