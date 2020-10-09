using Enum;
using Manager;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
        _trainSprite = gameObject;
        _gameManager = GameManager.Instance;
        _curSpeed = speed;
    }


    private void Update()
    {
        if (_isStopped)
        {
            return;
        }

        transform.position = Vector2.MoveTowards(current: transform.position, target: _targetPos, maxDistanceDelta: Time.deltaTime * _curSpeed);
        if (!_nextRail.IsCurve)
        {
            if (Vector2.Distance(a: new Vector2(x: transform.position.x, y: transform.position.y), b: _targetPos) < 0.005f)
            {
                transform.position = _targetPos;
                _curRail = _nextRail;
                GetNextTarget(nextGridX: _nextRail.GetNextRail().x, nextGridY: _nextRail.GetNextRail().y);
                RotateToTarget();
            }
        }
        else
        {
            if (Vector2.Distance(a: new Vector2(x: transform.position.x, y: transform.position.y), b: _targetPos) < 0.005f)
            {
                if (!_rotateDone)
                {
                    _rotateDone = true;
                    GetCurvePoints(firstPoint: out Vector2 _, secondPoint: out Vector2 curvP2);
                    _targetPos = new Vector2(x: _nextRail.x, y: _nextRail.y) + curvP2;
                    RotateToTarget();
                }
                else
                {
                    _curRail = _nextRail;
                    GetNextTarget(nextGridX: _nextRail.GetNextRail().x, nextGridY: _nextRail.GetNextRail().y);
                    RotateToTarget();
                }
            }
        }
    }

    private void RotateToTarget()
    {
        Vector3 dir = new Vector3(x: _targetPos.x, y: _targetPos.y, z: 0) - _trainSprite.transform.position;
        float angle = Mathf.Atan2(y: dir.x, x: dir.y) * Mathf.Rad2Deg;
        _trainSprite.transform.rotation = Quaternion.AngleAxis(angle: 360 - angle, axis: Vector3.forward);
    }

    private int CalcMoney(int level)
    {
        return (int) Mathf.Pow(f: _trainTraveledTiles, p: 1.3f) * 2 * (level + 1);
    }

    public void CheckIfTrackStillLoop()
    {
        if (_curRail._trackFinished)
        {
            return;
        }

        Debug.Log("Delete Train: " + _curRail._trackFinished);
        Destroy(gameObject);
    }

    private void GetNextTarget(int nextGridX, int nextGridY)
    {
        //check for station
        List<WorldTileClass> neighbours = _gameManager.GetNeighbourTiles(x: _curRail.x, y: _curRail.y);
        WorldTileClass station = neighbours.FirstOrDefault(x => x.worldTileSpecificationType == WorldTileSpecificationType.Station);
        if (station != null)
        {
            if (station.WorldTileSpecification is WorldTileStation temp)
            {
                _gameManager.ChangeMoney(sumToAdd: CalcMoney(level: temp.UpgradeLevel), pos: transform.position);
            }

            SoundManager.Instance.PlaySoundCoins();
            _trainTraveledTiles = 0;
        }
        else
        {
            _trainTraveledTiles++;
            if (_trainTraveledTiles > _curRail._trackRailCount)
            {
                _trainTraveledTiles = 0;
            }
        }

        //Debug.Log($"Train Mov to {nextGridX}|{nextGridY}");
        _gameManager.GetFieldStatus(x: nextGridX, y: nextGridY, worldTile: out WorldTileClass nextWorldTile);
        _nextRail = (WorldTileRail) nextWorldTile.WorldTileSpecification;

        //Add speed
        _curSpeed = speed + 0.5f * _nextRail.UpgradeLevel;

        if (!_nextRail.IsCurve)
        {
            _targetPos = new Vector2(x: nextGridX + 0.5f, y: nextGridY + 0.5f);
        }
        else
        {
            _rotateDone = false;
            GetCurvePoints(firstPoint: out Vector2 curvP1, secondPoint: out _);
            _targetPos = new Vector2(x: nextGridX, y: nextGridY) + curvP1;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 position = transform.position;
        Gizmos.DrawSphere(center: new Vector2(x: position.x + 0.5f, y: position.y + 0.5f), radius: 0.1f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(center: _targetPos, radius: 0.1f);
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
                    firstPoint = new Vector2(x: 0.5f, y: 0f);
                    secondPoint = new Vector2(x: 1f, y: 0.5f);
                }
                else
                {
                    firstPoint = new Vector2(x: 0f, y: 0.5f);
                    secondPoint = new Vector2(x: 0.5f, y: 1f);
                }

                break;
            case CompassDirection.SW:
                if (isAbove.HasValue && isAbove.Value)
                {
                    firstPoint = new Vector2(x: 1f, y: 0.5f);
                    secondPoint = new Vector2(x: 0.5f, y: 0f);
                }
                else
                {
                    firstPoint = new Vector2(x: 0.5f, y: 1f);
                    secondPoint = new Vector2(x: 0, y: 0.5f);
                }

                break;
            case CompassDirection.SE:
                if (isAbove.HasValue && isAbove.Value)
                {
                    firstPoint = new Vector2(x: 0f, y: 0.5f);
                    secondPoint = new Vector2(x: 0.5f, y: 0);
                }
                else
                {
                    firstPoint = new Vector2(x: 0.5f, y: 1f);
                    secondPoint = new Vector2(x: 1f, y: 0.5f);
                }

                break;
            case CompassDirection.NW:
                if (isAbove.HasValue && isAbove.Value)
                {
                    firstPoint = new Vector2(x: 0.5f, y: 0);
                    secondPoint = new Vector2(x: 0f, y: 0.5f);
                }
                else
                {
                    firstPoint = new Vector2(x: 1f, y: 0.5f);
                    secondPoint = new Vector2(x: 0.5f, y: 1f);
                }

                break;
        }
    }

    public void StartTrain(WorldTileRail startRail)
    {
        Debug.Log($"Start Train @ {startRail.x}|{startRail.y}");
        transform.position = new Vector2(x: startRail.x + 0.5f, y: startRail.y + 0.5f);
        _curRail = startRail;

        //Get  next
        GetNextTarget(nextGridX: _curRail.GetNextRail().x, nextGridY: _curRail.GetNextRail().y);
        RotateToTarget();

        _trainTraveledTiles = 0;


        _isStopped = false;
    }
}