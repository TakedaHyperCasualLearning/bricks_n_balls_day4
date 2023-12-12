using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class StageManager
{
    private List<List<bool>> stage = new List<List<bool>>();
    private int STAGE_WIDTH = 9;
    private int STAGE_HEIGHT = 6;
    private Vector2 PLACEMENT_MARGIN = new Vector2(0.1f, 0.1f);
    private Func<Vector2, BlockData> createBlockDataFunction;
    private Func<bool> clearCheckFunction;
    private Func<bool> stopCheckFunction;

    public void Initialize(Func<Vector2, BlockData> createBlock)
    {
        createBlockDataFunction = createBlock;
        GenerateStage();
        CreateStage();
    }

    public void OnUpdate(Func<bool> clearCheck, Func<bool> stopCheck)
    {
        clearCheckFunction = clearCheck;
        stopCheckFunction = stopCheck;
        if (clearCheckFunction.Invoke() && !stopCheckFunction.Invoke())
        {
            GenerateStage();
            CreateStage();
        }
    }

    public void GenerateStage()
    {
        for (int i = 0; i < STAGE_HEIGHT; i++)
        {
            stage.Add(new List<bool>());
            for (int j = 0; j < STAGE_WIDTH; j++)
            {
                int rand = UnityEngine.Random.Range(0, 100);
                if (rand >= 50)
                {
                    stage[i].Add(false);
                }
                else
                {
                    stage[i].Add(true);
                }
            }
        }
    }

    public void CreateStage()
    {
        Vector2 stageSide = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 blockSize = new Vector2(0.5f, 0.5f);
        for (int i = 0; i < STAGE_HEIGHT; i++)
        {
            for (int j = 0; j < STAGE_WIDTH; j++)
            {
                if (!stage[i][j]) continue;
                BlockData blockData = createBlockDataFunction.Invoke(new Vector2((j + 1) * blockSize.x + PLACEMENT_MARGIN.x * j - stageSide.x, (-i - 1) * blockSize.y - PLACEMENT_MARGIN.y * i + stageSide.y));
            }
        }
    }
}
