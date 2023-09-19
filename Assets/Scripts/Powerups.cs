using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerups : MonoBehaviour
{
    public GameObject[] powerups;
    public float spawnTime = 5f;
    public int maxPowerups = 6;

    private BoxCollider2D spawnArea;
    public static int powerupsCount = 0;
    private List<GameObject> spawnedPowerups = new List<GameObject>();

    private void Start()
    {
        spawnArea = GetComponent<BoxCollider2D>();
        InvokeRepeating("SpawnPowerup", spawnTime, spawnTime);
    }

    private void SpawnPowerup()
    {
        //Debug.Log("pc" + powerupsCount);
        if (powerupsCount >= maxPowerups)
        {
            return;
        }
  
        // Generate a random position within the spawn area
        float x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
        float y = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);
        Vector3 spawnPosition = new Vector3(x, y, 0f);


        // Choose a random powerup from the powerups array
        int index = Random.Range(0, powerups.Length);
        GameObject powerupPrefab = powerups[index];


        // Instantiate the powerup prefab
        GameObject powerup = Instantiate(powerupPrefab, spawnPosition, Quaternion.identity);
        spawnedPowerups.Add(powerup);

        powerupsCount++;
    }

}