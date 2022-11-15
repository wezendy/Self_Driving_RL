using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCheckpoints : MonoBehaviour
{

    private List<Checkpoint> checkpointList;
    private int nextCheckpointIndex;

    private void Awake()
    {
        Transform checkpointsTransform = transform.Find("Checkpoints");

        checkpointList = new List<Checkpoint>();
        foreach (Transform checkpointTransform in checkpointsTransform)
        {
            Checkpoint checkpoint = checkpointTransform.GetComponent<Checkpoint>();
            checkpoint.SetTrackCheckpoints(this);
            checkpointList.Add(checkpoint);
        }

        nextCheckpointIndex = 0;

    }

    public void PlayerTroughChechkpoint(Checkpoint checkpoint)
    {
        if (checkpointList.IndexOf(checkpoint) == nextCheckpointIndex)
        {
            //jó cp
            nextCheckpointIndex = (nextCheckpointIndex + 1) % checkpointList.Count;
            Debug.Log("jau");
        } else
        {
            // rossz cp
            Debug.Log("szar");
        }
    }

}
