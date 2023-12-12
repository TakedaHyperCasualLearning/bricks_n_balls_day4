using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField] private BallManager ballManager;

    // Start is called before the first frame update
    void Start()
    {
        ballManager.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        ballManager.OnUpdate();
    }
}
