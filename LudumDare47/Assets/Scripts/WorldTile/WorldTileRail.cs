using System;
using System.Collections.Generic;
using System.Linq;
using Enum;
using Manager;
using UnityEngine;

namespace WorldTile
{
    public class WorldTileRail : WorldTileSpecification
    {
        public bool IsCurve;

        public CompassDirection CompassDirection;

        private int ObjectId => _parent.objectId;
        private Vector2 Position => _parent.position;
        public bool _trackFinished = false;
        public int _trackRailCount = 0;
        private WorldTileRail _previousRail;
        private WorldTileRail _nextRail;
        private Sprite _railCurve;
        private Sprite _railStraight;

        private WorldTileClass _parent;

        public WorldTileRail(WorldTileClass parent, List<WorldTileClass> neighbours)
        {
            _parent = parent;

            List<WorldTileRail> railList = neighbours.Where(predicate: n => n != null &&
                                                                            n.worldTileSpecificationType ==
                                                                            WorldTileSpecificationType.Rail)
                .Select(selector: n => n.WorldTileSpecification as WorldTileRail)
                .Where(predicate: n => !n._trackFinished).ToList();

            GetSprites();
            CheckForPreviousRail(railList: railList);
            CheckForNextRail(railList: railList);
            CheckForFixRail(railList: railList);
            CheckForCorrectSprite();
            CheckForCorrectSpriteInNeighbours();
            CheckForLoop();

            Type = WorldTileSpecificationType.Rail;
            SoundManager.Instance.PlaySoundPlaceRail();
        }


        private void GetSprites()
        {
            if (SpriteManager.Instance.TryGetSpriteByName(spriteName: "rail_curve", outSprite: out Sprite curve))
            {
                _railCurve = curve;
            }

            if (SpriteManager.Instance.TryGetSpriteByName(spriteName: "rail_straight", outSprite: out Sprite straight))
            {
                _railStraight = straight;
            }
        }

        private void CheckForPreviousRail(List<WorldTileRail> railList)
        {
            WorldTileRail previous = railList.FirstOrDefault(predicate: rail =>
                                                                 rail._nextRail == null &&
                                                                 (rail._previousRail == null ||
                                                                  rail._previousRail != this));

            if (previous != null)
            {
                _previousRail = previous;
                previous._nextRail = this;
            }
        }

        private void CheckForNextRail(List<WorldTileRail> railList)
        {
            WorldTileRail next = railList.FirstOrDefault(predicate: rail =>
                                                             rail._previousRail == null && (rail._nextRail == null ||
                                                                 rail._nextRail != this));

            if (next != null)
            {
                _nextRail = next;
                next._previousRail = this;
            }
        }

        public WorldTileRail GetNextRail()
        {
            return _nextRail;
        }

        private void CheckForFixRail(List<WorldTileRail> railList)
        {
            if (_previousRail != null && _nextRail != null ||
                _previousRail == null && _nextRail == null)
            {
                return;
            }

            if (_previousRail == null)
            {
                WorldTileRail next = railList.FirstOrDefault(predicate: rail =>
                                                                 rail._previousRail == null && rail._nextRail != this);

                if (next != null)
                {
                    _previousRail = next;
                    next.RecursiveDirectionCorrection(next: true);
                    next._nextRail = this;
                }
            }
            else if (_nextRail == null)
            {
                WorldTileRail previous = railList.FirstOrDefault(predicate: rail =>
                                                                     rail._nextRail == null &&
                                                                     rail._previousRail != this);

                if (previous != null)
                {
                    _nextRail = previous;
                    previous.RecursiveDirectionCorrection(next: false);
                    previous._previousRail = this;
                }
            }
        }

        private void CheckForCorrectSprite()
        {
            CalculateCompassDirection();
            bool? isAbove = CheckIfPointIsAboveDir();

            switch (CompassDirection)
            {
                case CompassDirection.N:
                    Sprite = _railStraight;
                    _parent.sprite.transform.rotation = Quaternion.AngleAxis(angle: 90, axis: Vector3.forward);
                    break;
                case CompassDirection.E:
                    Sprite = _railStraight;
                    _parent.sprite.transform.rotation = Quaternion.AngleAxis(angle: 0, axis: Vector3.forward);
                    break;
                case CompassDirection.S:
                    Sprite = _railStraight;
                    _parent.sprite.transform.rotation = Quaternion.AngleAxis(angle: 270, axis: Vector3.forward);
                    break;
                case CompassDirection.W:
                    Sprite = _railStraight;
                    _parent.sprite.transform.rotation = Quaternion.AngleAxis(angle: 180, axis: Vector3.forward);
                    break;
                case CompassDirection.NE:
                case CompassDirection.SW:
                    IsCurve = true;
                    Sprite = _railCurve;
                    if (isAbove.HasValue && isAbove.Value)
                    {
                        _parent.sprite.transform.rotation = Quaternion.AngleAxis(angle: 0, axis: Vector3.forward);
                    }
                    else
                    {
                        _parent.sprite.transform.rotation = Quaternion.AngleAxis(angle: 180, axis: Vector3.forward);
                    }

                    break;
                case CompassDirection.SE:
                case CompassDirection.NW:
                    IsCurve = true;
                    Sprite = _railCurve;
                    if (isAbove.HasValue && isAbove.Value)
                    {
                        _parent.sprite.transform.rotation = Quaternion.AngleAxis(angle: 270, axis: Vector3.forward);
                    }
                    else
                    {
                        _parent.sprite.transform.rotation = Quaternion.AngleAxis(angle: 90, axis: Vector3.forward);
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _parent.UpdateSprite();
        }

        /// <summary>
        /// This calculates, if the point of current is above or
        /// below the line between prev and next
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public bool? CheckIfPointIsAboveDir()
        {
            if (_nextRail == null || _previousRail == null)
            {
                return null;
            }

            int otherY;

            if (Mathf.RoundToInt(f: Position.x) == Mathf.RoundToInt(f: _previousRail.Position.x))
            {
                otherY = Mathf.RoundToInt(f: _previousRail.Position.y);
            }
            else if (Mathf.RoundToInt(f: Position.x) == Mathf.RoundToInt(f: _nextRail.Position.x))
            {
                otherY = Mathf.RoundToInt(f: _nextRail.Position.y);
            }
            else
            {
                return null;
            }

            return Mathf.RoundToInt(f: Position.y) > otherY;
        }

        private void CheckForCorrectSpriteInNeighbours()
        {
            _previousRail?.CheckForCorrectSprite();
            _nextRail?.CheckForCorrectSprite();
        }

        private void CalculateCompassDirection()
        {
            Vector2 vec = Vector2.zero;
            if (_previousRail == null && _nextRail == null)
            {
                CompassDirection = CompassDirection.E;
                return;
            }
            else if (_previousRail == null)
            {
                vec = _nextRail.Position - Position;
            }
            else if (_nextRail == null)
            {
                vec = Position - _previousRail.Position;
            }
            else
            {
                vec = _nextRail.Position - _previousRail.Position;
            }

            float angle = Mathf.Atan2(y: vec.y, x: vec.x);
            int octant = Mathf.RoundToInt(f: 8 * angle / (2 * Mathf.PI) + 8) % 8;

            CompassDirection = (CompassDirection) octant;
        }

        private bool CheckForLoop()
        {
            if (_previousRail == null || _nextRail == null)
            {
                return false;
            }

            int countRails = 1;

            WorldTileRail recursive = _nextRail;

            while (recursive != null && recursive.ObjectId != ObjectId)
            {
                recursive = recursive._nextRail;
                countRails++;
            }

            if (recursive != null)
            {
                Debug.Log(message: $"Loop with {countRails} rails. Nice!");
                _trackFinished = true;
                _trackRailCount = countRails;

                recursive = _nextRail;

                while (recursive != null && recursive.ObjectId != ObjectId)
                {
                    recursive._trackFinished = true;
                    recursive._trackRailCount = countRails;
                    recursive = recursive._nextRail;

                }

                x = (int)_parent.position.x;
                y = (int)_parent.position.y;
                GameManager.Instance.SpawnTrain(this);

                return true;
            }
            else
            {
                return false;
            }
        }

        private void RecursiveDirectionCorrection(bool next)
        {
            if (next)
            {
                _nextRail?.RecursiveDirectionCorrection(next: true);
            }
            else
            {
                _previousRail?.RecursiveDirectionCorrection(next: false);
            }

            WorldTileRail oldPreviousRail = _previousRail;
            _previousRail = _nextRail;
            _nextRail = oldPreviousRail;
            CheckForCorrectSprite();
        }
    }
}