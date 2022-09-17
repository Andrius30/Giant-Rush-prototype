using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static Action<PlayerController> onPlayerSpawn;

    public float moveSpeed = 5f;
    public float lerpSpeed;
    public float reduceSpeedRatio = 0.3f;
    public PlayerView playerView; // controlls visuals

    [SerializeField] int currentStrengh = 0;
    [SerializeField] Camera mainCamera;
    [SerializeField] List<float> movePositions;

    public int currentPositionIndex { get; set; }
    public List<float> MovePositions => movePositions;

    public PlayerModel playerModel { get; private set; }
    public Inputs Inputs { get; private set; }

    void Start()
    {
        onPlayerSpawn?.Invoke(this);
        playerModel = new PlayerModel(this);
        Inputs = new Inputs();
    }
    void Update()
    {
        Vector3 currentPosition = transform.position;
#if UNITY_EDITOR
        if (Inputs.LeftInput())
        {
            playerModel.MoveLeft();
        }
        else if (Inputs.RightInput())
        {
            playerModel.MoveRight();
        }
#endif
#if UNITY_ANDROID
        Inputs.DetectTouches();
        if (Inputs.swipeLeft)
        {
            playerModel.MoveLeft();
        }
        else if (Inputs.swipeRight)
        {
            playerModel.MoveRight();
        }
#endif
        playerModel.LerpPosition(currentPosition);
        playerModel.MoveForward();
    }

    public void IncreasePlayerStrengh() => currentStrengh++;
    public void DecreasePlayerStrengh() => currentStrengh--;


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6) // end collider layer
        {
            StartCoroutine(playerModel.ReduceSpeedRoutine());
        }
    }
}
