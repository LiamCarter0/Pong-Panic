using UnityEngine;
using System.Collections;

public class Brick_Spawner : MonoBehaviour
{
    public GameObject brickPrefab; // Assign the brick prefab in the Inspector
    public float spawnInterval = 3f; // Time interval between spawns in seconds
    public Vector3 defaultPosition = new Vector3(0, 3.57f, 0); // Default spawn position

    private void Start()
    {
        // Start spawning bricks at intervals
        StartCoroutine(SpawnBricks());
    }

    private IEnumerator SpawnBricks()
    {
        while (true)
        {
            // Spawn a new brick with a random x-offset of ±1 from the default position
            Vector3 spawnPosition = defaultPosition + new Vector3(Random.Range(-2f, 2f), 0, 0);
            Instantiate(brickPrefab, spawnPosition, Quaternion.identity);

            // Wait for the next spawn interval
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
