using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    public int initMass;
    public int prevMass;
    public int mass;
    public int pickupMass;

    private int totalScore;
    public Text totalScoreText;
    public Text massText;

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
    private const float splitingSpeed = .2f; //  A const variable is one whose value cannot be changed.
    private const float growingSize = 0.2f; // Mass : Scale = 5 : 1
    private const int splitLimit = 0; // *should be double of init mass // minimum splitable mass

    //public Transform followingTarget;
    private float followingSpeed = .2f;
    private float boundary; // boundary for preventing to overlap

    public bool eatableStart;
    public bool eatable;
    private float eatableTime = 1f; // *should be 30sec

    public Queue<GameObject> queue = new Queue<GameObject>();
    public bool IsSplit = true;
    public List<GameObject> playerClonePositions;
    public List<GameObject> PlayerClones;

    public float distanceFromCamera = 10f;
    public float movementSpeed = .3f;
    // Use this for initialization
    // all of the Start() is called on the first frame that the script is active
    void Start()
    {
        spawnControllerInstance = mySpawner.GetComponent<SpawnController>();
        massControllerInstance = myMassController.GetComponent<MassController>();

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

        boundary = transform.localScale.x * 750f;

        eatable = false;
        eatableStart = false;

        prevMass = mass;
    }

    // Update() is called once per frame
    // and it is called before rendering a frame
    void Update()
    {
        bool isSpace = Input.GetKeyDown(KeyCode.Space); // when space key is pushed

        if (isSpace && mass >= splitLimit && IsSplit == true)
        {
            IsSplit = false;

            Split();
           
            StartCoroutine(nameof(ResplitEnable));
        }
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = distanceFromCamera;

        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        targetPosition.y = transform.position.y;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed /** Time.deltaTime*/);
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
               if((g.transform.position - this.transform.position).magnitude > boundary)
                {
                    Vector3 mousePosition = Input.mousePosition;
                    mousePosition.z = distanceFromCamera;

                    Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                    targetPosition.y = transform.position.y;
                    
                    g.transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
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
            totalScore = totalScore + 1;
            SetTotalScoreText();

            Destroy(collider.gameObject);
            spawnControllerInstance.SubtractPickup();

            // run SpawnPickup() for spawning again
            spawnControllerInstance.InvokeRepeating("SpawnPickup", spawnControllerInstance.spawnTime,
                spawnControllerInstance.spawnDelay);

            Eat();
        }
        else if (collider.gameObject.CompareTag("Virus"))
        {
            if (mass >= splitLimit && IsSplit == true)
            {
                IsSplit = false;

                Split();

                StartCoroutine(nameof(ResplitEnable));
            }
        }
        // when player hit User
        else if (collider.gameObject.CompareTag("User") && eatable)
        {
            PlayerController CollidedObjectPlayerController = CollidedObject.GetComponent<PlayerController>();
            int userMass = CollidedObjectPlayerController.GetMass();

            if (mass > userMass)
            {
                //Debug.Log ("mass > userMass: " + mass + " > " + userMass);
                Eat(CollidedObject);
                massControllerInstance.DecreaseMass(userMass);
                Destroy(collider.gameObject);
            }

            // run SpawnPickup() for spawning again
            spawnControllerInstance.InvokeRepeating("SpawnPickup", spawnControllerInstance.spawnTime,
                spawnControllerInstance.spawnDelay);
        }
        else if (collider.gameObject.CompareTag("OriginalPlayer"))
        {
            PlayerController CollidedObjectPlayerController = CollidedObject.GetComponent<PlayerController>();
            int userMass = CollidedObjectPlayerController.GetMass();

            if (mass > userMass)
            {
                Debug.Log("mass > userMass: " + mass + " > " + userMass);
                Eat(CollidedObject);
                Destroy(this.gameObject);
            }

            // run SpawnPickup() for spawning again
            spawnControllerInstance.InvokeRepeating("SpawnPickup", spawnControllerInstance.spawnTime,
                spawnControllerInstance.spawnDelay);
        }
        else if (collider.gameObject.CompareTag("AI"))
        {
            //PlayerController CollidedObjectPlayerController = CollidedObject.GetComponent<PlayerController>();
            int userMass = CollidedObject.GetComponent<AI>().mass;

            if (mass < userMass)
            {
                Debug.Log("mass > userMass: " + mass + " > " + userMass);
                //Eat(CollidedObject);
                Destroy(this.gameObject);
                Time.timeScale = 0;
                SceneManager.LoadScene(0);
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
    IEnumerator ResetVirusPlayerClone()
    {
        yield return new WaitForSeconds(4f);
        for (int i = 0; i < playerClonePositions.Count; i++)
        {
            Destroy(this.transform.GetChild(i));
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

    void Split()
    {
        MakeSizeHalf(); // make player half size first
        prevMass = mass;
        // and instantiate(create) the pickup prefab with the above position and rotation
        if (splittedPickup == null)
        {
            
            splittedPickup = (GameObject)Instantiate(this.gameObject, this.transform.GetChild(0).transform.position, transform.rotation);
            Faster(splittedPickup); // make splitted one faster in short time

            splittedPickup.gameObject.tag = "User";
        }
        else if (splittedPickup != null)
        {
            Destroy(splittedPickup);
            splittedPickup = (GameObject)Instantiate(this.gameObject, transform.localPosition, transform.rotation);
            Faster(splittedPickup); // make splitted one faster in short time

            splittedPickup.gameObject.tag = "User";
        }
    }

    //	// Eject mass of mine
    //	void EjectMass()
    //	{
    //		tempScale = transform.localScale;
    //		tempScale.Set(tempScale.x - 3f, tempScale.y - 3f, tempScale.z - 3f);
    //		transform.localScale = tempScale;
    //
    //		// prevent getting higher too much when it is being smaller
    //		tempPosition = transform.localPosition;
    //		tempPosition.y -= 1f;
    //		transform.localPosition = tempPosition;
    //	}

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
