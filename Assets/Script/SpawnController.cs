using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

// https://answers.unity.com/questions/537688/how-to-randomly-generate-pickups.html

public class SpawnController : MonoBehaviour
{
    //the pickup prefab assigned via the Inspector
    public GameObject pickupPrefab;
    public GameObject VirusPrefabs;
    private GameObject spawnedPickup;
    

    public float spawnTime;
    public float spawnDelay;
  
    private int mapsizeX = 500;
    private int mapsizeZ = 500;

    public int totalNumberOfPickups;
    public int currentNumberOfPickups = 0;
    public List<GameObject> EnemiesList;

    void Start()
    {
        InvokeRepeating("SpawnPickup", spawnTime, spawnDelay);
    }

   public  void SpawnPickup()
    {
        Vector3 randomPostion = GenerateRandomPosition();

        // instantiate (create) the pickup prefab with the above position and rotation
        spawnedPickup = Instantiate(pickupPrefab, randomPostion, transform.rotation);
        currentNumberOfPickups++;
        if (currentNumberOfPickups % 20 == 0)
        {
           Instantiate(VirusPrefabs, randomPostion, transform.rotation);
           
           // spawnedPickup.GetComponent<AI>().mass = Random.Range(25, 100);
        }
        // change color
        ChangeColor(spawnedPickup);

        // set the name of the pickup to it's x, y, z location
        spawnedPickup.name = randomPostion.x.ToString() + '.' + randomPostion.y.ToString()
            + '.' + randomPostion.z.ToString();

        if(currentNumberOfPickups >= totalNumberOfPickups)
        {
            CancelInvoke("SpawnPickup");
            //Debug.Log("CancelInvoke: " + currentNumberOfPickups + ", " +  totalNumberOfPickups);
        }
        if(currentNumberOfPickups < totalNumberOfPickups)
        {
            InvokeRepeating("SpawnPickup", spawnTime, spawnDelay);
            //Debug.Log("InvokeRepeating: " + currentNumberOfPickups + ", " + totalNumberOfPickups);
        }
    }

    Vector3 GenerateRandomPosition()
    {
        int randomPositionX = Random.Range(-mapsizeX + 1, mapsizeX - 1);
        int randomPositionZ = Random.Range(-mapsizeZ + 1, mapsizeZ - 1);

        Vector3 position = new Vector3(randomPositionX, 0, randomPositionZ);
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
}
