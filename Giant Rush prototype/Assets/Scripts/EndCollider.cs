using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCollider : MonoBehaviour
{
    public static Action onEndReached;

    [SerializeField] Transform playerPosition;
    [SerializeField] float moveToPositionSpeed = 5f;
    [SerializeField] float stopDistance = 1f;

    IEnumerator LerpToPosition(PlayerController player)
    {
        var distance = (player.transform.position - playerPosition.position).magnitude;
        while (distance > stopDistance)
        {
            distance = (player.transform.position - playerPosition.position).magnitude;
            player.transform.position = Vector3.Lerp(player.transform.position, playerPosition.position, moveToPositionSpeed * Time.deltaTime);
            yield return null;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8) // 8 layer == player
        {
            onEndReached?.Invoke();
            PlayerController player = other.GetComponent<PlayerController>();
            StartCoroutine(LerpToPosition(player));
        }
    }
}
