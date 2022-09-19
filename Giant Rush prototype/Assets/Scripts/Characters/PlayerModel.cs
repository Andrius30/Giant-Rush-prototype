using System.Collections;
using UnityEngine;

public class PlayerModel
{
    PlayerController controller;

    public PlayerModel(PlayerController controller)
    {
        this.controller = controller;
    }
    public void MoveLeft()
    {
        controller.currentPositionIndex--;
        if (controller.currentPositionIndex <= 0)
        {
            controller.currentPositionIndex = 0;
        }
    }
    public void MoveRight()
    {
        controller.currentPositionIndex++;
        if (controller.currentPositionIndex >= controller.MovePositions.Count - 1)
        {
            controller.currentPositionIndex = controller.MovePositions.Count - 1;
        }
    }
    public void LerpPosition(Vector3 currentPosition)
    {
        currentPosition.x = controller.MovePositions[controller.currentPositionIndex];
        controller.transform.position = Vector3.Lerp(controller.transform.position, currentPosition, controller.lerpSpeed * Time.deltaTime);
    }
    public void MoveForward()
    {
        controller.transform.Translate(controller.transform.forward * controller.moveSpeed * Time.deltaTime);
        controller.playerView.SetRun();
    }
    public IEnumerator ReduceSpeedRoutine()
    {
        CameraController.onEndReached?.Invoke();
        while (controller.moveSpeed > 0)
        {
            controller.moveSpeed -= controller.reduceSpeedRatio;
            yield return null;
        }
        controller.moveSpeed = 0;
        // start boss fight
    }
    public void IncreasePlayerStrengh() => controller.currentStrengh++;
    public void DecreasePlayerStrengh() => controller.currentStrengh--;
}
