using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlockData
{
    private GameObject blockObject;
    private TextMeshProUGUI durabilityText;
    private Vector2 size;
    private int durability;
    private bool isBreak;

    public GameObject BlockObject { get => blockObject; set => blockObject = value; }
    public TextMeshProUGUI DurabilityText { get => durabilityText; set => durabilityText = value; }
    public Vector2 Size { get => size; set => size = value; }
    public int Durability { get => durability; set => durability = value; }
    public bool IsBreak { get => isBreak; set => isBreak = value; }
}
