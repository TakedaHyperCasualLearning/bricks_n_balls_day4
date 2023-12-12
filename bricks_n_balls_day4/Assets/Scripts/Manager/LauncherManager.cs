using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherManager
{
    Vector2 direction = Vector2.zero;

    public void OnUpdate(
        Action<Vector2> shot,
        Func<Func<Vector2, float, Vector2, Vector2>, Vector2> dottedLine,
        Func<Vector2, float, Vector2, Vector2> edgeCollision,
        Func<bool> shotCheck,
        Action lineClear
    )
    {
        Action<Vector2> shotAction = shot;
        Func<Func<Vector2, float, Vector2, Vector2>, Vector2> dottedLineFunc = dottedLine;
        Func<Vector2, float, Vector2, Vector2> edgeCollisionFunc = edgeCollision;
        Func<bool> shotCheckFunc = shotCheck;
        Action otherAction = lineClear;

        if (shotCheckFunc.Invoke()) return;

        if (Input.GetMouseButton(0))
        {
            direction = dottedLineFunc.Invoke(edgeCollisionFunc);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            shotAction.Invoke(direction);
            direction = Vector2.zero;
            otherAction.Invoke();
        }
    }
}
