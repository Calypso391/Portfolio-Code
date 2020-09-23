using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public GameObject enemy;
    public int xPos;
    public int zPos;
    public static int enemyCount;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemyDrop());
    }

    IEnumerator EnemyDrop() {
        while(true) {
            xPos = Random.Range(-20, 20);
            zPos = Random.Range(-20, 20);
            while (xPos < 10 && xPos > -10){
                xPos = Random.Range(-20, 20);
            }
            while (zPos < 1 && zPos > -1){
                zPos = Random.Range(-20, 20);
            }
            Instantiate(enemy, new Vector3(xPos, 0, zPos), Quaternion.identity);
            yield return new WaitForSeconds(0.05f);
            enemyCount += 1;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
