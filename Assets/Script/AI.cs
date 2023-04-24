using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AI : MonoBehaviour
{
    public static AI aiInstance;
    public NavMeshAgent aiController;
    public GameObject Player;
    public float xRange = 10f;
    public float yRange = 5f;
    public float zRange = 10f;
    public float moveInterval = 2f; // time interval to move to a new random position
    public int mass;
    Vector3 tmpposition;
    private void Awake()
    {
        aiInstance = this;
        aiController = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("OriginalPlayer");

        tmpposition = this.transform.position;
        InvokeRepeating("MoveToRandomPosition", 0f, moveInterval);
    }
    void Update()
    {
        float dist = Vector3.Distance(aiController.transform.position, Player.transform.position);
        if ( dist < 70)
        {
            aiController.SetDestination(Player.transform.position);
        }
        else
        {
            aiController.SetDestination(tmpposition);
        }
        
    }
    void MoveToRandomPosition()
    {
        // generate new random positions within the range
        float randomX = Random.Range(-xRange, xRange);
        float randomY = Random.Range(-yRange, yRange);
        float randomZ = Random.Range(-zRange, zRange);

        // move the object to the new random position
        transform.position = new Vector3(randomX, randomY, randomZ) ;
    }
    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.CompareTag("OriginalPlayer"))
        //{
        //    PlayerController CollidedObjectPlayerController = other.GetComponent<PlayerController>();
        //    int userMass = CollidedObjectPlayerController.GetMass();
        //    if (mass < userMass)
        //    {
        //        Destroy(this.gameObject);
        //    }
        //    else if (mass > userMass)
        //    {
        //        // Destroy(other.gameObject);
        //        Time.timeScale = 0;
        //    }



        //}
        //else
        if (other.gameObject.CompareTag("Pickup"))
        {

            mass += 1;
            this.transform.localScale = new Vector3(
                this.transform.localScale.x + .1f,
                this.transform.localScale.y + .1f,
                this.transform.localScale.z + .1f);
            Debug.Log("AI ney kha lea ");
            Destroy(other.gameObject);
            //Eat();
        }
    }

}
