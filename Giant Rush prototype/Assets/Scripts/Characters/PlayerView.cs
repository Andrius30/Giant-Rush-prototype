using System;
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

    public void SetRun() => animator.SetTrigger("run");
    public void PlayBoxingIdle() => animator.SetTrigger("boxingIdle");
    public void PlayBoxing() => animator.SetTrigger("boxing");
    public void SetBoxingIdle() => animator.SetTrigger("boxingIdle");

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

    public void IncreasePlayerScale()
    {
        transform.root.localScale = new Vector3(transform.root.localScale.x + .05f, transform.root.localScale.y + .05f, transform.root.localScale.z + .05f);
    }
    public void DecreasePlayerScale()
    {
        transform.root.localScale = new Vector3(transform.root.localScale.x - .05f, transform.root.localScale.y - .05f, transform.root.localScale.z - .05f);
    }

}
