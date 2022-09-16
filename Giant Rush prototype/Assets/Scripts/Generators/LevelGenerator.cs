using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] Transform longTrack;
    [SerializeField] GameObject changeColorPlatformPrefab;
    [SerializeField] EndCollider endCollider;
    [SerializeField] Transform bossCircle;

    // Move to database or somewhere else from where it can be easely changed not rebuilding the game
    [SerializeField] float scaleZ = 180;
    [SerializeField] int changeColorPlatformCount = 2;
    [SerializeField] float distanceBetweenPlatforms;
    GameObject previousePlatform;
    GameObject platform;

    void Start()
    {
        distanceBetweenPlatforms = scaleZ / (changeColorPlatformCount + 1);
        var scale = longTrack.localScale;
        scale.z = scaleZ;
        longTrack.localScale = scale;
        longTrack.position = new Vector3(0, 0, scaleZ / 2);

        for (int i = 0; i < changeColorPlatformCount; i++)
        {
            platform = Instantiate(changeColorPlatformPrefab);
            if (i == 0)
            {
                platform.transform.position = new Vector3(0, 0, distanceBetweenPlatforms);
                previousePlatform = platform;
                previousePlatform.transform.position = platform.transform.position;
            }
            else
            {
                platform.transform.position = new Vector3(0, 0, previousePlatform.transform.position.z + distanceBetweenPlatforms);
                previousePlatform = platform;
                previousePlatform.transform.position = platform.transform.position;
            }
        }
        endCollider.transform.position = new Vector3(0, 1.72f, scaleZ);
        bossCircle.position = new Vector3(0, 0.48f, scaleZ + 4f);
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;

    //    if (previousePlatform != null)
    //    {
    //        Gizmos.DrawWireSphere(previousePlatform.transform.position, 1f);
    //        Gizmos.DrawLine(previousePlatform.transform.position, platform.transform.position);
    //        var distance = (previousePlatform.transform.position - platform.transform.position).magnitude;
    //        Handles.color = Color.green;
    //        Handles.Label(new Vector3(0, 0, previousePlatform.transform.position.z + (distance / 2)), $"Distance: {distance}");
    //    }
    //}
}
