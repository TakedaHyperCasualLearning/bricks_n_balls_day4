using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private GameObject ballRoot;
    private List<BallData> ballDataList = new List<BallData>();
    private int possessionNumber = 10;
    private float SPEED = 0.05f;
    private bool isShot = false;
    private float shotTimer = 0.0f;
    private float SHOT_INTERVAL = 0.2f;
    private int shotCount = 0;

    public void Initialize()
    {

    }

    public void OnUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePosition - (Vector2)transform.position;
            direction.Normalize();
            ShotAllBall(direction);
        }

        if (!isShot) return;

        for (int i = 0; i < ballDataList.Count; i++)
        {
            if (i >= shotCount) break;
            BallData ballData = ballDataList[i];

            if (!ballData.IsMoving && !ballData.IsGather) return;

            MoveBall(ballData); // ボールの移動処理
        }

        if (shotCount >= possessionNumber) return;
        shotTimer += Time.deltaTime;
        if (shotTimer < SHOT_INTERVAL) return;
        shotTimer = 0.0f;
        shotCount++;
    }

    private BallData CreateBall()
    {
        GameObject ballObject = Instantiate(ballPrefab);
        ballObject.transform.SetParent(ballRoot.transform);
        BallData ballData = new BallData
        {
            BallObject = ballObject,
            Radius = ballObject.transform.localScale.x / 2,
            Speed = SPEED,
            IsMoving = false,
            IsGather = false
        };
        ballDataList.Add(ballData);
        return ballData;
    }

    private void MoveBall(BallData ballData)
    {
        ballData.BallObject.transform.Translate(ballData.Velocity * ballData.Speed);
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
}
