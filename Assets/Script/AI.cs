using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AI : MonoBehaviour
{
   
    public GameObject Player;
   
    public int currrent_enemy_value;
   
    public int mass;
    public int score;
    
    
    public string targetTag = "Pickup";
    private float speed = 5f;
    private GameObject nearestTarget;


    #region PlayerDetection
    public float PlayerFacespeed = 5f;
    public float detectionRange = 1000;
    public GameObject targetObject; 
    #endregion

    private void Start()
    {
        try
        {
            //FindNearestTarget();
            Player = GameObject.FindGameObjectWithTag("OriginalPlayer");
        }
        catch (System.Exception)
        {

            throw;
        }
       

        
        
    }
    void Update()
    {
       
        Vector3 Playerdirection = Player.transform.position - transform.position;
        float distanceToTarget = Playerdirection.magnitude;
       
        if (distanceToTarget <= detectionRange)
        {

            // Move in the opposite direction
            if (Player.transform.localScale.x > this.transform.localScale.x)
            {
                transform.Translate(-Playerdirection.normalized * speed * Time.deltaTime * 20);
            }
            else
            {
                transform.Translate(Playerdirection.normalized * speed * Time.deltaTime * 20);
            }

        }
        else
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

    public void FindPlayerDetection()
    {
        Vector3 direction = Player.transform.position - transform.position;
        float distanceToTarget = direction.magnitude;

        if (distanceToTarget <= detectionRange)
        {
            // Move in the opposite direction
            transform.Translate(-direction.normalized * speed * Time.deltaTime);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
       
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
            
        }
    }
    
}
