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

    public float Water = 100, WaterLostPerSecond = 1;
    public float Food = 100, FoodLostPerSecond = 1;
    public float Health = 100;
    public float MaxDispersalDistance;

    public BabyFishStateT currentState;

    NavMeshAgent thisNavMeshAgent;
    void Start()
    {
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

        if (Food < 0 || Water < 0 || Health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
public void TakeDamage (float damage)
    {

    }
}
