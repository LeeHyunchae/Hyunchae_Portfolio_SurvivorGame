using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointCalculator
{
    private const int CREATE_LIMIT_RANGE = 3;
    private const int RANDOM_DIRECTION_LINE_LENGTH = 100;

    private enum EMapLine
    {
        MAP_LEFT_TOP = 0,
        MAP_LEFT_BOTTOM,
        MAP_RIGHT_TOP,
        MAP_RIGHT_BOTTOM,
        END
    }

    private MapData mapData;
    private Vector2[] mapLines = new Vector2[(int)EMapLine.END];

    private bool isRight = false;
    private bool isTop = false;
    private bool isTooClose = false;
    private Vector2 randomDirection = Vector2.zero;
    private Vector2 randomDirectionPos = Vector2.zero;
    private Vector2 playerPos;

    public void SetMapData(MapData _mapData)
    {
        mapData = _mapData;
        SetMapLine();
    }
    private void SetMapLine()
    {
        mapLines[(int)EMapLine.MAP_LEFT_TOP] = new Vector2(mapData.mapWidth * 0.5f * -1, mapData.mapHeight * 0.5f);
        mapLines[(int)EMapLine.MAP_LEFT_BOTTOM] = new Vector2(mapData.mapWidth * 0.5f * -1, mapData.mapHeight * 0.5f * -1);
        mapLines[(int)EMapLine.MAP_RIGHT_TOP] = new Vector2(mapData.mapWidth * 0.5f, mapData.mapHeight * 0.5f);
        mapLines[(int)EMapLine.MAP_RIGHT_BOTTOM] = new Vector2(mapData.mapWidth * 0.5f, mapData.mapHeight * 0.5f * -1);
    }

    public bool GetSpawnPosition(Vector2 _playerPos, out Vector2 _intersectionPos)
    {
        playerPos = _playerPos;
        SetRandomDirection();

        if(CompareMapLineToRandomDirection(out _intersectionPos))
        {
            return true;
        }
        return false;
    }
    private void SetRandomDirection()
    {
        randomDirection = new Vector2(Random.Range(mapData.mapWidth * 0.5f * -1, mapData.mapWidth * 0.5f), Random.Range(mapData.mapHeight * 0.5f * -1, mapData.mapHeight * 0.5f));
        randomDirection.Normalize();

        randomDirectionPos = playerPos + randomDirection * RANDOM_DIRECTION_LINE_LENGTH;

        isRight = randomDirection.x >= 0;
        isTop = randomDirection.y >= 0;
    }

    private bool CompareMapLineToRandomDirection(out Vector2 _intersectionPos)
    {

        if (isTop)
        {
            if(GetCrossPosition(mapLines[(int)EMapLine.MAP_LEFT_TOP], mapLines[(int)EMapLine.MAP_RIGHT_TOP], out _intersectionPos))
            {
                return true;
            }
        }
        else
        {
            if(GetCrossPosition(mapLines[(int)EMapLine.MAP_LEFT_BOTTOM], mapLines[(int)EMapLine.MAP_RIGHT_BOTTOM], out _intersectionPos))
            {
                return true;
            }
        }

        if(isRight)
        {
            if(GetCrossPosition(mapLines[(int)EMapLine.MAP_RIGHT_TOP], mapLines[(int)EMapLine.MAP_RIGHT_BOTTOM], out _intersectionPos))
            {
                return true;
            }
        }
        else
        {
            if (GetCrossPosition(mapLines[(int)EMapLine.MAP_LEFT_TOP], mapLines[(int)EMapLine.MAP_LEFT_BOTTOM], out _intersectionPos))
            {
                return true;
            }
         }

        return false;
    }

    private bool GetCrossPosition(Vector2 _mapLineFirstDot, Vector2 _mapLineSecondDot, out Vector2 _intersectionPos)
    {
        float firstLineDotX, firstLineDotY, secondLineDotX,secondLineDotY;

        firstLineDotX = _mapLineFirstDot.x;
        firstLineDotY = _mapLineFirstDot.y;

        secondLineDotX = _mapLineSecondDot.x;
        secondLineDotY = _mapLineSecondDot.y;

        float isCross = (firstLineDotX - secondLineDotX) * (playerPos.y - randomDirectionPos.y) - (firstLineDotY - secondLineDotY) * (playerPos.x - randomDirectionPos.x);

        _intersectionPos.x = ((firstLineDotX * secondLineDotY - firstLineDotY * secondLineDotX)
            * (playerPos.x - randomDirectionPos.x) - (firstLineDotX - secondLineDotX)
            * (playerPos.x * randomDirectionPos.y - playerPos.y * randomDirectionPos.x)) / isCross;

        _intersectionPos.y = ((firstLineDotX * secondLineDotY - firstLineDotY * secondLineDotX)
            * (playerPos.y - randomDirectionPos.y) - (firstLineDotY - secondLineDotY)
            * (playerPos.x * randomDirectionPos.y - playerPos.y * randomDirectionPos.x)) / isCross;

        if (isCross == 0)
        {
            return false;
        }
        else
        {
            if(CheckDotInLine(_mapLineFirstDot, _mapLineSecondDot, _intersectionPos) && CheckDotInLine(playerPos, randomDirectionPos, _intersectionPos))
            {
                float distance = Vector2.Distance(playerPos, _intersectionPos);
                if (distance < CREATE_LIMIT_RANGE)
                {
                    if(isTooClose)
                    {
                        isTooClose = false;
                        return false;
                    }

                    isTooClose = true;
                    ReversCreateDirection();
                    return CompareMapLineToRandomDirection(out _intersectionPos);
                }

                isTooClose = false;
                _intersectionPos = CalculateSpawnPosition(distance);
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    // IntersectionPos In line?
    private bool CheckDotInLine(Vector2 _lineDotA, Vector2 _lineDotB, Vector2 _intersectionPos)
    {
        float epsilon = 0.00001f;
        float distanceAToB = Vector2.Distance(_lineDotA, _lineDotB);
        float distanceAToDot = Vector2.Distance(_lineDotA, _intersectionPos);
        float distanceBToDot = Vector2.Distance(_lineDotB, _intersectionPos);

        return ((distanceAToB + epsilon) >= (distanceAToDot + distanceBToDot));
    }

    private void ReversCreateDirection()
    {
        randomDirection *= -1;
        randomDirectionPos = playerPos + randomDirection * RANDOM_DIRECTION_LINE_LENGTH;
        isTop = !isTop;
        isRight = !isRight;
    }

    private Vector2 CalculateSpawnPosition(float _distance)
    {
        float randomRange = Random.Range(CREATE_LIMIT_RANGE, _distance);

        return playerPos + randomDirection * randomRange;
    }
}
