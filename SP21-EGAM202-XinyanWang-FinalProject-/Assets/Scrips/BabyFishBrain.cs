using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BabyFishBrain : MonoBehaviour

{
    public Animator anim;
    public enum BabyFishStateT
    {
        DecidingWhatToDoNext,
        SeekingSpikes, MovingToSpikes,
        RunAway,
        Struggling,
        Resting
    }
    public float DistanceToPlayer;
    public GameObject player;
    public float SeekingDistance = 0.5f;
    public float moveSpeed = 0.5f;

    public float Water = 100, WaterLostPerSecond = 1;
    public float Food = 100, FoodLostPerSecond = 1;
    public float CurrentHealth;
    public float MaxDispersalDistance;

    public BabyFishStateT currentState;

    NavMeshAgent thisNavMeshAgent;

    public Text GameEndText;
    void Start()
    {
        
        player = GameObject.FindWithTag("Player");

        float terrinHeight = GameObject.Find("Terrain").GetComponent<Terrain>().SampleHeight(transform.position);

        thisNavMeshAgent = GetComponent<NavMeshAgent>();

        currentState = BabyFishStateT.DecidingWhatToDoNext;

        transform.position = new Vector3(transform.position.x, terrinHeight, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        CurrentHealth = GetComponentInChildren<HealthBar>().Hp;
        //water and food levels decrease
        Water -= WaterLostPerSecond * Time.deltaTime;
        Food -= FoodLostPerSecond * Time.deltaTime;

        //check the distance between player
        DistanceToPlayer = Vector3.Distance(player.transform.position, this.transform.position);

        if (Food < 0 || Water < 0 || CurrentHealth <= 0)
        {

            Destroy(this.gameObject,3f);
            SceneManager.LoadScene("Main");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        //if it is weak
        if(Food < 20 || Water <20)
        {
            moveSpeed = 0.3f;
            anim.SetTrigger("Weak");
        }
        if (Food > 20 && Water > 20)
        {
            moveSpeed = 0.5f;
            anim.SetTrigger("Normal");
            anim.ResetTrigger("Weak");
        }

        switch(currentState)
        {
            case BabyFishStateT.DecidingWhatToDoNext:
                DecideWhatToDoNext();
                break;

            case BabyFishStateT.RunAway:
                RunAway();
                break;

            //case BabyFishStateT.Struggling:
                //Struggling();
                //break;
                case BabyFishStateT.SeekingSpikes:
                SeekingSpikes();
                break;

                case BabyFishStateT.MovingToSpikes:
                MovingToSpikes();
                break;
        }
    }
    public void DecideWhatToDoNext()
    {
        if (DistanceToPlayer < SeekingDistance)
        {
            currentState = BabyFishStateT.RunAway;
            return;
        }

        if (DistanceToPlayer > SeekingDistance)
        {
            currentState = BabyFishStateT.SeekingSpikes;
            return;
        }

    }

    public void RunAway()
    {
        Vector3 dir = player .transform.position - transform.position;
        //Debug.Log("run");
        // if the player is on the back
        if (player.transform.position.z < transform.position.z)
        {
            Debug.Log("1");
            transform.position = transform.position + new Vector3(0,0.5f * moveSpeed,1);
        }

        // if the player is on the front
        if (player.transform.position.z > transform .position .z)
        {
            Debug.Log("2");
            transform.position = transform.position + new Vector3(0, 0.5f * moveSpeed, -1);
        }
        //if the player is on the left
        if (player.transform.position.x < transform.position.x)
        {
            Debug.Log("3");
            transform.position = transform.position + new Vector3(1f * moveSpeed, 0, 0);
        }
        //if the player is on the right
        if (player.transform.position.x > transform.position.x)
        {
            Debug.Log("4");
            transform.position = transform.position + new Vector3(-1f * moveSpeed, 0, 0);
        }

        if (DistanceToPlayer > SeekingDistance)
        {
            currentState = BabyFishStateT.DecidingWhatToDoNext;
        }
    }

    public void SeekingSpikes()
    {
        Debug.Log("seeking spikes");
        GameObject targetSpikes = FindClosestObjectWithTag("Spikes");
        thisNavMeshAgent.SetDestination(targetSpikes.transform.position);
        currentState = BabyFishStateT.MovingToSpikes; 
        if (DistanceToPlayer < SeekingDistance)
        {
            currentState = BabyFishStateT.RunAway;
        }
    }

    public void MovingToSpikes()
    {
        Debug.Log("moving to spikes");
        if (DistanceToPlayer < SeekingDistance)
        {
            currentState = BabyFishStateT.RunAway;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            currentState = BabyFishStateT.Struggling;
            Debug.Log("struggling");
            GameObject babyfish = GameObject.Find("BabyFish");
            babyfish.gameObject.transform.SetParent(other.transform);
            babyfish.gameObject.transform.localPosition = new Vector3(0, 25, 1);
            StartCoroutine(Count());
        }

        if (currentState == BabyFishStateT.MovingToSpikes && other.gameObject.tag == "Spikes")
        {
            currentState = BabyFishStateT.DecidingWhatToDoNext;
        }
    }

    IEnumerator Count()
    {
        GameObject babyfish = GameObject.Find("BabyFish");
        yield return new WaitForSecondsRealtime(3);
        babyfish.gameObject.transform.SetParent(null);
        yield return new WaitForSecondsRealtime(1);
        currentState = BabyFishStateT.DecidingWhatToDoNext;
    }


    public GameObject FindClosestObjectWithTag(string tag)
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);

        if (objectsWithTag.Length == 0)
            return null;

        GameObject closestObject = objectsWithTag[0];
        float distanceToClosestObject = 1e6f,
            distanceToCurrentObject;
        for (int i = 0; i < objectsWithTag.Length; i++)
        {
            Vector3 vectorToCurrentObject;
            GameObject currentObject;
            currentObject = objectsWithTag[i];
            vectorToCurrentObject = currentObject.transform.position - transform.position;
            distanceToCurrentObject = vectorToCurrentObject.magnitude;
            if (distanceToCurrentObject < distanceToClosestObject)
            {
                closestObject = objectsWithTag[i];
                distanceToClosestObject = distanceToCurrentObject;
            }
        }
        return closestObject;

    }

}
