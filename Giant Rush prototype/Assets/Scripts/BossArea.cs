using UnityEngine;

public class BossArea : MonoBehaviour
{



    void OnTriggerEnter(Collider collision)
    {
        Clone clone = collision.gameObject.GetComponent<Clone>();
        if (clone != null)
        {
            Destroy(clone.gameObject); // in case clones were spawned at boss area
        }
    }
}
