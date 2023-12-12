using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DottedLineManager : MonoBehaviour
{
    [SerializeField] private GameObject dotPointMarkerPrefab;
    [SerializeField] private GameObject impactPointMarkerPrefab;
    [SerializeField] private GameObject dotPointMarkerRoot;
    [SerializeField] private GameObject impactPointMarkerRoot;

    private List<PointMarkerData> dotPointMarkerList = new List<PointMarkerData>();
    private PointMarkerData impactPointMarker = null;
    private int drawDotPointCount = 0;
    private float DOT_POINT_INTERVAL = 0.2f;
    private int REFLECTION_COUNT = 6;
    private Vector2 startingPosition = new Vector2(0.0f, -4.5f);

    public void Initialize()
    {
        impactPointMarker = new PointMarkerData()
        {
            PointMarkerObject = Instantiate(impactPointMarkerPrefab),
            Radius = impactPointMarkerPrefab.transform.localScale.x / 2.0f
        };
        impactPointMarker.PointMarkerObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
        impactPointMarker.PointMarkerObject.transform.SetParent(impactPointMarkerRoot.transform);
        impactPointMarker.PointMarkerObject.SetActive(false);
    }

    public Vector2 DrawDottedLine(Func<Vector2, float, Vector2, Vector2> IsOutScreen)
    {
        drawDotPointCount = 0;
        Vector2 mouseMosution = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mouseMosution - startingPosition;
        direction.Normalize();

        for (int i = 0; i < 100; i++)
        {
            Vector2 position = startingPosition + direction * DOT_POINT_INTERVAL * i;

            Vector2 reflectNormal = IsOutScreen(position, 0.05f, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height))); // 画面外に出たかどうかの判定
            if (reflectNormal != Vector2.zero)
            {
                position = DrawImpactPoint(position, reflectNormal, direction, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)));

                Vector2 reflectDirection = Vector2.Reflect(direction, reflectNormal);

                for (int j = 0; j < REFLECTION_COUNT; j++)
                {
                    Vector2 dotPointPosition = position + reflectDirection * DOT_POINT_INTERVAL * j;
                    DrawDotPoint(drawDotPointCount, dotPointPosition);
                }
                break;
            }

            DrawDotPoint(drawDotPointCount, position);
        }

        // 使っていないもの非表示
        ClearDottedLine(drawDotPointCount);

        return direction;
    }

    //プール内なら配置、プール外なら生成
    private void DrawDotPoint(int index, Vector2 position)
    {
        if (index >= dotPointMarkerList.Count)
        {
            GameObject dotPointMarker = Instantiate(dotPointMarkerPrefab);
            dotPointMarker.transform.position = position;
            dotPointMarker.transform.SetParent(dotPointMarkerRoot.transform);
            PointMarkerData pointMarkerData = new PointMarkerData()
            {
                PointMarkerObject = dotPointMarker,
                Radius = dotPointMarker.transform.localScale.x / 2.0f
            };
            dotPointMarkerList.Add(pointMarkerData);
            return;
        }
        else
        {
            dotPointMarkerList[index].PointMarkerObject.SetActive(true);
            dotPointMarkerList[index].PointMarkerObject.transform.position = position;
        }

        drawDotPointCount++;
    }

    private Vector2 DrawImpactPoint(Vector2 position, Vector2 reflectNormal, Vector2 direction, Vector2 screenEdge)
    {
        float differenceRation = 0.0f;
        if (reflectNormal.x > 0.0f) differenceRation = (-screenEdge.x - position.x) / direction.x;
        if (reflectNormal.x < 0.0f) differenceRation = (screenEdge.x - position.x) / direction.x;
        if (reflectNormal.y > 0.0f) differenceRation = (-screenEdge.y - position.y) / direction.y;
        if (reflectNormal.y < 0.0f) differenceRation = (screenEdge.y - position.y) / direction.y;

        Vector2 impactPosition = position + direction * differenceRation;
        impactPointMarker.PointMarkerObject.SetActive(true);
        impactPointMarker.PointMarkerObject.transform.position = impactPosition;
        return impactPosition;
    }

    private void ClearDottedLine(int index)
    {
        for (int i = index; i < dotPointMarkerList.Count; i++)
        {
            dotPointMarkerList[i].PointMarkerObject.SetActive(false);
        }
    }

    public void AllClearDottedLine()
    {
        for (int i = 0; i < dotPointMarkerList.Count; i++)
        {
            dotPointMarkerList[i].PointMarkerObject.SetActive(false);
        }
        impactPointMarker.PointMarkerObject.SetActive(false);
    }
}
