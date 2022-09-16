using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSpawners : MonoBehaviour
{
    [SerializeField] Clone clonePrefab;
    [SerializeField] List<float> spawnPositions;
    [SerializeField] Vector3 spawnOffset;
    [SerializeField] float distanceBetweenClones = 5f;
    [SerializeField] float stopDistance = 10f;

    LevelGenerator levelGenerator;
    Vector3 firstClonePos;
    Vector3 secondClonePos;
    Vector3 thirdClonePos;
    GameObject firstCloneObj;
    GameObject secondCloneObj;
    GameObject thirdCloneObj;
    static int lastID;

    BodyColorTypes firstColor;
    BodyColorTypes secondColor;
    BodyColorTypes thirdColor;

    Dictionary<int, List<BodyColorTypes>> colorMap = new Dictionary<int, List<BodyColorTypes>>();
    List<BodyColorTypes> colorPattern_1 = new List<BodyColorTypes>() { BodyColorTypes.Yellow, BodyColorTypes.Green, BodyColorTypes.Red };
    List<BodyColorTypes> colorPattern_2 = new List<BodyColorTypes>() { BodyColorTypes.Green, BodyColorTypes.Yellow, BodyColorTypes.Red };
    List<BodyColorTypes> colorPattern_3 = new List<BodyColorTypes>() { BodyColorTypes.Green, BodyColorTypes.Red, BodyColorTypes.Yellow };

    void Start()
    {
        levelGenerator = LevelGenerator.Instance;
        colorMap.Add(0, colorPattern_1);
        colorMap.Add(1, colorPattern_2);
        colorMap.Add(2, colorPattern_3);
        StartCoroutine(SpawnClone());
    }
    void PickRandomColorPattern()
    {
        int random = Random.Range(0, colorMap.Count);
        List<BodyColorTypes> colorPattern = colorMap[random];
        for (int i = 0; i < colorPattern.Count; i++)
        {
            if (i == 0)
            {
                firstColor = colorPattern[i];
            }
            else if (i == 1)
            {
                secondColor = colorPattern[i];
            }
            else
            {
                thirdColor = colorPattern[i];
            }
        }
    }

    public IEnumerator SpawnClone()
    {
        var startPossition = Vector3.zero + spawnOffset;
        var firstClonePosition = new Vector3(spawnPositions[0], 0.579f, startPossition.z);
        var secondClonePosition = new Vector3(spawnPositions[1], 0.579f, startPossition.z);
        var thirdClonePosition = new Vector3(spawnPositions[2], 0.579f, startPossition.z);

        var firstResult = CreateInitialObjects(firstClonePosition, firstColor);
        firstCloneObj = firstResult.obj;

        var secondResult = CreateInitialObjects(secondClonePosition, secondColor);
        secondCloneObj = secondResult.obj;

        var thirdResult = CreateInitialObjects(thirdClonePosition, thirdColor);
        thirdCloneObj = thirdResult.obj;
        PickRandomColorPattern();
        var distance = (startPossition - levelGenerator.EndColider.position).magnitude;
        while (distance > stopDistance)
        {
            var fResult = CreateClone(0, firstCloneObj.transform.position, ref firstResult.obj, ref firstClonePos, firstColor); // first
            if (!fResult.pointer.canSpawn)
            {
                PickRandomColorPattern();
            }
            if (fResult.pointer.isLastPlatformDetected)
            {
                // randomize spawn
                Debug.Log($"Randomize colors");
            }
            CreateClone(1, secondCloneObj.transform.position, ref secondResult.obj, ref secondClonePos, secondColor); // second
            CreateClone(2, thirdCloneObj.transform.position, ref thirdResult.obj, ref thirdClonePos, thirdColor); // third

            distance = (fResult.position - levelGenerator.EndColider.position).magnitude;

            //yield return null;
            yield return new WaitForSeconds(.1f);
        }
    }

    (Vector3 position, SpanerPointer pointer) CreateClone(int index, Vector3 objPosition, ref GameObject cloneObj, ref Vector3 objPos, BodyColorTypes colorType)
    {
        var position = new Vector3(spawnPositions[index], 0.579f, objPosition.z + distanceBetweenClones);
        cloneObj.transform.position = position;
        var pointer = cloneObj.GetComponent<SpanerPointer>();
        if (pointer.canSpawn)
        {
            Clone clone = Instantiate(clonePrefab, cloneObj.transform.position, Quaternion.identity);
            clone.SetColor(colorType);
        }
        objPos = cloneObj.transform.position;
        return (position, pointer);
    }
    (GameObject obj, SpanerPointer pointer) CreateInitialObjects(Vector3 firstClonePosition, BodyColorTypes colorType)
    {
        Clone clone = null;
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        var rb = obj.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        SpanerPointer pointer = obj.AddComponent<SpanerPointer>();
        pointer.ID = lastID;
        lastID++;
        if (pointer.canSpawn)
        {
            obj.transform.position = firstClonePosition;
            clone = Instantiate(clonePrefab, obj.transform.position, Quaternion.identity);
            clone.SetColor(colorType);
        }

        return (obj, pointer);
    }
}
