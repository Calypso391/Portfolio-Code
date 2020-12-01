using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteRunnerGenerator : MonoBehaviour
{
    static InfiniteRunnerGenerator _instance;
    public static InfiniteRunnerGenerator Instance => _instance;
    [Header("Tuning Spawning")]
    public float SpawnNewPrefabXCoord;
    public Vector3 NewSegmentSpawnValue;
    [Header("Tuning Combo")]
    [HideInInspector]
    public float currentSpeed;
    public float minSpeed;
    public float maxSpeed;
    public int ComboRequiredForSpeedUp = 5;
    public int ComboLostOnHit = 5;  

    [Header("References")]
    public SerializedArray[] infiniteRunnerBits = new SerializedArray[5];
    public BackgroundAppear[] backgroundComboAppear;
    public Transform BustSpawnLocation;
    public GameObject BustPrefab;
    [HideInInspector]
    public float currentSpeedMult = 1.0f;
    GameObject currentInfiniteRunnerPiece;
    int CurrentCombo = 0;
    public int currentComboLevel = 0;
    private void Awake()
    {
        _instance = this;
    }

    int totalSpeedSegments;
    private void Start()
    {
        StartCoroutine(GenerateInfiniteRunnerRoutine());
        currentSpeed = minSpeed;
        totalSpeedSegments = ((infiniteRunnerBits.Length-1) * ComboRequiredForSpeedUp);
    }
    bool hitThisPrefab = false;
    public void PlayerHit()
    {
        CurrentCombo -= ComboLostOnHit;
        if (CurrentCombo <= 0)
        {
            if (currentComboLevel > 0)
            {
                backgroundComboAppear[currentComboLevel].Disappear();
                currentComboLevel--;
                CurrentCombo = ComboRequiredForSpeedUp - 1;
            }
            else
            {
                CurrentCombo = 0;
            }
            currentSpeed = Mathf.Lerp(minSpeed, maxSpeed, currentComboLevel + 1 / infiniteRunnerBits.Length);
            currentSpeedMult = currentSpeed / minSpeed;
        }
        hitThisPrefab = true;
    }

    IEnumerator GenerateInfiniteRunnerRoutine()
    {
        while(true)
        {
            if (currentInfiniteRunnerPiece != null)
            {
                Destroy(currentInfiniteRunnerPiece, 10f);
            }
            int randomPiece = Random.Range(0, infiniteRunnerBits[currentComboLevel].BucketPrefabs.Length - 1);
            currentInfiniteRunnerPiece = Instantiate(infiniteRunnerBits[currentComboLevel].BucketPrefabs[randomPiece] ,NewSegmentSpawnValue, Quaternion.identity);
            while (currentInfiniteRunnerPiece.transform.position.x > SpawnNewPrefabXCoord)
            {
                yield return null;
            }
            if(!hitThisPrefab)
                CurrentCombo++;

            hitThisPrefab = false;

            if (CurrentCombo >= ComboRequiredForSpeedUp)
            {
                CurrentCombo = 0;
                currentComboLevel++;
                if (currentComboLevel >= infiniteRunnerBits.Length) currentComboLevel = infiniteRunnerBits.Length - 1;
                yield return StartCoroutine(DoNewComboRoutine());
                backgroundComboAppear[currentComboLevel].gameObject.SetActive(true);
                backgroundComboAppear[currentComboLevel].Appear();
            }
            int numSegment = (currentComboLevel * ComboRequiredForSpeedUp) + CurrentCombo;
            currentSpeed = Mathf.Lerp(minSpeed, maxSpeed, (float)numSegment / totalSpeedSegments);
            Debug.Log(currentSpeed);
            if (currentSpeed > maxSpeed)
                currentSpeed = maxSpeed;
            currentSpeedMult = currentSpeed / minSpeed;
        }
    }
    IEnumerator DoNewComboRoutine()
    {
        yield return new WaitForSeconds(1.0f);
        for(int i = 0; i<currentComboLevel; i++)
        {
            currentInfiniteRunnerPiece = Instantiate(BustPrefab, BustSpawnLocation.transform.position, BustPrefab.transform.rotation);
            while (currentInfiniteRunnerPiece.transform.position.x > SpawnNewPrefabXCoord + 10)
            {
                yield return null;
            }
            Destroy(currentInfiniteRunnerPiece);
        }
        currentInfiniteRunnerPiece = null;
    }

    [System.Serializable]
    public class SerializedArray
    {
        public GameObject[] BucketPrefabs;
    }

}
