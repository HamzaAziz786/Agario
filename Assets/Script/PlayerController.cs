using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using System;
using System.Linq;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    public int initMass;
    public int prevMass;
    public int mass;
    public int pickupMass;

    private int totalScore;
    public Text totalScoreText;
    public Text massText;
    public GameObject LoadingPanel;
    public GameObject mySpawner; // GameObejct instance for SpawnController
    private SpawnController spawnControllerInstance; // script instance of SpawnController for connecting SpawnController 
    private GameObject CollidedObject;

    public GameObject myMassController;
    private MassController massControllerInstance;

    // for changing scale
    private Vector3 tempScale;
    private Vector3 tempPosition;

    //for spliting
    public GameObject splittedPickup;
    public GameObject tempplayerclone;
    private const float splitingSpeed = 1f; //  A const variable is one whose value cannot be changed.
    private const float growingSize = 0.05f; // Mass : Scale = 5 : 1
    private const int splitLimit = 6; // *should be double of init mass // minimum splitable mass

    //public Transform followingTarget;
    private float followingSpeed = .02f;
    private float boundary; // boundary for preventing to overlap

    public bool eatableStart;
    public bool eatable;
    private float eatableTime = 50000f; // *should be 30sec

    public Queue<GameObject> queue = new Queue<GameObject>();
    public bool IsSplit = true;


    public float distanceFromCamera = 10f;
    public float movementSpeed = .3f;
    public bool isMove = true;
    public bool isCloneMore = true;
    int indexplayerchild = 0;
    public int[] scores;
    public bool lockaction = false;
    public static int CountSplit=0;
    public ACtion action;
    // Use this for initialization
    // all of the Start() is called on the first frame that the script is active
    void Start()
    {
        spawnControllerInstance = mySpawner.GetComponent<SpawnController>();
        massControllerInstance = myMassController.GetComponent<MassController>();
        action = this.GetComponent<ACtion>();
        initMass = massControllerInstance.initMass;
        pickupMass = massControllerInstance.pickupMass;
        if (prevMass != 0)
        {
            mass = prevMass;
        }
        else
        {
            mass = massControllerInstance.mass;
        }

        SetTotalScoreText();
        SetMassText();

        boundary = transform.localScale.x * 1200f;

        eatable = false;
        eatableStart = false;

        prevMass = mass;
    }

    // Update() is called once per frame
    // and it is called before rendering a frame
    void Update()
    {
        if (lockaction)
        {
            return;
        }
        if (Input.GetKey(KeyCode.W) && isCloneMore == true)
        {
            if (transform.localScale.x >= 1)
            {
                isCloneMore = false;
                GameObject a = Instantiate(this.gameObject, this.transform.position, Quaternion.identity);
                a.transform.Translate(Vector3.forward * 200 * Time.deltaTime);


                a.GetComponent<PlayerController>().isMove = false;
                a.gameObject.tag = "PlayerShootingClone";
            }


            //Rigidbody rigidclone = a.GetComponent<Rigidbody>();
            //rigidclone.AddForce(this.transform.position.x+10, this.transform.position.y, (this.transform.position.z + 20));

        }
        bool isSpace = Input.GetKeyDown(KeyCode.Space); // when space key is pushed
        if (isSpace)
        {
            if (transform.localScale.x <= 2 && CountSplit<3)
            {
                return;
            }
            else
            {
                CountSplit++;
                tempScale = transform.localScale;
                float biggerScaleX = tempScale.x - .5f * growingSize;
                float biggerScaleY = tempScale.y - .5f * growingSize;
                float biggerScaleZ = tempScale.z - .5f * growingSize;

                tempScale.Set(biggerScaleX, biggerScaleY, biggerScaleZ);
                transform.localScale = tempScale;

                action.Split();
            }

        }
        //if (isSpace && mass >= splitLimit && transform.localScale.x >= 1 && IsSplit == true)
        //{
        //    IsSplit = false;

        //    Split();

        //    StartCoroutine(nameof(ResplitEnable));
        //}
        if (isMove == true)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = distanceFromCamera;

            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            targetPosition.y = transform.position.y;

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed / 4 /** Time.deltaTime*/);

        }
        //if ((transform.position - followingTarget.position).magnitude > boundary) { // prevent shittering
        //	transform.LookAt (followingTarget.position);
        //	transform.Translate (0.0f, 0.0f, followingSpeed * Time.deltaTime);

        //}

        followingSpeed = 30.0f * ((float)initMass / (float)mass); // the bigger, the slower 


    }
    IEnumerator ResplitEnable()
    {
        yield return new WaitForSeconds(10f);
        IsSplit = true;

    }
    // FixedUpdate() is called before performing any physics calculations
    void LateUpdate()
    {
        if (queue.Count != 0)
        {
            // if gameObject is in the queue, make it ineatable and get it move with splitting speed
            foreach (GameObject g in queue)
            {
                if ((g.transform.position - this.transform.position).magnitude > boundary)
                {
                    Vector3 mousePosition = Input.mousePosition;
                    mousePosition.z = distanceFromCamera;

                    Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                    targetPosition.y = transform.position.y;

                    g.transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed / 2.2f /** Time.deltaTime*/);
                }

                //if ((g.transform.position - followingTarget.position).magnitude > boundary) { // prevent shittering
                //	g.transform.LookAt (followingTarget.position);
                //	g.transform.Translate (0.0f, 0.0f, splitingSpeed * Time.deltaTime);
                //}
            }
        }

        // execute SetEatable() one time 
        if (gameObject.CompareTag("User") && eatableStart == false)
        {
            StartCoroutine(SetEatable(eatableTime));
        }
    }

    // it is called when our player game object first touches a trigger collider
    void OnTriggerEnter(Collider collider)
    {
        CollidedObject = collider.gameObject;

        // destroy collided object, respawn, eat when player hit Pickup
        if (collider.gameObject.CompareTag("Pickup"))
        {




            scores[0] = SpawnController.instance.ScoreList[0];
            int highestvalue = scores.OrderByDescending(score => score).Max();
            SpawnController.instance.ScoreText[0].text = "Score :" + highestvalue.ToString();
            //for (int i = 1; i < SpawnController.instance.EnemiesList.Count; i++)
            //{
            //    SpawnController.instance.ScoreList[i] = SpawnController.instance.EnemiesList[i].GetComponent<AI>().score;
            //    Debug.Log("Score : " + SpawnController.instance.ScoreList[i]);
            //}
            //for (int i = 1; i < SpawnController.instance.EnemiesList.Count; i++)
            //{
            //    SpawnController.instance.ScoreText[i].text = "Score :" + SpawnController.instance.ScoreList[i].ToString();
            //}


            totalScore = totalScore + 1;
            SetTotalScoreText();

            Destroy(collider.gameObject);
            spawnControllerInstance.SubtractPickup();

            // run SpawnPickup() for spawning again
            spawnControllerInstance.InvokeRepeating("SpawnPickup", spawnControllerInstance.spawnTime,
                spawnControllerInstance.spawnDelay);

            Eat();
        }
        else if (collider.gameObject.tag == "PlayerShootingClone")
        {
            Destroy(collider.gameObject);

            tempScale = transform.localScale;
            float biggerScaleX = tempScale.x + growingSize;
            float biggerScaleY = tempScale.y + growingSize;
            float biggerScaleZ = tempScale.z + growingSize;

            tempScale.Set(biggerScaleX, biggerScaleY, biggerScaleZ);
            transform.localScale = tempScale;
            this.transform.position = new Vector3(this.transform.position.x, 0, this.transform.position.z);
            isCloneMore = true;
        }
        else if (collider.gameObject.CompareTag("Virus"))
        {
            if (mass >= splitLimit && IsSplit == true)
            {
                IsSplit = false;

                //Split();
                action.Split();
                action.Split();
                action.Split();
                action.Split();
                StartCoroutine(nameof(ResplitEnable));
            }
        }
        // when player hit User
        else if (collider.gameObject.CompareTag("User") /*&& eatable*/)
        {
            PlayerController CollidedObjectPlayerController = CollidedObject.GetComponent<PlayerController>();
            int userMass = CollidedObjectPlayerController.GetMass();
            tempScale = transform.localScale;
            float biggerScaleX = tempScale.x + userMass * growingSize;
            float biggerScaleY = tempScale.y + userMass * growingSize;
            float biggerScaleZ = tempScale.z + userMass * growingSize;

            tempScale.Set(biggerScaleX, biggerScaleY, biggerScaleZ);
            transform.localScale = tempScale;

            Eat(CollidedObject);

            Destroy(collider.gameObject);


            // run SpawnPickup() for spawning again
            spawnControllerInstance.InvokeRepeating("SpawnPickup", spawnControllerInstance.spawnTime,
                spawnControllerInstance.spawnDelay);
        }
        //SplitClone
        else if (collider.gameObject.CompareTag("SplitClone"))
        {
            tempScale = transform.localScale;
            float biggerScaleX = tempScale.x + .5f;
            float biggerScaleY = tempScale.y + .5f;
            float biggerScaleZ = tempScale.z + .5f;

            tempScale.Set(biggerScaleX, biggerScaleY, biggerScaleZ);
            transform.localScale = tempScale;
            Destroy(collider.gameObject);
        }

        //else if (collider.gameObject.CompareTag("OriginalPlayer"))
        //{
        //    .//Destroy(collider.gameObject);
        //    //PlayerController CollidedObjectPlayerController = CollidedObject.GetComponent<PlayerController>();
        //    //int userMass = CollidedObjectPlayerController.GetMass();

        //    //if (mass > userMass)
        //    //{
        //    //    Debug.Log("mass > userMass: " + mass + " > " + userMass);
        //    //    Eat(CollidedObject);
        //    //    Destroy(this.gameObject);
        //    //}

        //    //// run SpawnPickup() for spawning again
        //    //spawnControllerInstance.InvokeRepeating("SpawnPickup", spawnControllerInstance.spawnTime,
        //    //    spawnControllerInstance.spawnDelay);
        //}
        else if (collider.gameObject.CompareTag("AI"))
        {
            //PlayerController CollidedObjectPlayerController = CollidedObject.GetComponent<PlayerController>();
            int userMass = CollidedObject.GetComponent<AI>().mass;

            if (mass < userMass)
            {
                Debug.Log("mass > userMass: " + mass + " > " + userMass);
                //Eat(CollidedObject);
                //Destroy(this.gameObject);

                SceneManager.LoadScene(0);
                //Time.timeScale = 1;
            }
            else
            {
                Destroy(collider.gameObject);
            }

            // run SpawnPickup() for spawning again
            spawnControllerInstance.InvokeRepeating("SpawnPickup", spawnControllerInstance.spawnTime,
                spawnControllerInstance.spawnDelay);
        }

    }
   
    void SetTotalScoreText()
    {
        totalScoreText.text = "Score: " + totalScore.ToString();
    }

    void SetMassText()
    {
        massText.text = "Mass: " + mass.ToString();
    }

    // change mass and scale when it eat Pickup
    void Eat()
    {
        prevMass = mass; // save mass before add
        mass = mass + pickupMass;
        SetMassText();
        massControllerInstance.IncreaseMass(pickupMass);

        tempScale = transform.localScale;
        float biggerScaleX = tempScale.x + growingSize;
        float biggerScaleY = tempScale.y + growingSize;
        float biggerScaleZ = tempScale.z + growingSize;

        tempScale.Set(biggerScaleX, biggerScaleY, biggerScaleZ);
        transform.localScale = tempScale;

        // prevent getting deeper when it is being bigger
        tempPosition = transform.localPosition;
        tempPosition.y = biggerScaleY / 2;
        transform.localPosition = tempPosition;
    }

    // change mass and scale when it eat User
    void Eat(GameObject user)
    {
        PlayerController CollidedObjectPlayerController = user.GetComponent<PlayerController>();
        int userMass = CollidedObjectPlayerController.GetMass();

        Debug.Log("ate a User!!" + " / " + "userMass: " + mass + " + " + userMass);
        prevMass = mass;// save mass before add
        mass = mass + userMass;
        Debug.Log("= " + mass);
        SetMassText();
        Debug.Log("userMass" + userMass);

        tempScale = transform.localScale;
        float biggerScaleX = tempScale.x + userMass * growingSize;
        float biggerScaleY = tempScale.y + userMass * growingSize;
        float biggerScaleZ = tempScale.z + userMass * growingSize;

        tempScale.Set(biggerScaleX, biggerScaleY, biggerScaleZ);
        transform.localScale = tempScale;

        // prevent getting deeper when it is being bigger
        tempPosition = transform.localPosition;
        tempPosition.y = biggerScaleY / 2;
        transform.localPosition = tempPosition;
    }

    // make player smaller when it divide itself
    void MakeSizeHalf()
    {
        mass = mass / 2;
        SetMassText();

        tempScale = transform.localScale;
        tempScale.Set(tempScale.x / 2, tempScale.y / 2, tempScale.z / 2);
        transform.localScale = tempScale;

        // prevent getting higher too much when it is being smaller
        tempPosition = transform.localPosition;
        tempPosition.y = tempScale.y / 2;
        transform.localPosition = tempPosition;
    }
    void MakeDoubleSize()
    {
        mass = mass * 2;
        SetMassText();

        tempScale = transform.localScale;
        tempScale.Set(tempScale.x * 2, tempScale.y * 2, tempScale.z * 2);
        transform.localScale = tempScale;

        // prevent getting higher too much when it is being smaller
        tempPosition = transform.localPosition;
        tempPosition.y = tempScale.y * 2;
        transform.localPosition = tempPosition;
    }

    void Split()
    {
        MakeSizeHalf(); // make player half size first
        prevMass = mass;
        // and instantiate(create) the pickup prefab with the above position and rotation
        if (splittedPickup == null)
        {

            splittedPickup = (GameObject)Instantiate(this.gameObject, this.transform.GetChild(indexplayerchild).transform.position, transform.rotation);
            Faster(splittedPickup); // make splitted one faster in short time
            indexplayerchild++;
            if (indexplayerchild > 6)
            {
                indexplayerchild = 0;
            }
            splittedPickup.gameObject.tag = "User";
        }
        else if (splittedPickup != null)
        {
            Destroy(splittedPickup);
            splittedPickup = (GameObject)Instantiate(this.gameObject, this.transform.GetChild(indexplayerchild).transform.position, transform.rotation);
            Faster(splittedPickup); // make splitted one faster in short time
            indexplayerchild++;
            if (indexplayerchild > 6)
            {
                indexplayerchild = 0;
            }
            splittedPickup.gameObject.tag = "User";
        }
    }

    // Eject mass of mine
    void EjectMass()
    {
        tempScale = transform.localScale;
        tempScale.Set(tempScale.x - 1f, tempScale.y - 1f, tempScale.z - 1f);
        transform.localScale = tempScale;

        // prevent getting higher too much when it is being smaller
        tempPosition = transform.localPosition;
        tempPosition.y -= 1f;
        transform.localPosition = tempPosition;
    }

    IEnumerator DequeueAllAfterWait(float time)
    {
        //Debug.Log (queue.Count);
        yield return new WaitForSeconds(time);

        if (queue.Count > 1)
        {
            foreach (GameObject g in queue)
            {
                queue.Dequeue();
            }
            //Debug.Log (queue.Count);
        }
        else if (queue.Count == 1)
        {
            queue.Dequeue();
        }

        else
        {
            Debug.Log("Nothing in queue");
        }


    }

    // make divided one eatable adding properties of Rigidbody and isKinematic after some time later.
    IEnumerator SetEatable(float t)
    {
        eatableStart = true; // for executing one time 

        yield return new WaitForSeconds(t);
        eatable = true; // make eatable true after that t time later 
        Debug.Log("SetEatable!!");

        // if it doesn't have rigidbody, make it
        if (!gameObject.GetComponent<Rigidbody>())
        {
            Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();
            rigidbody.isKinematic = true;
        }
        else // if it has rigidbody
        {
            Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
            rigidbody.isKinematic = true;
        }
    }

    public int GetMass()
    {
        return mass;
    }

    void Faster(GameObject g)
    {
        queue.Enqueue(g);
        StartCoroutine(DequeueAllAfterWait(.2f));

    }

}
