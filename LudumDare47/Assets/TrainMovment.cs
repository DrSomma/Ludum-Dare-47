using Enum;
using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldTile;

public class TrainMovment : MonoBehaviour
{
    public float speed = 2f;

    private GameManager gameManager;
    private WorldTileRail nextRail;
    private WorldTileRail curRail;
    private Vector2 targetPos;
    private bool rotateDone;
    private GameObject train_sprite;

    private void Awake()
    {
        //train_sprite = GetComponentInChildren<SpriteRenderer>().gameObject;
        train_sprite = this.gameObject;     
    }

    void Start()
    {
        gameManager = GameManager.Instance;
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);
        if (!nextRail.IsCurve)
        {
            if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), targetPos) < 0.005f)
            {
                transform.position = targetPos;
                curRail = nextRail;
                GetNextTarget(nextRail.GetNextRail().x, nextRail.GetNextRail().y);
                RotateToTarget();

            }
        }
        else
        {
            if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), targetPos) < 0.005f)
            {
                if (!rotateDone)
                {
                    rotateDone = true;
                    GetCurvePoints(out Vector2 curvP1, out Vector2 curvP2);
                    targetPos = new Vector2(nextRail.x, nextRail.y) + curvP2;
                    RotateToTarget(); 
                }
                else
                {
                    curRail = nextRail;
                    GetNextTarget(nextRail.GetNextRail().x, nextRail.GetNextRail().y);
                    RotateToTarget(); 
                }

            }
            
        }
        
    }

    private void RotateToTarget()
    {
        var dir = new Vector3(targetPos.x, targetPos.y, 0) - train_sprite.transform.position;
        var angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        train_sprite.transform.rotation = Quaternion.AngleAxis(360 - angle, Vector3.forward);
    }

    private void GetNextTarget(int nextGridX, int nextGridY)
    {
        Debug.Log($"Train Mov to {nextGridX}|{nextGridY}");
        gameManager.GetFieldStatus(x: nextGridX, y: nextGridY, worldTile: out WorldTileClass nextWorldTile);
        nextRail = (WorldTileRail)nextWorldTile.WorldTileSpecification;


        if (!nextRail.IsCurve)
        {
            targetPos = new Vector2(nextGridX + 0.5f, nextGridY + 0.5f);
        }
        else
        {
            rotateDone = false;
            GetCurvePoints(out Vector2 curvP1, out _);
            targetPos = new Vector2(nextGridX, nextGridY) + curvP1;
            //targetPosCheck = targetPos + curvP1;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(new Vector2(transform.position.x + 0.5f, transform.position.y + 0.5f), 0.1f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(targetPos, 0.1f);
    }

    private void GetCurvePoints(out Vector2 firstPoint, out Vector2 secondPoint)
    {
        WorldTileRail nextNextRail = nextRail.GetNextRail();

        //Left->Up
        if (curRail.y < nextNextRail.y && curRail.x < nextNextRail.x)
        {
            //train_sprite.transform.rotation = Quaternion.Euler(0, 0, -45);
            firstPoint = new Vector2(0f, 0.5f);
            secondPoint = new Vector2(0.5f, 1f);
        }
        //Up->Left
        else if (curRail.y < nextNextRail.y && curRail.x > nextNextRail.x)
        {
            //train_sprite.transform.rotation = Quaternion.Euler(0, 0, 90);
            firstPoint = new Vector2(0.5f, 0);
            secondPoint = new Vector2(0f, 0.5f);
        }
        //Right->Down
        else if (curRail.y > nextNextRail.y && curRail.x > nextNextRail.x)
        {
            //train_sprite.transform.rotation = Quaternion.Euler(0, 0, 45);
            firstPoint = new Vector2(1f, 0.5f);
            secondPoint = new Vector2(0.5f, 0f);
        }
        //Up->Right
        else if (curRail.y > nextNextRail.y && curRail.x < nextNextRail.x)
        {
            //train_sprite.transform.rotation = Quaternion.Euler(0, 0, 45);
            firstPoint = new Vector2(0.5f, 1f);
            secondPoint = new Vector2(1f, 0.5f);
        }
        else
        {
            firstPoint = new Vector2(1f, 1f);
            secondPoint = new Vector2(1f, 1f);
        }
    }

    public void StartTrain(int x, int y)
    {
        Debug.Log($"Start Train @ {x}|{y}");
        transform.position = new Vector2(x+0.5f, y + 0.5f);
        WorldTileStatusType status = gameManager.GetFieldStatus(x: x, y: y, worldTile: out WorldTileClass curWorldTile);
        Debug.Log(status);
        curRail = (WorldTileRail)curWorldTile.WorldTileSpecification;

        //Get  next
        GetNextTarget(curRail.GetNextRail().x, curRail.GetNextRail().y);
        RotateToTarget(); 
    }
}
