using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour  // NetworkBehaviour deðil
{
    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    private List<int> usedSpawnIndices = new List<int>();

    public static SpawnManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Vector3 GetSpawnPosition(int playerId)
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points defined! Using default position.");
            return Vector3.zero;
        }

        int spawnIndex = GetAvailableSpawnIndex();
        usedSpawnIndices.Add(spawnIndex);

        Debug.Log($"Player {playerId} assigned to spawn point {spawnIndex}");
        return spawnPoints[spawnIndex].position;
    }

    int GetAvailableSpawnIndex()
    {
        if (usedSpawnIndices.Count >= spawnPoints.Length)
        {
            usedSpawnIndices.Clear();
        }

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (!usedSpawnIndices.Contains(i))
            {
                return i;
            }
        }

        return 0;
    }
}