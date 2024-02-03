using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class AgentController : Agent
{

    [SerializeField] private Transform target;

    // episode start state parameters
    public override void OnEpisodeBegin()
    {
        // Agent spawn position
        transform.localPosition = new Vector3(0f, 0.3f, 0f);

        // Pellet spawn position
        int rand = Random.Range(0,2);
        if (rand == 0)
        {
            target.localPosition = new Vector3(-3f, 0.3f, 0f);
        }
        if (rand == 1)
        {
            target.localPosition = new Vector3(3f, 0.3f, 0f);
        }
    }

    // positions retrieval
    public override void CollectObservations(VectorSensor sensor){
        sensor.AddObservation(transform.localPosition); // retrieve the agent's position 
        sensor.AddObservation(target.localPosition); // retrieve the target's position
    }


    // actions
    public override void OnActionReceived(ActionBuffers actions) // function that allows the agent to move 
    {
        // parameters
        float move = actions.ContinuousActions[0]; 
        float moveSpeed = 2f;    

        // movement 
        transform.localPosition += new Vector3(move, 0f) * Time.deltaTime * moveSpeed; 
    }

    // manual action trigger
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
    }

    // reward (for ML)
    private void OnTriggerEnter(Collider other)
    {
        // Pellet reward
        if(other.gameObject.tag == "Pellet")
        {
            AddReward(2f); //MLAgent package 
            EndEpisode();
        }

        // Wall reward
        if(other.gameObject.tag == "Wall")
        {
            AddReward(-0.5f); 
            EndEpisode();
        }
    }
}
