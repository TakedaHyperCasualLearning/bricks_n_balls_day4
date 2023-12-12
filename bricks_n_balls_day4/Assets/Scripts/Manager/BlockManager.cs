using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private GameObject blockRoot;
    [SerializeField] private TextMeshProUGUI durabilityTextPrefab;
    [SerializeField] private Canvas durabilityTextCanvas;
    private List<BlockData> blockDataList = new List<BlockData>();
    private int blockNumber = 5;
    public void Initialize()
    {
        for (int i = 0; i < blockNumber; i++)
        {
            BlockData block = CreateBlock();
            block.BlockObject.transform.position = new Vector2(i * 1.0f, 0.0f);
            block.DurabilityText.transform.position = block.BlockObject.transform.position;
        }
    }

    public void OnUpdate()
    {

    }

    public BlockData CreateBlock()
    {
        GameObject blockObject = Instantiate(blockPrefab, new Vector2(0.0f, 0.0f), Quaternion.identity);
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
            Durability = 1,
            IsBreak = false
        };
        blockDataList.Add(blockData);
        return blockData;
    }
}