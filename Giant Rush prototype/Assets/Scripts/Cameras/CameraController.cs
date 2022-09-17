using Andrius.Core.Debuging;
using Andrius.Core.Utils;
using System;
using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static Action onEndReached;

    [SerializeField] float cameraFallowSpeed;
    [SerializeField] Vector3 cameraOffset;
    [ReadOnly, SerializeField] PlayerController player;
    [ReadOnly, SerializeField] Transform target;
    [ReadOnly, SerializeField] Transform endCameraTransform;

    [Header("End Reached camera functionality")]
    public float cameraLerpSpeed = 5f;
    bool endReached = false;

    void LateUpdate()
    {
        if (player == null || endReached) return;
        transform.position = Vector3.Lerp(transform.position, target.transform.position + cameraOffset, cameraFallowSpeed * Time.deltaTime);
    }

    void SetTarget(PlayerController player)
    {
        this.player = player;
        target = StaticFunctions.FindChild(player.transform, "Target");
        endCameraTransform = StaticFunctions.FindChild(player.transform, "CameraEndPosition");
    }
    void MoveAndRotateCamera() => StartCoroutine(MoveCameraRoutine());
    IEnumerator MoveCameraRoutine()
    {
        endReached = true;
        while (transform.position != endCameraTransform.position && transform.rotation != endCameraTransform.rotation)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, endCameraTransform.rotation, cameraLerpSpeed * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, endCameraTransform.position, cameraLerpSpeed * Time.deltaTime);
            yield return null;
        }
    }

    void OnEnable()
    {
        PlayerController.onPlayerSpawn += SetTarget;
        onEndReached += MoveAndRotateCamera;
    }

    void OnDisable()
    {
        PlayerController.onPlayerSpawn -= SetTarget;
        onEndReached -= MoveAndRotateCamera;

    }
}
