using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager
{
    public Vector2 CircleToBox(Vector2 circlePosition, float circleRadius, Vector2 boxPosition, Vector2 boxSize)
    {
        Vector2 result = new Vector2(0.0f, 0.0f);
        float circleLeft = circlePosition.x - circleRadius;
        float circleRight = circlePosition.x + circleRadius;
        float circleTop = circlePosition.y + circleRadius;
        float circleBottom = circlePosition.y - circleRadius;
        float boxLeft = boxPosition.x - boxSize.x;
        float boxRight = boxPosition.x + boxSize.x;
        float boxTop = boxPosition.y + boxSize.y;
        float boxBottom = boxPosition.y - boxSize.y;

        if (circleLeft > boxRight || circleRight < boxLeft || circleBottom > boxTop || circleTop < boxBottom) return result.normalized;

        if (circleLeft < boxRight && circleRight > boxRight) result += Vector2.left;
        if (circleRight > boxLeft && circleLeft < boxLeft) result += Vector2.right;
        if (circleBottom < boxTop && circleTop > boxTop) result += Vector2.down;
        if (circleTop > boxBottom && circleBottom < boxBottom) result += Vector2.up;

        return result.normalized;
    }

    public Vector2 CheckHitEdge(Vector2 position, float radius, Vector2 screenSize)
    {
        Vector2 result = new Vector2(0.0f, 0.0f);
        float left = position.x - radius;
        float right = position.x + radius;
        float top = position.y + radius;
        float bottom = position.y - radius;


        if (left < -screenSize.x) result += Vector2.right;
        if (right > screenSize.x) result += Vector2.left;
        if (top > screenSize.y) result += Vector2.down;
        if (bottom < -screenSize.y) result += Vector2.up;

        return result.normalized;
    }
}
