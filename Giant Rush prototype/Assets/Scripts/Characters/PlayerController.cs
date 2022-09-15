using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float lerpSpeed;
    public float cameraLerpSpeed = 5f;
    public float reduceSpeedRatio = 0.3f;

    [SerializeField] Camera mainCamera;
    [SerializeField] Transform endCameraTransform;
    [SerializeField] List<float> movePositions;

    [SerializeField] PlayerView playerView; // controlls visuals
    int currentPositionIndex = 0;


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

    void MoveForward() => transform.Translate(transform.forward * moveSpeed * Time.deltaTime);

    IEnumerator ReduceSpeedRoutine()
    {
        StartCoroutine(MoveCameraRoutine());
        while (moveSpeed > 0)
        {
            moveSpeed -= reduceSpeedRatio;
            yield return null;
        }
        moveSpeed = 0;
        playerView.SetIdle();
        // start boss fight
    }

    IEnumerator MoveCameraRoutine()
    {
        while (mainCamera.transform.position != endCameraTransform.position && mainCamera.transform.rotation != endCameraTransform.rotation)
        {
            mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, endCameraTransform.rotation, cameraLerpSpeed * Time.deltaTime);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, endCameraTransform.position, cameraLerpSpeed * Time.deltaTime);
            yield return null;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6) // end collider layer
        {
            StartCoroutine(ReduceSpeedRoutine());
        }
    }
}
