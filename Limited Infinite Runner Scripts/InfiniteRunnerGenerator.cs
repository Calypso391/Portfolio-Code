using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteRunnerGenerator : MonoBehaviour
{
    [Header("Tuning")]
    public float spawnNewInfiniteRunnerPieceVal;
    public Vector3 segmentSpawnVal;
    public float segmentsBetweenSpeedUp = 5;
    public float speedUpAmount = .2f;

    static InfiniteRunnerGenerator _instance;
    public static InfiniteRunnerGenerator Instance => _instance;
    public GameObject[] infiniteRunnerBits;

    [HideInInspector]
    public float currentSpeedMult = 1.0f;
    int numSegmentsPassed = 0;
    GameObject currentInfiniteRunnerPiece;
    float spawnDiff;
    private void Awake()
    {
        _instance = this;
         spawnDiff = segmentSpawnVal.x - spawnNewInfiniteRunnerPieceVal;
    }

    private void Start()
    {
        StartCoroutine(infiniteRunnerRoutine());
    }

    IEnumerator infiniteRunnerRoutine()
    {
        while(true)
        {
            numSegmentsPassed++;
            if(numSegmentsPassed >= segmentsBetweenSpeedUp)
            {
                numSegmentsPassed = 0;
                currentSpeedMult += speedUpAmount;
            }
            if(currentInfiniteRunnerPiece != null)
            {
                Destroy(currentInfiniteRunnerPiece, 10f);
            }
            currentInfiniteRunnerPiece = Instantiate(infiniteRunnerBits[Random.Range(0,infiniteRunnerBits.Length)],segmentSpawnVal, Quaternion.identity);
            currentInfiniteRunnerPiece.transform.localScale = new Vector3(currentSpeedMult, 1, 1);
            while (currentInfiniteRunnerPiece.transform.position.x > segmentSpawnVal.x - (spawnDiff * currentSpeedMult))
            {
                yield return null;
            }
        }
    }
}
