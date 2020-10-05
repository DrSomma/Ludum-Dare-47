using Enum;
using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldTile;

public class TrainMovment : MonoBehaviour
{
    public float speed = 2f;

    //TODO SOLLTE SINGELTEN VERWENDEN!!!!!!!!!!!!
    public GameManager gameManager;
    private WorldTileRail nextRail;
    private int nextGridX;
    private int nextGridY;
    private Transform targetTransform;
    private Vector2 targetPos;

    // Start is called before the first frame update
    void Start()
    {
        //gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);
        if(Vector2.Distance(transform.position,targetPos) < 0.1f)
        {
            
            GetNextTarget(nextRail.NextRail.x, nextRail.NextRail.y);
        }
    }

    private void GetNextTarget(int x, int y)
    {
        Debug.Log($"Train Mov to {x}|{y}");
        gameManager.GetFieldStatus(x: x, y: y, worldTile: out WorldTileClass nextWorldTile);
        nextRail = (WorldTileRail)nextWorldTile.WorldTileSpecification;
        nextGridX = x;
        nextGridY = y;
        targetPos = new Vector2(nextGridX, nextGridY);
    }

    public void StartTrain(int x, int y)
    {
        Debug.Log($"Start Train @ {x}|{y}");
        transform.position = new Vector2(x, y);
        WorldTileStatusType status = gameManager.GetFieldStatus(x: x, y: y, worldTile: out WorldTileClass curWorldTile);
        Debug.Log(status);
        WorldTileRail curRail = (WorldTileRail)curWorldTile.WorldTileSpecification;

        //Get  next
        GetNextTarget(curRail.NextRail.x, curRail.NextRail.y);
    }
}
