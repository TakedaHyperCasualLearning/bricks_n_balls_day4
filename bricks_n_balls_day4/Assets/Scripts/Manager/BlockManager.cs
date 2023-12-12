using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private GameObject blockRoot;
    [SerializeField] private TextMeshProUGUI durabilityTextPrefab;
    [SerializeField] private Canvas durabilityTextCanvas;
    private List<BlockData> blockDataList = new List<BlockData>();
    private int activeBlockCount = 0;
    private int breakBlockCount = 0;
    private bool isAllBlockBreak = false;

    public void Initialize()
    {
    }

    public void OnUpdate()
    {

    }

    public void OnHitBlock(BlockData blockData)
    {
        blockData.Durability--;
        blockData.DurabilityText.text = blockData.Durability.ToString();
        if (blockData.Durability <= 0)
        {
            blockData.IsBreak = true;
            breakBlockCount++;
            blockData.BlockObject.SetActive(false);
            blockData.DurabilityText.gameObject.SetActive(false);
        }

        if (breakBlockCount >= activeBlockCount)
        {
            isAllBlockBreak = true;
            breakBlockCount = 0;
            activeBlockCount = 0;
        }
    }

    public BlockData CreateBlock(Vector2 position)
    {
        isAllBlockBreak = false;
        if (activeBlockCount >= blockDataList.Count)
        {
            GameObject blockObject = Instantiate(blockPrefab, position, Quaternion.identity);
            blockObject.transform.SetParent(blockRoot.transform);
            TextMeshProUGUI durabilityText = Instantiate(durabilityTextPrefab, new Vector2(0.0f, 0.0f), Quaternion.identity);
            durabilityText.transform.SetParent(durabilityTextCanvas.transform);
            durabilityText.transform.position = blockObject.transform.position;
            durabilityText.transform.localScale = new Vector2(1.0f, 1.0f);
            BlockData blockData = new BlockData
            {
                BlockObject = blockObject,
                DurabilityText = durabilityText,
                Size = blockObject.transform.localScale / 2,
                Durability = UnityEngine.Random.Range(30, 50),
                IsBreak = false
            };
            durabilityText.text = blockData.Durability.ToString();
            blockDataList.Add(blockData);
            activeBlockCount++;
            return blockData;
        }
        else
        {
            blockDataList[activeBlockCount].BlockObject.SetActive(true);
            blockDataList[activeBlockCount].DurabilityText.gameObject.SetActive(true);
            blockDataList[activeBlockCount].BlockObject.transform.position = position;
            blockDataList[activeBlockCount].DurabilityText.transform.position = position;
            blockDataList[activeBlockCount].Durability = UnityEngine.Random.Range(30, 50);
            blockDataList[activeBlockCount].DurabilityText.text = blockDataList[activeBlockCount].Durability.ToString();
            blockDataList[activeBlockCount].IsBreak = false;
            activeBlockCount++;
            return blockDataList[activeBlockCount - 1];
        }
    }

    public List<BlockData> GetBlockDataList()
    {
        return blockDataList;
    }

    public bool IsAllBlockBreak()
    {
        return isAllBlockBreak;
    }
}
