using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleAImotion : MonoBehaviour
{
    public enum MotionTypeT { NA, WaypointPatrol, RandomWalk }

    public MotionTypeT MotionType;

    public Transform[] Waypoints;
    public int CurrentDestinationIndex;

    //For random walk
    public GameObject DestinationMarker;
    public float MaxStepSize;
    public float prevRemaining;

    private NavMeshAgent thisNavMeshAgent;

    void Start()
    {
        thisNavMeshAgent = GetComponent<NavMeshAgent>();

        switch (MotionType)
        {
            case MotionTypeT.WaypointPatrol:
                CurrentDestinationIndex = 0;
                thisNavMeshAgent.SetDestination(Waypoints[0].position);
                //do waypoint stuff
                break;
            case MotionTypeT.RandomWalk:
                //do random walk stuff
                DestinationMarker = new GameObject();
                DestinationMarker.name = "DestinationMarker_for_" + name;

                Vector3 randomStep = MaxStepSize * Random.onUnitSphere;
                DestinationMarker.transform.position = transform.position + randomStep;
                thisNavMeshAgent.SetDestination(DestinationMarker.transform.position);
                break;

        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (MotionType)
        {
            case MotionTypeT.WaypointPatrol:
                WaypointPatrolMotion();
                break;
            case MotionTypeT.RandomWalk:
                RandomWalkMotion();
                break;
        }
    }

    private void WaypointPatrolMotion()
    {
        //if close to destination waypoint, redirect to next waypoint
        if (thisNavMeshAgent.remainingDistance < .05f)
        {
            CurrentDestinationIndex++;
            if (CurrentDestinationIndex == Waypoints.Length)
                CurrentDestinationIndex = 0;

            thisNavMeshAgent.SetDestination(Waypoints[CurrentDestinationIndex].position);
        }

    }

    private void RandomWalkMotion()
    {
        //is AI as close as it can get to the destination?
        //am I stooped ot slow?
        bool asCloseAsPossible = false;
        if (thisNavMeshAgent.velocity.magnitude < .01f)
            asCloseAsPossible = true;

        if (asCloseAsPossible)
        {
            //Debug.Log("Is as close as possible, and finding a new place to go");
            Vector3 randomStep = MaxStepSize * Random.onUnitSphere;
            DestinationMarker.transform.position = transform.position + randomStep;
            thisNavMeshAgent.SetDestination(DestinationMarker.transform.position);
        }
    }

}