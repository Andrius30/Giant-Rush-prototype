using UnityEngine;

public class SpanerPointer : MonoBehaviour
{
    public int ID;
    public bool canSpawn = true;


    void OnTriggerEnter(Collider other)
    {
        ChangeColorOnTrigger color = other.GetComponent<ChangeColorOnTrigger>();
        if (color != null)
        {
            canSpawn = false;
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
