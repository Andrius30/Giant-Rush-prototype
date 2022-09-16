using UnityEngine;

public class SpanerPointer : MonoBehaviour
{
    public int ID;
    public bool canSpawn = true;
    public bool isLastPlatformDetected;

    void OnTriggerEnter(Collider other)
    {
        ChangeColorOnTrigger color = other.GetComponent<ChangeColorOnTrigger>();
        if (color != null)
        {
            canSpawn = false;
        }
        if (color.isLastPlatform)
        {
            isLastPlatformDetected = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        ChangeColorOnTrigger color = other.GetComponent<ChangeColorOnTrigger>();
        if (color != null)
        {
            canSpawn = true;
        }
    }
}
