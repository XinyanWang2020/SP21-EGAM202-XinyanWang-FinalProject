using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BabyFishBrain : MonoBehaviour

{
    public enum BabyFishStateT
    {
        DecidingWhatToDoNext,
        SeekingSpike, MovingToSpike,
        RunAway,
        Straggling,
        Resting
    }
    public float DistanceToPlayer;
    public GameObject player;
    public float SeekingDistance = 0.5f;
    public float moveSpeed = 1f;

    public float Water = 100, WaterLostPerSecond = 1;
    public float Food = 100, FoodLostPerSecond = 1;
    public float Health = 100;
    public float MaxDispersalDistance;

    public BabyFishStateT currentState;

    NavMeshAgent thisNavMeshAgent;
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
        //water and food levels decrease
        Water -= WaterLostPerSecond * Time.deltaTime;
        Food -= FoodLostPerSecond * Time.deltaTime;

        //check the distance between player
        DistanceToPlayer = Vector3.Distance(player.transform.position, this.transform.position);

        if (Food < 0 || Water < 0 || Health <= 0)
        {
            Destroy(this.gameObject);
        }

        switch(currentState)
        {
            case BabyFishStateT.DecidingWhatToDoNext:
                DecideWhatToDoNext();
                break;

            case BabyFishStateT.RunAway:
                RunAway();
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
    }

    public void RunAway()
    {
        Debug.Log("run");
        transform.position = transform.position + new Vector3(1 * moveSpeed, 0, 0);
        if (DistanceToPlayer > SeekingDistance)
        {
            currentState = BabyFishStateT.DecidingWhatToDoNext;
        }
    }

}
