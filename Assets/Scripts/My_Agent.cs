using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class My_Agent : Agent
{
    public CheckpointManager checkpointManager;
    private Car_Move carMove;

    public override void Initialize()
    {
        carMove = GetComponent<Car_Move>();
    }

    public override void OnEpisodeBegin()
    {
        checkpointManager.ResetCheckpoints();
        carMove.Respawn();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 diff = checkpointManager.nextCheckPointToReach.transform.position - transform.position;
        sensor.AddObservation(diff / 20f);
        AddReward(-0.001f);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var Actions = actions.ContinuousActions;

        carMove.ApplyAcceleration(Actions[1]);
        carMove.Steer(Actions[0]);
        carMove.AnimateKart(Actions[0]);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continousActions = actionsOut.ContinuousActions;
        continousActions.Clear();

        continousActions[0] = Input.GetAxis("Horizontal");
        continousActions[1] = Input.GetKey(KeyCode.W) ? 1f : 0f;
    }
}
