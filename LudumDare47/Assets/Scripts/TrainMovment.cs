using Enum;
using Manager;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using WorldTile;

public class TrainMovment : MonoBehaviour
{
    public float speed = 2f;
    private float _curSpeed;

    private GameManager _gameManager;
    private WorldTileRail _nextRail;
    private WorldTileRail _curRail;
    private Vector2 _targetPos;
    private bool _rotateDone;
    private GameObject _trainSprite;

    private bool _isStopped = true; 
    private int _trainTraveledTiles;

    private void Awake()
    {
        _trainSprite = this.gameObject;
        _gameManager = GameManager.Instance;
        _curSpeed = speed;
    }


    void Update()
    {
        if (_isStopped)
            return;

        transform.position = Vector2.MoveTowards(transform.position, _targetPos, Time.deltaTime * _curSpeed);
        if (!_nextRail.IsCurve)
        {
            if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), _targetPos) < 0.005f)
            {
                transform.position = _targetPos;
                _curRail = _nextRail;
                GetNextTarget(_nextRail.GetNextRail().x, _nextRail.GetNextRail().y);
                RotateToTarget();

            }
        }
        else
        {
            if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), _targetPos) < 0.005f)
            {
                if (!_rotateDone)
                {
                    _rotateDone = true;
                    GetCurvePoints(out Vector2 curvP1, out Vector2 curvP2);
                    _targetPos = new Vector2(_nextRail.x, _nextRail.y) + curvP2;
                    RotateToTarget();
                }
                else
                {
                    _curRail = _nextRail;
                    GetNextTarget(_nextRail.GetNextRail().x, _nextRail.GetNextRail().y);
                    RotateToTarget();
                }

            }

        }

    }

    private void RotateToTarget()
    {
        var dir = new Vector3(_targetPos.x, _targetPos.y, 0) - _trainSprite.transform.position;
        var angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        _trainSprite.transform.rotation = Quaternion.AngleAxis(360 - angle, Vector3.forward);
    }

    private int CalcMoney(int level)
    {
        return (int) Mathf.Pow(_trainTraveledTiles,1.3f)*2*(level+1);
    }

    public void CheckIfTrackStillLoop()
    {
        if (!_curRail._trackFinished)
        {
            Debug.Log("Delete Train: " + _curRail._trackFinished);
            Destroy(this.gameObject);
        }
    }

    private void GetNextTarget(int nextGridX, int nextGridY)
    {
        //check for station
        List<WorldTileClass> neighbours = _gameManager.GetNeighbourTiles(_curRail.x, _curRail.y);
        var station = neighbours.FirstOrDefault(x => x.worldTileSpecificationType == WorldTileSpecificationType.Station);
        if(station != null)
        {
            WorldTileStation temp = station.WorldTileSpecification as WorldTileStation;
            _gameManager.ChangeMoney(CalcMoney(temp.UpgradeLevel), transform.position);
            SoundManager.Instance.PlaySoundCoins();
            _trainTraveledTiles = 0;
        }
        else
        {
            _trainTraveledTiles++;
            if(_trainTraveledTiles > _curRail._trackRailCount)
            {
                _trainTraveledTiles = 0;
            }
        }

        //Debug.Log($"Train Mov to {nextGridX}|{nextGridY}");
        _gameManager.GetFieldStatus(x: nextGridX, y: nextGridY, worldTile: out WorldTileClass nextWorldTile);
        _nextRail = (WorldTileRail)nextWorldTile.WorldTileSpecification;

        //Add speed
        _curSpeed = speed + 0.5f * _nextRail.UpgradeLevel;

        if (!_nextRail.IsCurve)
        {
            _targetPos = new Vector2(nextGridX + 0.5f, nextGridY + 0.5f);
        }
        else
        {
            _rotateDone = false;
            GetCurvePoints(out Vector2 curvP1, out _);
            _targetPos = new Vector2(nextGridX, nextGridY) + curvP1;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(new Vector2(transform.position.x + 0.5f, transform.position.y + 0.5f), 0.1f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(_targetPos, 0.1f);
    }

    private void GetCurvePoints(out Vector2 firstPoint, out Vector2 secondPoint)
    {
        WorldTileRail nextNextRail = _nextRail.GetNextRail();
        bool? isAbove = _nextRail.CheckIfPointIsAboveDir();

        firstPoint = Vector2.zero;
        secondPoint = Vector2.zero;
        //Debug.Log($"{nextRail.CompassDirection} | {isAbove.Value}");
        switch (_nextRail.CompassDirection)
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
        _curRail = startRail;

        //Get  next
        GetNextTarget(_curRail.GetNextRail().x, _curRail.GetNextRail().y);
        RotateToTarget();

        _trainTraveledTiles = 0;


        _isStopped = false;
    }
}
