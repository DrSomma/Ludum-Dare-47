using Enum;
using Manager;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WorldTile;

public class TrainMovment : MonoBehaviour
{
    public float speed = 2f;
    private float curSpeed;

    private GameManager gameManager;
    private WorldTileRail nextRail;
    private WorldTileRail curRail;
    private Vector2 targetPos;
    private bool rotateDone;
    private GameObject train_sprite;

    private bool IsStopped = true;
    public int tratraveledTiles = 0;

    private void Awake()
    {
        //train_sprite = GetComponentInChildren<SpriteRenderer>().gameObject;
        train_sprite = this.gameObject;     
        gameManager = GameManager.Instance;
        curSpeed = speed;
    }


    void Update()
    {
        if (IsStopped)
            return;

        transform.position = Vector2.MoveTowards(transform.position, targetPos, Time.deltaTime * curSpeed);
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

    private int CalcMoney(int level)
    {
        return (int) Mathf.Pow(tratraveledTiles,1.3f);
    }

    public void CheckIfTrackStillLoop()
    {
        if (!curRail._trackFinished)
        {
            Debug.Log("Delete Train: " + curRail._trackFinished);
            Destroy(this.gameObject);
        }
    }
    
    private void GetNextTarget(int nextGridX, int nextGridY)
    {
        //if(curRail == null || nextRail == null || !curRail._trackFinished)


        //check for station
        List<WorldTileClass> neighbours = gameManager.GetNeighbourTiles(curRail.x, curRail.y);
        var station = neighbours.FirstOrDefault(x => x.worldTileSpecificationType == WorldTileSpecificationType.Station);
        if(station != null)
        {
            WorldTileStation temp = station.WorldTileSpecification as WorldTileStation;
            gameManager.ChangeMoney(CalcMoney(temp.UpgradeLevel));
            SoundManager.Instance.PlaySoundCoins();
            tratraveledTiles = 0;
        }
        else
        {
            tratraveledTiles++;
            if(tratraveledTiles > curRail._trackRailCount)
            {
                tratraveledTiles = 0;
            }
        }

        //Debug.Log($"Train Mov to {nextGridX}|{nextGridY}");
        gameManager.GetFieldStatus(x: nextGridX, y: nextGridY, worldTile: out WorldTileClass nextWorldTile);
        nextRail = (WorldTileRail)nextWorldTile.WorldTileSpecification;

        //Add speed
        curSpeed = speed + 0.5f * nextRail.UpgradeLevel;

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
        bool? isAbove = nextRail.CheckIfPointIsAboveDir();

        firstPoint = Vector2.zero;
        secondPoint = Vector2.zero;
        Debug.Log($"{nextRail.CompassDirection} | {isAbove.Value}");
        switch (nextRail.CompassDirection)
        {
            case CompassDirection.NE:
                if (isAbove.HasValue && isAbove.Value)
                {
                    firstPoint = new Vector2(0.5f, 0f);
                    secondPoint = new Vector2(1f, 0.5f);
                }
                else
                {
                    firstPoint = new Vector2(0f, 0.5f);
                    secondPoint = new Vector2(0.5f, 1f);
                }
                break;
            case CompassDirection.SW:
                if (isAbove.HasValue && isAbove.Value)
                {
                    firstPoint = new Vector2(1f, 0.5f);
                    secondPoint = new Vector2(0.5f, 0f);
                }
                else
                {
                    firstPoint = new Vector2(0.5f, 1f);
                    secondPoint = new Vector2(0, 0.5f);
                }
                break;
            case CompassDirection.SE:
                if (isAbove.HasValue && isAbove.Value)
                {
                    firstPoint = new Vector2(0f, 0.5f);
                    secondPoint = new Vector2(0.5f, 0);
                }
                else
                {
                    firstPoint = new Vector2(0.5f, 1f);
                    secondPoint = new Vector2(1f, 0.5f);
                }

                break;
            case CompassDirection.NW:
                if (isAbove.HasValue && isAbove.Value)
                {
                    firstPoint = new Vector2(0.5f, 0);
                    secondPoint = new Vector2(0f, 0.5f);
                }
                else
                {
                    firstPoint = new Vector2(1f, 0.5f);
                    secondPoint = new Vector2(0.5f, 1f);
                }

                break;                
        }   
    }

    public void StartTrain(WorldTileRail startRail)
    {
        Debug.Log($"Start Train @ {startRail.x}|{startRail.y}");
        transform.position = new Vector2(startRail.x + 0.5f, startRail.y + 0.5f);
        curRail = startRail;

        //Get  next
        GetNextTarget(curRail.GetNextRail().x, curRail.GetNextRail().y);
        RotateToTarget();

        tratraveledTiles = 0;


        IsStopped = false;  
    }
}
