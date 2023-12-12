using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using TMPro;

public class BallManager : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private GameObject ballRoot;
    [SerializeField] private TextMeshProUGUI ballCountText;
    private List<BallData> ballDataList = new List<BallData>();
    private int possessionNumber = 80;
    private float SHOT_SPEED = 0.05f;
    private float GATHER_SPEED = 0.1f;
    private bool isShot = false;
    private float shotTimer = 0.0f;
    private float SHOT_INTERVAL = 0.05f;
    private int shotCount = 0;
    private int stopCount = 0;
    private int gatherCount = 0;
    private Vector2 firstPosition = new Vector2(0.0f, -4.5f);
    private float COUNT_TEXT_OFFSET = 0.25f;
    private float STOP_POSITION_OFFSET = 0.1f;
    private Func<Vector2, float, Vector2, Vector2> edgeCollisionFunction;
    private Func<Vector2, float, Vector2, Vector2, Vector2> boxCollisionFunction;
    private Action<BlockData> otherHitActionFunction;
    private Func<List<BlockData>> blockDataListFunction;

    public void Initialize()
    {
        ballCountText.text = "×" + possessionNumber.ToString();
        ballCountText.transform.position = new Vector2(firstPosition.x, firstPosition.y + COUNT_TEXT_OFFSET);
        BallData ballData = CreateBall();
        ballData.BallObject.SetActive(true);
    }

    public void OnUpdate(
        Func<Vector2, float, Vector2, Vector2> edgeCollision,
        Func<Vector2, float, Vector2, Vector2, Vector2> boxCollision,
        Action<BlockData> otherHitAction,
        Func<List<BlockData>> blockDataList)
    {
        if (!isShot) return;

        edgeCollisionFunction = edgeCollision;
        boxCollisionFunction = boxCollision;
        otherHitActionFunction = otherHitAction;
        blockDataListFunction = blockDataList;

        for (int i = 0; i < ballDataList.Count; i++)
        {
            if (i >= shotCount) break;
            BallData ballData = ballDataList[i];

            if (!ballData.IsMoving) continue;

            MoveBall(ballData); // ボールの移動処理
            OnHitCollision(ballData); // ボールの衝突処理
        }

        if (shotCount >= possessionNumber) return;
        shotTimer += Time.deltaTime;
        if (shotTimer < SHOT_INTERVAL) return;
        shotTimer = 0.0f;
        shotCount++;
    }

    // ボールの生成
    private BallData CreateBall()
    {
        GameObject ballObject = Instantiate(ballPrefab);
        ballObject.transform.position = firstPosition;
        ballObject.transform.SetParent(ballRoot.transform);
        BallData ballData = new BallData
        {
            BallObject = ballObject,
            Radius = ballObject.transform.localScale.x / 2,
            Speed = SHOT_SPEED,
            IsMoving = false,
            IsGather = false
        };
        ballDataList.Add(ballData);
        return ballData;
    }

    private void MoveBall(BallData ballData)
    {
        if (!ballData.IsGather)
        {
            ballData.BallObject.transform.Translate(ballData.Velocity * ballData.Speed);
        }
        else
        {
            ballData.GatherTimer += Time.deltaTime;
            ballData.BallObject.transform.position = Vector2.Lerp(ballData.StopPosition, firstPosition, ballData.GatherTimer / GATHER_SPEED);
            if (ballData.GatherTimer >= GATHER_SPEED) StopBall(ballData);
        }
    }

    public void ShotAllBall(Vector2 velocity)
    {
        isShot = true;
        for (int i = 0; i < possessionNumber; i++)
        {
            if (i >= ballDataList.Count) { CreateBall(); }

            BallData ballData = ballDataList[i];
            ballData.BallObject.SetActive(true);
            ballData.Velocity = velocity;
            ballData.IsMoving = true;
            ballData.IsGather = false;
        }

        if (possessionNumber >= ballDataList.Count) return;
        for (int i = possessionNumber; i < ballDataList.Count; i++)
        {
            BallData ballData = ballDataList[i];
            ballData.BallObject.SetActive(false);
            ballData.IsMoving = false;
            ballData.IsGather = false;
        }
    }

    public void OnHitCollision(BallData ballData)
    {
        Vector2 result = edgeCollisionFunction.Invoke(ballData.BallObject.transform.position, ballData.Radius, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)));

        if (result == Vector2.up)
        {
            GatherStart(ballData);
            return;
        }
        if (result != Vector2.zero)
        {
            ballData.Velocity = Vector2.Reflect(ballData.Velocity, result);
            return;
        }


        List<BlockData> blockList = blockDataListFunction.Invoke();

        for (int j = 0; j < blockList.Count; j++)
        {
            BlockData blockData = blockList[j];

            if (blockData.IsBreak) continue;
            result = boxCollisionFunction.Invoke(ballData.BallObject.transform.position, ballData.Radius, blockData.BlockObject.transform.position, blockData.Size);

            if (result == Vector2.zero) continue;
            ballData.Velocity = Vector2.Reflect(ballData.Velocity, result);
            otherHitActionFunction.Invoke(blockData);
        }
    }

    public void GatherStart(BallData ballData)
    {
        gatherCount++;
        ballData.IsGather = true;
        ballData.BallObject.transform.position = new Vector2(ballData.BallObject.transform.position.x, ballData.BallObject.transform.position.y + STOP_POSITION_OFFSET);
        ballData.StopPosition = ballData.BallObject.transform.position;
        ballData.Velocity = Vector2.zero;
        if (gatherCount == 1) firstPosition = ballData.StopPosition;
    }

    public void StopBall(BallData ballData)
    {
        ballData.BallObject.transform.position = firstPosition;
        ballData.IsGather = false;
        ballData.IsMoving = false;
        ballData.GatherTimer = 0.0f;
        stopCount++;
        ballCountText.text = "×" + stopCount.ToString();
        ballCountText.transform.position = new Vector2(firstPosition.x, firstPosition.y + COUNT_TEXT_OFFSET);
        if (stopCount == shotCount)
        {
            isShot = false;
            shotCount = 0;
            stopCount = 0;
            gatherCount = 0;
        }
    }

    public bool IsShot() { return isShot; }
    public Vector2 GetFirstPosition() { return firstPosition; }
}
