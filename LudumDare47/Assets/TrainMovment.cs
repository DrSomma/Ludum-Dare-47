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
    private WorldTileRail curRail;
    private Vector2 targetPos;
    private Vector2 targetPosCheck;
    private GameObject train_sprite;

    private void Awake()
    {
        train_sprite = GetComponentInChildren<SpriteRenderer>().gameObject;
    }

    void Start()
    {
        //gameManager = GameManager.Instance;
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);
        if(Vector2.Distance(new Vector2(transform.position.x+0.5f, transform.position.y+0.5f), targetPosCheck) < 0.005f)
        {
            transform.position = targetPos;
            curRail = nextRail;
            GetNextTarget(nextRail.NextRail.x, nextRail.NextRail.y);
        }
    }

    private void GetNextTarget(int nextGridX, int nextGridY)
    {
        Debug.Log($"Train Mov to {nextGridX}|{nextGridY}");
        gameManager.GetFieldStatus(x: nextGridX, y: nextGridY, worldTile: out WorldTileClass nextWorldTile);
        nextRail = (WorldTileRail)nextWorldTile.WorldTileSpecification;

        RotateTrain(nextGridX, nextGridY);

        targetPos = new Vector2(nextGridX, nextGridY);
        targetPosCheck = new Vector2(nextGridX+0.5f, nextGridY + 0.5f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(targetPosCheck, 0.1f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(new Vector2(transform.position.x + 0.5f, transform.position.y + 0.5f), 0.1f);
    }

    private void RotateTrain(int nextGridX, int nextGridY)
    {
        if (nextGridY > curRail.y)
        {
            train_sprite.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (nextGridY < curRail.y)
        {
            train_sprite.transform.rotation = Quaternion.Euler(0, 0, 180);
        }

        if (nextGridX > curRail.x)
        {
            train_sprite.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (nextGridX < curRail.x)
        {
            train_sprite.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
    }

    public void StartTrain(int x, int y)
    {
        Debug.Log($"Start Train @ {x}|{y}");
        transform.position = new Vector2(x, y);
        WorldTileStatusType status = gameManager.GetFieldStatus(x: x, y: y, worldTile: out WorldTileClass curWorldTile);
        Debug.Log(status);
        curRail = (WorldTileRail)curWorldTile.WorldTileSpecification;

        //Get  next
        GetNextTarget(curRail.NextRail.x, curRail.NextRail.y);
    }
}
