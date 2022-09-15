using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorOnTrigger : MonoBehaviour
{
    List<BodyColorTypes> bodyColorTypes = new List<BodyColorTypes>() { BodyColorTypes.Yellow, BodyColorTypes.Green, BodyColorTypes.Red };

    void OnTriggerEnter(Collider other)
    {
        PlayerView view = other.GetComponent<PlayerView>();
        if (view != null)
        {
           StartCoroutine(RegenerateColorRoutine(view));
        }
    }

    IEnumerator RegenerateColorRoutine(PlayerView view)
    {
        int randomColor = Random.Range(0, bodyColorTypes.Count);
        var nextColor = bodyColorTypes[randomColor];
        while (nextColor == view.currentBodyColor)
        {
            randomColor = Random.Range(0, bodyColorTypes.Count);
            nextColor = bodyColorTypes[randomColor];
            yield return null;
        }
        view.ChangeColor(nextColor);
    }

}
