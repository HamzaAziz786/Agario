using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// https://answers.unity.com/questions/537688/how-to-randomly-generate-pickups.html

public class SpawnController : MonoBehaviour
{
    //the pickup prefab assigned via the Inspector
    public static SpawnController instance;
    public GameObject pickupPrefab;
    public GameObject Ai;
    private GameObject spawnedPickup;
    public GameObject ParentAI;

    public float spawnTime;
    public float spawnDelay;

    private int mapsizeX = 500;
    private int mapsizeZ = 500;

    public int totalNumberOfPickups;
    public int currentNumberOfPickups = 0;
    public List<GameObject> EnemiesList;
    int countenemies = 0;
    public List<Text> ScoreText;
    public List<Text> MassText;
    public List<int> ScoreList;
    void Start()
    {

        if (instance == null)
        {
            instance = this;
            Debug.Log(instance);
        }
        
        InvokeRepeating("SpawnPickup", spawnTime, spawnDelay);
    }
    
    public void SpawnPickup()
    {
        Vector3 randomPostion = GenerateRandomPosition();

        // instantiate (create) the pickup prefab with the above position and rotation
        spawnedPickup = Instantiate(pickupPrefab, randomPostion, transform.rotation);
        currentNumberOfPickups++;
        if (currentNumberOfPickups % 20 == 0 && EnemiesList.Count<10)
        {
            Ai = Instantiate(Ai, randomPostion, transform.rotation,ParentAI.transform);
           
            Ai.gameObject.name = countenemies.ToString();
            Ai.GetComponent<AI>().mass = 0;
            Ai.GetComponent<AI>().currrent_enemy_value = countenemies;
            EnemiesList.Add(Ai);
            if (EnemiesList.Count % 2 == 0)
            {
                Ai.GetComponent<AI>().isAgressive = false;
            }
            else if (EnemiesList.Count % 2 != 0)
            {
                Ai.GetComponent<AI>().isAgressive = true;
            }

            MassText[countenemies].text = "Mass :"+EnemiesList[countenemies].GetComponent<AI>().mass.ToString();
            ScoreText[countenemies].text = "Score :" + EnemiesList[countenemies].GetComponent<AI>().score.ToString();
            countenemies++;
            if (countenemies > 9)
                countenemies = 0;
        }

        for (int i = 0; i < EnemiesList.Count; i++)
        {
            ScoreList[i] = EnemiesList[i].GetComponent<AI>().score;
            
        }
        // change color
        ChangeColor(spawnedPickup);

        // set the name of the pickup to it's x, y, z location
        spawnedPickup.name = randomPostion.x.ToString() + '.' + randomPostion.y.ToString()
            + '.' + randomPostion.z.ToString();

        if (currentNumberOfPickups >= totalNumberOfPickups)
        {
            CancelInvoke("SpawnPickup");
            //Debug.Log("CancelInvoke: " + currentNumberOfPickups + ", " +  totalNumberOfPickups);
        }
        if (currentNumberOfPickups < totalNumberOfPickups)
        {
            InvokeRepeating("SpawnPickup", spawnTime, spawnDelay);
            //Debug.Log("InvokeRepeating: " + currentNumberOfPickups + ", " + totalNumberOfPickups);
        }
       

    }

    Vector3 GenerateRandomPosition()
    {
        int randomPositionX = Random.Range(-mapsizeX + 1, mapsizeX - 1);
        int randomPositionZ = Random.Range(-mapsizeZ + 1, mapsizeZ - 1);

        Vector3 position = new Vector3(randomPositionX, 0.9100037f, randomPositionZ);
        return position;
    }

    public void SubtractPickup()
    {
        currentNumberOfPickups -= 1;
    }

    public void ChangeColor(GameObject target)
    {
        Color RandomColor = Color.HSVToRGB(Random.Range(0f, 1f),
            Random.Range(0f, 1f), Random.Range(0f, 1f));
        target.GetComponent<Renderer>()
            .material.color = RandomColor;
    }
    public void RemoveAndAddAI(GameObject RemoveAI)
    {
        EnemiesList.Remove(RemoveAI);
        Vector3 randomPostion = GenerateRandomPosition();
        Ai = Instantiate(Ai, randomPostion, transform.rotation, ParentAI.transform);
        Ai.gameObject.name = countenemies.ToString();
        Ai.GetComponent<AI>().mass = Random.Range(25, 100);
        Ai.GetComponent<AI>().currrent_enemy_value = countenemies;
        EnemiesList.Add(Ai);

        MassText[countenemies].text = "Mass :" + EnemiesList[countenemies].GetComponent<AI>().mass.ToString();
        ScoreText[countenemies].text = "Score :" + EnemiesList[countenemies].GetComponent<AI>().score.ToString();
        countenemies++;
    }
}
