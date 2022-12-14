using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator Instance;

    public Transform LongTrack => longTrack;
    public Transform EndColider => endCollider.transform;
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
    public bool Initialized;

    void Awake()
    {
        Instance = this;
    }
    Vector3 lastPlatformPosition;

    void Start()
    {
        Initialized = false;
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
                if(i == changeColorPlatformCount - 1)
                {
                    lastPlatformPosition = platform.transform.position; 
                    ChangeColorOnTrigger changeColorOnTrigger = platform.GetComponentInChildren<ChangeColorOnTrigger>();
                    changeColorOnTrigger.isLastPlatform = true;

                }
            }
        }
        endCollider.transform.position = new Vector3(0, 1.72f, scaleZ);
        bossCircle.position = new Vector3(0, 0.48f, scaleZ + 4f);
        Initialized = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (lastPlatformPosition != null)
        {
            Gizmos.DrawWireSphere(lastPlatformPosition, 5f);
        }
    }
}
