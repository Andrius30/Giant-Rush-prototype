using System.Collections.Generic;
using UnityEngine;

public enum BodyColorTypes
{
    Yellow,
    Green,
    Red
}

public class PlayerView : MonoBehaviour
{
    public BodyColorTypes currentBodyColor = BodyColorTypes.Yellow;

    [SerializeField] Animator animator;
    [SerializeField] List<Material> bodyMaterials;
    [SerializeField] SkinnedMeshRenderer bodyRenderer;

    public void SetIdle() => animator.SetTrigger("idle");
    public void SetRun() => animator.SetTrigger("run");
    // setBox

    public void ChangeColor(BodyColorTypes colorType)
    {
        switch (colorType)
        {
            case BodyColorTypes.Yellow:
                bodyRenderer.material = bodyMaterials[0];
                currentBodyColor = colorType;
                break;
            case BodyColorTypes.Green:
                bodyRenderer.material = bodyMaterials[1];
                currentBodyColor = colorType;
                break;
            case BodyColorTypes.Red:
                bodyRenderer.material = bodyMaterials[2];
                currentBodyColor = colorType;
                break;
        }
    }


}
