using System.Collections.Generic;
using UnityEngine;

public class Clone : MonoBehaviour
{
    public BodyColorTypes bodyColor;
    public List<Material> bodyMaterials;

    [SerializeField] SkinnedMeshRenderer skinnedMeshRenderer;

    public void SetColor(BodyColorTypes bodyColor)
    {
        switch (bodyColor)
        {
            case BodyColorTypes.Yellow:
                this.bodyColor = BodyColorTypes.Yellow;
                skinnedMeshRenderer.material = bodyMaterials[0];
                break;
            case BodyColorTypes.Green:
                this.bodyColor = BodyColorTypes.Green;
                skinnedMeshRenderer.material = bodyMaterials[1];
                break;
            case BodyColorTypes.Red:
                this.bodyColor = BodyColorTypes.Red;
                skinnedMeshRenderer.material = bodyMaterials[2];
                break;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            PlayerController controller = other.GetComponent<PlayerController>();
            if (controller.playerView.currentBodyColor == bodyColor)
            {
                controller.playerView.IncreasePlayerScale();
                controller.IncreasePlayerStrengh();
                controller.UpdateTargetPosition();
                Destroy(gameObject);
            }
            else
            {
                controller.playerView.DecreasePlayerScale();
                controller.DecreasePlayerStrengh();
                controller.UpdateTargetPosition();
            }
        }
    }
}
