using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSpawners : MonoBehaviour
{
    [SerializeField] Clone clonePrefab;
    [SerializeField] List<float> spawnPositions;
    [SerializeField] float spawnOffset;
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

    void Start()
    {
        levelGenerator = LevelGenerator.Instance;
        StartCoroutine(SpawnClone());

    }

    public IEnumerator SpawnClone()
    {
        var startPossition = levelGenerator.LongTrack.transform.position;

        var distance = (startPossition - levelGenerator.EndColider.position).magnitude;

        var firstClonePosition = new Vector3(spawnPositions[0], 0.579f, levelGenerator.LongTrack.position.z + spawnOffset);
        var secondClonePosition = new Vector3(spawnPositions[1], 0.579f, levelGenerator.LongTrack.position.z + spawnOffset);
        var thirdClonePosition = new Vector3(spawnPositions[2], 0.579f, levelGenerator.LongTrack.position.z + spawnOffset);

        var firstResult = CreateInitialObjects(firstClonePosition, BodyColorTypes.Yellow);
        firstCloneObj = firstResult.obj;
        SpanerPointer firstPointer = firstResult.pointer;

        var secondResult = CreateInitialObjects(secondClonePosition, BodyColorTypes.Green);
        secondCloneObj = secondResult.obj;
        SpanerPointer secondPointer = secondResult.pointer;

        var thirdResult = CreateInitialObjects(thirdClonePosition, BodyColorTypes.Red);
        thirdCloneObj = thirdResult.obj;
        SpanerPointer thirdPointer = secondResult.pointer;

        while (distance > stopDistance)
        {
            var fResult = CreateClone(0, firstCloneObj.transform.position, ref firstResult.obj, ref firstClonePos, BodyColorTypes.Yellow); // first
            var sResult = CreateClone(1, secondCloneObj.transform.position, ref secondResult.obj, ref secondClonePos, BodyColorTypes.Green); // second
            var tResult = CreateClone(2, thirdCloneObj.transform.position, ref thirdResult.obj, ref thirdClonePos, BodyColorTypes.Red); // third

            distance = (fResult.position - levelGenerator.EndColider.position).magnitude;

            //yield return new WaitForSeconds(.3f);
            yield return null;
        }
    }

    (Vector3 position, SpanerPointer pointer) CreateClone(int index, Vector3 objPosition, ref GameObject cloneObj, ref Vector3 objPos, BodyColorTypes colorType)
    {
        var position = new Vector3(spawnPositions[index], 0.579f, objPosition.z + distanceBetweenClones);
        cloneObj.transform.position = position;
        var pointer = cloneObj.GetComponent<SpanerPointer>();
        if (pointer.canSpawn)
        {
            Clone firstClone = Instantiate(clonePrefab, cloneObj.transform.position, Quaternion.identity);
            firstClone.SetColor(colorType);
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
