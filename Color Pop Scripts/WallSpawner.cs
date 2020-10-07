using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class WallSpawner : MonoBehaviour
{
    public GameObject WallPrefab = null;
    private PlayerController playerController;
    [HideInInspector] public List<SpawnDescription> Description = new List<SpawnDescription>();

    [Serializable]
    public class SpawnDescription
    {
        public float TimeAfterPrevious = 0.0f;
        public EColorFlag AllowedColors = EColorFlag.None;
        public int totalAllowed = 1;
        public int blocksInRound = 1;
        public float TimeAfterAround = 0.0f;
        public float blockSpeed = 1.0f;
        public string DeveloperComment;
    }

    private float perItemTimer = 0.0f;
    private float roundAfterTimer = 0.0f;
    private int indexInDescription = 0;
    private static int prevIndex = 0;
    private SpawnDescription currentSpawn = null;
    private float currBlock = 0;

    private void Start()
    {
        if (Description.Count > 0)
        {
            currentSpawn = Description[0];
        }
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        SetUpAllowedColors();
    }

    private void Update()
    {
        perItemTimer += Time.deltaTime;

        bool shouldIterate = indexInDescription < Description.Count && perItemTimer > currentSpawn.TimeAfterPrevious && currBlock < currentSpawn.blocksInRound;
        if (shouldIterate)
        {
            perItemTimer = 0.0f;
            currBlock++;

            bool shouldSpawn = currentSpawn.AllowedColors != EColorFlag.None;
            if (shouldSpawn)
            {
                GameObject wall = Instantiate(WallPrefab, transform.position, Quaternion.identity, null);
                var comp = wall.GetComponent<Wall>();
                comp.color = EColorRGBHelper.GetRandomFlagFromFlags(currentSpawn.AllowedColors);
                comp.Speed = currentSpawn.blockSpeed;
            }
        }

        else if(currBlock >= currentSpawn.blocksInRound)
        {
            roundAfterTimer += Time.deltaTime;
            if (indexInDescription == 0 && !AudioManager.bgmPlaying && roundAfterTimer > currentSpawn.TimeAfterAround / 2)
            {
                AudioManager.StartBGM();
            }
            if (roundAfterTimer > currentSpawn.TimeAfterAround)
            {
                roundAfterTimer = 0.0f;
                currBlock = 0;
                prevIndex = indexInDescription;
                if(indexInDescription < Description.Count - 1) indexInDescription++;
                currentSpawn = Description[indexInDescription];

                if(prevIndex != indexInDescription) AudioManager.PlayVoice(indexInDescription);

                SetUpAllowedColors();
            }
        }
    }

    public void SetUpAllowedColors()
    {
        playerController.ShuffleBindings(currentSpawn.totalAllowed);

        currentSpawn.AllowedColors = EColorFlag.None;
        //Get the top x colors for allowed
        for(int i = 0; i < currentSpawn.totalAllowed; i++)
        {
            currentSpawn.AllowedColors |= playerController.colors[UnityEngine.Random.Range(0, currentSpawn.totalAllowed)];
        }
    }
}
