using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public float MaxTimeToReachNextCheckpoint = 30f;
    public float TimeLeft = 30f;

    public My_Agent myAgent;
    public Checkpoint nextCheckPointToReach;

    private int CurrentCheckpointIndex;
    private List<Checkpoint> Checkpoints;
    private Checkpoint lastCheckpoint;

    public event Action<Checkpoint> reachedCheckpoint;

    void Start()
    {
        Checkpoints = FindObjectOfType<Checkpoints>().checkPoints;
        ResetCheckpoints();
    }

    public void ResetCheckpoints()
    {
        CurrentCheckpointIndex = 0;
        TimeLeft = MaxTimeToReachNextCheckpoint;

        SetNextCheckpoint();
    }

    private void Update()
    {
        TimeLeft -= Time.deltaTime;

        if (TimeLeft < 0f)
        {
            myAgent.AddReward(-1f);
            myAgent.EndEpisode();
        }
    }

    public void CheckPointReached(Checkpoint checkpoint)
    {
        if (nextCheckPointToReach != checkpoint) return;

        lastCheckpoint = Checkpoints[CurrentCheckpointIndex];
        reachedCheckpoint?.Invoke(checkpoint);
        CurrentCheckpointIndex++;

        if (CurrentCheckpointIndex >= Checkpoints.Count)
        {
            myAgent.AddReward(1f);
            myAgent.EndEpisode();
        }
        else
        {
            myAgent.AddReward((1f) / Checkpoints.Count);
            SetNextCheckpoint();
        }
    }

    private void SetNextCheckpoint()
    {
        if (Checkpoints.Count > 0)
        {
            TimeLeft = MaxTimeToReachNextCheckpoint;
            nextCheckPointToReach = Checkpoints[CurrentCheckpointIndex];

        }
    }
}
