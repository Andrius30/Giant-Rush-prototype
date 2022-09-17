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

    [SerializeField] int currentStrengh = 0;
    [SerializeField] Camera mainCamera;
    [SerializeField] List<float> movePositions;
    [SerializeField] Transform cameraTarget;
    public PlayerView playerView; // controlls visuals
    int currentPositionIndex = 0;

    void Start()
    {
        onPlayerSpawn?.Invoke(this);
    }

    void Update()
    {
        Vector3 currentPosition = transform.position;
        if (Input.GetKeyDown(KeyCode.A))
        {
            currentPositionIndex--;
            if (currentPositionIndex <= 0)
            {
                currentPositionIndex = 0;
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            currentPositionIndex++;
            if (currentPositionIndex >= movePositions.Count - 1)
            {
                currentPositionIndex = movePositions.Count - 1;
            }
        }
        currentPosition.x = movePositions[currentPositionIndex];
        transform.position = Vector3.Lerp(transform.position, currentPosition, lerpSpeed * Time.deltaTime);
        MoveForward();
    }

    public void IncreasePlayerStrengh() => currentStrengh++;
    public void DecreasePlayerStrengh() => currentStrengh--;
    public void UpdateTargetPosition()
    {
        var currentScale = transform.localScale / 2;
        cameraTarget.transform.localPosition = new Vector3(0, currentScale.y, -currentScale.z);
    }

    void MoveForward() => transform.Translate(transform.forward * moveSpeed * Time.deltaTime);
    IEnumerator ReduceSpeedRoutine()
    {
        CameraController.onEndReached?.Invoke();
        while (moveSpeed > 0)
        {
            moveSpeed -= reduceSpeedRatio;
            yield return null;
        }
        moveSpeed = 0;
        playerView.SetIdle();
        // start boss fight
    }



    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6) // end collider layer
        {
            StartCoroutine(ReduceSpeedRoutine());
        }
    }
}
