using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AI : MonoBehaviour
{
    public static AI aiInstance;
    public NavMeshAgent aiController;
    public GameObject Player;
    //public Text Scoretext;
    //public Text Masstext;
    public int currrent_enemy_value;
    public float xRange = 10f;
    public float yRange = 5f;
    public float zRange = 10f;
    public float moveInterval = 2f; // time interval to move to a new random position
    public int mass;
    public int score;
    Vector3 tmpposition;
    private void Awake()
    {
        aiInstance = this;
        aiController = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        try
        {
            Player = GameObject.FindGameObjectWithTag("OriginalPlayer");
        }
        catch (System.Exception)
        {

            throw;
        }
        
        //Masstext.text = "Enemy Mass : " +mass.ToString();
        //Scoretext.text = "Enemy Score : " + score.ToString();
        tmpposition = this.transform.position;
        //InvokeRepeating("MoveToRandomPosition", 0f, moveInterval);
    }
    void Update()
    {
        aiController.SetDestination(Player.transform.position * Time.deltaTime);
        //float dist = Vector3.Distance(aiController.transform.position, Player.transform.position);
        //if ( dist < 90)
        //{
        //    aiController.SetDestination(Player.transform.position);
        //}
        //else if(dist > 95)
        //{
        //    aiController.SetDestination(tmpposition);
        //}
        
    }
    void MoveToRandomPosition()
    {
        // generate new random positions within the range
        float randomX = Random.Range(this.transform.position.x-xRange, this.transform.position.x + xRange);
        float randomY = Random.Range(this.transform.position.y - yRange, this.transform.position.y+yRange);
        float randomZ = Random.Range(this.transform.position.z - zRange, this.transform.position.z+zRange);

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
            score += 1;
            SpawnController.instance.ScoreText[currrent_enemy_value].text = "Score:"+score.ToString();
            SpawnController.instance.MassText[currrent_enemy_value].text = "Mass:"+mass.ToString();
            //Masstext.text = "Enemy Mass : " + mass.ToString();
            //Scoretext.text = "Enemy Score : " + score.ToString();

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
