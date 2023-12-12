using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField] private BallManager ballManager;
    [SerializeField] private BlockManager blockManager;
    [SerializeField] private DottedLineManager dottedLineManager;
    private CollisionManager collisionManager = new CollisionManager();

    // Start is called before the first frame update
    void Start()
    {
        ballManager.Initialize();
        blockManager.Initialize();
        dottedLineManager.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        ballManager.OnUpdate();
        blockManager.OnUpdate();

        dottedLineManager.DrawDottedLine(collisionManager.CheckHitEdge);

        ballManager.OnHitCollision(collisionManager.CheckHitEdge, collisionManager.CircleToBox, blockManager.OnHitBlock, blockManager.GetBlockDataList);
    }
}
