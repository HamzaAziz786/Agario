using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AI : MonoBehaviour
{
    public static AI aiInstance;
    public Transform aiController;
    public GameObject Player;
    //public Text Scoretext;
    //public Text Masstext;
    public int currrent_enemy_value;
   
    public int mass;
    public int score;
    public GameObject target;
    Vector3 tmpposition;
    private float speed = 5f;
    private void Awake()
    {
        aiInstance = this;
        
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
        target = GameObject.FindWithTag("Pickup");

        //tmpposition = this.transform.position;

    }
    void Update()
    {
        // aiController.SetDestination(Player.transform.position * Time.deltaTime);
        target = GameObject.FindWithTag("Pickup");

        if (target != null)
        {
            // Calculate the direction to the target
            Vector3 direction = target.transform.position - transform.position;
            direction.Normalize();

            // Move towards the target
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }
  
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.CompareTag("Pickup"))
        {
            target = GameObject.FindWithTag("Pickup");
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
            
        }
    }
    public void FindTarget()
    {
        target = GameObject.FindWithTag("Pickup");

        // Check if a game object with the specified tag was found
        if (target != null)
        {
            // Do something with the target game object
            Debug.Log("Target found: " + target.name);
        }
    }
}
