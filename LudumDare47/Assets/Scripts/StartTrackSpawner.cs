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
        //BuildTestRound();
        BuildTestRoundRevers();
    }

    private void BuildTestRoundRevers()
    {
        for (int i = 6; i > 0; i--)
        {
            BuildRail(i, 2);

        }
        BuildRail(1, 3);
        BuildRail(1, 4);

        for (int i = 1; i <= 6; i++)
        {
            BuildRail(i, 5);
        }
        BuildRail(6, 3);
        BuildRail(6, 4);
       
        BuildStaton(3, 1);
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
        BuildStaton(3, 1);
    }

    private void BuildRail(int x, int y)
    {
        GameManager.Instance.BuildSomething(x, y,WorldTileSpecificationType.Rail,0);
    }
    private void BuildStaton(int x, int y)
    {
        GameManager.Instance.BuildSomething(x, y, WorldTileSpecificationType.Station,0);
    }
}
