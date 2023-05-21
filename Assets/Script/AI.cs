using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class AI : MonoBehaviour
{

    public GameObject Player;

    public int currrent_enemy_value;

    public int mass;
    public int score;


    public string targetTag = "Pickup";
    public string PlayerTag = "OriginalPlayer";
    public string AiTag = "AI";

    private float speed = 5f;
    private GameObject nearestTarget;
    private GameObject nearestAITarget;

    public bool isAgressive;
    private float initialYPosition;

    private void Start()
    {
        try
        {
            FindNearestTarget();
            Player = GameObject.FindGameObjectWithTag("OriginalPlayer");
        }
        catch (System.Exception)
        {

            throw;
        }

        initialYPosition = transform.position.y;


    }
    void Update()
    {
        float PlayerDisctance = Vector3.Distance(transform.position, Player.transform.position);
        Vector3 PlayerDirection = Player.transform.position - transform.position;
        PlayerDirection.Normalize();
        if (PlayerDisctance < 70)
        {
            if(isAgressive)
            {
                if (Player.transform.localScale.x < this.transform.localScale.x)
                {
                    transform.Translate(PlayerDirection * speed * Time.deltaTime);
                }
                else
                {
                    transform.Translate(-PlayerDirection * speed * Time.deltaTime);
                }
            }
            else if (!isAgressive)
            {
               
                    transform.Translate(-PlayerDirection * speed * Time.deltaTime);
                
            }

        }
        else if (PlayerDisctance >= 150)
        {
            FindNearestTarget();
            if (nearestTarget != null)
            {
                // Calculate the direction to the nearest target
                Vector3 direction = nearestTarget.transform.position - transform.position;
                direction.Normalize();

                // Move towards the nearest target
                transform.Translate(direction * speed * Time.deltaTime);
            }
           
        }

        this.transform.position = new Vector3(this.transform.position.x,
            initialYPosition,this.transform.position.z);
    }
    private void FindNearestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
       

        float nearestDistance = 100f/*Mathf.Infinity*/;

        foreach (GameObject target in targets)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);


            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestTarget = target;
            }

        }
       
    }
  

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Pickup"))
        {

            mass += 1;
            score += 1;
            SpawnController.instance.ScoreText[currrent_enemy_value].text = "Score:" + score.ToString();
            SpawnController.instance.MassText[currrent_enemy_value].text = "Mass:" + mass.ToString();
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
   

}
