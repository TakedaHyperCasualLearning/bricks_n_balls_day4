using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField] private BallManager ballManager;
    [SerializeField] private BlockManager blockManager;

    private CollisionManager collisionManager = new CollisionManager();

    // Start is called before the first frame update
    void Start()
    {
        ballManager.Initialize();
        blockManager.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        ballManager.OnUpdate();
        blockManager.OnUpdate();

        ballManager.OnHitCollision(collisionManager.CheckHitEdge, collisionManager.CircleToBox, blockManager.OnHitBlock, blockManager.GetBlockDataList);
    }
}
