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

    List<BodyColorTypes> bodyColorList = new List<BodyColorTypes>() { BodyColorTypes.Yellow, BodyColorTypes.Green, BodyColorTypes.Red };
    List<BodyColorTypes> pickedColors = new List<BodyColorTypes>();

    bool regenerating = false;

    void Start()
    {
        levelGenerator = LevelGenerator.Instance;
        StartCoroutine(SpawnClone());

    }

    public IEnumerator SpawnClone()
    {
        var startPossition = Vector3.zero + spawnOffset;
        var firstClonePosition = new Vector3(spawnPositions[0], 0.579f, startPossition.z);
        var secondClonePosition = new Vector3(spawnPositions[1], 0.579f, startPossition.z);
        var thirdClonePosition = new Vector3(spawnPositions[2], 0.579f, startPossition.z);

        StartCoroutine(PickColor());
        while (regenerating)
        {
            yield return null;
        }
        var firstResult = CreateInitialObjects(firstClonePosition, firstColor);
        firstCloneObj = firstResult.obj;
        //SpanerPointer firstPointer = firstResult.pointer;

        var secondResult = CreateInitialObjects(secondClonePosition, secondColor);
        secondCloneObj = secondResult.obj;
        //SpanerPointer secondPointer = secondResult.pointer;

        var thirdResult = CreateInitialObjects(thirdClonePosition, thirdColor);
        thirdCloneObj = thirdResult.obj;
        //SpanerPointer thirdPointer = secondResult.pointer;

        var distance = (startPossition - levelGenerator.EndColider.position).magnitude;
        while (distance > stopDistance)
        {
            var fResult = CreateClone(0, firstCloneObj.transform.position, ref firstResult.obj, ref firstClonePos, firstColor); // first
            CreateClone(1, secondCloneObj.transform.position, ref secondResult.obj, ref secondClonePos, secondColor); // second
            CreateClone(2, thirdCloneObj.transform.position, ref thirdResult.obj, ref thirdClonePos, thirdColor); // third

            distance = (fResult.position - levelGenerator.EndColider.position).magnitude;

            //yield return null;
            yield return new WaitForSeconds(.1f);
        }
    }

    IEnumerator PickColor()
    {
        var fRandColor = Random.Range(0, bodyColorList.Count);
        firstColor = bodyColorList[fRandColor];
        pickedColors.Add(firstColor);
        var sRandColor = Random.Range(0, bodyColorList.Count);
        secondColor = bodyColorList[sRandColor];
        while (pickedColors.Contains(secondColor))
        {
            regenerating = true;
            sRandColor = Random.Range(0, bodyColorList.Count);
            secondColor = bodyColorList[sRandColor];
            yield return null;
        }
        pickedColors.Add(secondColor);
        if (pickedColors.Contains(BodyColorTypes.Yellow) && pickedColors.Contains(BodyColorTypes.Green))
        {
            thirdColor = BodyColorTypes.Red;
            pickedColors.Add(thirdColor);
        }
        else if (pickedColors.Contains(BodyColorTypes.Yellow) && pickedColors.Contains(BodyColorTypes.Red))
        {
            thirdColor = BodyColorTypes.Green;
            pickedColors.Add(thirdColor);
        }
        else if (pickedColors.Contains(BodyColorTypes.Green) && pickedColors.Contains(BodyColorTypes.Red))
        {
            thirdColor = BodyColorTypes.Yellow;
            pickedColors.Add(thirdColor);
        }
        regenerating = false;
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
        else
        {
            StartCoroutine(PickColor());
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
