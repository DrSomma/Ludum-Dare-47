using Enum;
using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTrackSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        BuildTestRound();
        SpawnTrain();
    }

    private void SpawnTrain()
    {
        //First Tile
        //train.StartTrain(3, 2);
    }

    private void BuildTestRound()
    {
        for (int i = 1; i <= 6; i++)
        {
            BuildRail(i, 2);

        }
        BuildRail(6, 3);
        BuildRail(6, 4);
        for (int i = 6; i > 0; i--)
        {
            BuildRail(i, 5);
        }
        BuildRail(1, 4);
        BuildRail(1, 3);
    }

    private void BuildRail(int x, int y)
    {
        GameManager.Instance.BuildSomething(x, y,WorldTileSpecificationType.Rail);
    }
}
