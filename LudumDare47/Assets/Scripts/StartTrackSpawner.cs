using Enum;
using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTrackSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        //BuildTestRound();
        // BuildTestRoundRevers();
    }

    private void BuildTestRoundRevers()
    {
        for (int i = 6; i > 0; i--)
        {
            BuildRail(x: i, y: 2);
        }

        BuildRail(x: 1, y: 3);
        BuildRail(x: 1, y: 4);

        for (int i = 1; i <= 6; i++)
        {
            BuildRail(x: i, y: 5);
        }

        BuildRail(x: 6, y: 3);
        BuildRail(x: 6, y: 4);

        BuildStaton(x: 3, y: 1);
    }

    private void BuildTestRound()
    {
        for (int i = 1; i <= 6; i++)
        {
            BuildRail(x: i, y: 2);
        }

        BuildRail(x: 6, y: 3);
        BuildRail(x: 6, y: 4);
        for (int i = 6; i > 0; i--)
        {
            BuildRail(x: i, y: 5);
        }

        BuildRail(x: 1, y: 4);
        BuildRail(x: 1, y: 3);
        BuildStaton(x: 3, y: 1);
    }

    private void BuildRail(int x, int y)
    {
        GameManager.Instance.BuildSomething(x: x, y: y, buildType: WorldTileSpecificationType.Rail, level: 0);
    }

    private void BuildStaton(int x, int y)
    {
        GameManager.Instance.BuildSomething(x: x, y: y, buildType: WorldTileSpecificationType.Station, level: 0);
    }
}