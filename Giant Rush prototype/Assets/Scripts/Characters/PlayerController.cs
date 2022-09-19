using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static Action<PlayerController> onPlayerSpawn;

    public float moveSpeed = 5f;
    public float lerpSpeed;
    public float reduceSpeedRatio = 0.3f;
    public PlayerView playerView; // controlls visuals

    [SerializeField] Camera mainCamera;
    [SerializeField] List<float> movePositions;

    public int currentStrengh = 0; // FIXME: to property after debuging done
    public int currentPositionIndex { get; set; }
    public List<float> MovePositions => movePositions;

    public PlayerModel playerModel { get; private set; }
    public Inputs Inputs { get; private set; }
    bool endReached;

    void Start()
    {
        onPlayerSpawn?.Invoke(this);
        playerModel = new PlayerModel(this);
        Inputs = new Inputs();
        endReached = false;
    }
    void Update()
    {
        // When boss area reached disable these inputs and enable other type of inputs for example: Doge inputs ( Left/Right, Tap screen to attack, ... )
        Vector3 currentPosition = transform.position;
#if UNITY_EDITOR
        if (!endReached && Inputs.LeftInput())
        {
            playerModel.MoveLeft();
        }
        else if (!endReached && Inputs.RightInput())
        {
            playerModel.MoveRight();
        }
        else if (endReached && Inputs.LeftMouseInputDown())
        {
            playerView.PlayBoxing();
        }
#endif
#if UNITY_ANDROID
        Inputs.DetectTouches();
        if (!endReached && Inputs.swipeLeft)
        {
            playerModel.MoveLeft();
        }
        else if (!endReached && Inputs.swipeRight)
        {
            playerModel.MoveRight();
        }
        else if (endReached && Inputs.isTouching)
        {
            playerView.PlayBoxing();
        }
        else if (endReached && !Inputs.isTouching)
        {
            playerView.SetBoxingIdle();
        }
#endif
        if (!endReached)
        {
            playerModel.LerpPosition(currentPosition);
            playerModel.MoveForward();
        }
    }

    void OnEndReached() => endReached = true;

    void OnEnable()
    {
        EndCollider.onEndReached += OnEndReached;
    }
    void OnDisable()
    {
        EndCollider.onEndReached -= OnEndReached;

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6) // end collider layer
        {
            StartCoroutine(playerModel.ReduceSpeedRoutine());
        }
    }
}
