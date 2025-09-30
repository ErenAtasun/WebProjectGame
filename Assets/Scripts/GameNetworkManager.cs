using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class GameNetworkManager : NetworkManager
{
    [Header("Scene Settings")]
    public string gameSceneName = "GameScene";
    private Dictionary<NetworkConnectionToClient, PlayerMatchData> playerMatchData = new Dictionary<NetworkConnectionToClient, PlayerMatchData>();

    [System.Serializable]
    public class PlayerMatchData
    {
        public int matchId;
        public int characterId;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        NetworkClient.RegisterHandler<MatchFoundMessage>(OnMatchFoundMessage);
        Debug.Log("Client message handlers registered");
    }

    public override void OnStopClient()
    {
        NetworkClient.UnregisterHandler<MatchFoundMessage>();
        base.OnStopClient();
    }

    void OnMatchFoundMessage(MatchFoundMessage msg)
    {
        Debug.Log($"Received MatchFoundMessage: Match ID {msg.matchId}");

        MainMenuUI menuUI = FindObjectOfType<MainMenuUI>();
        if (menuUI != null)
        {
            menuUI.OnMatchFoundReceived(msg);
        }
    }

    public void JoinQueue()
    {
        Debug.Log($"JoinQueue called. NetworkClient.isConnected: {NetworkClient.isConnected}");

        if (NetworkClient.isConnected)
        {
            Debug.Log("Sending QueueJoinMessage to server");
            NetworkClient.Send(new QueueJoinMessage());
        }
        else
        {
            Debug.Log("Not connected, starting client...");
            StartClient();
        }
    }

    // YENÝ METOD
    public void LoadGameSceneForMatch(int matchId)
    {
        if (NetworkServer.active)
        {
            Debug.Log($"Server loading game scene for match {matchId}");
            ServerChangeScene(gameSceneName);
        }
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        Debug.Log("Client connected to server");
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("GameNetworkManager server started");
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);

        if (sceneName == gameSceneName)
        {
            Debug.Log("Game scene loaded on server, re-spawning players...");

            // Tüm baðlý oyuncular için player spawn et
            foreach (var conn in NetworkServer.connections.Values)
            {
                if (conn.identity == null) // Henüz player'ý yoksa
                {
                    OnServerAddPlayer(conn);
                }
            }
        }
    }


    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        Debug.Log($"OnServerAddPlayer called for connection {conn.connectionId}");

        // Spawn pozisyonu al
        Vector3 spawnPos = Vector3.zero;
        if (SpawnManager.Instance != null)
        {
            spawnPos = SpawnManager.Instance.GetSpawnPosition(conn.connectionId);
            Debug.Log($"Spawn position: {spawnPos}");
        }
        else
        {
            Debug.LogWarning("SpawnManager.Instance is null!");
        }

        // Player'ý spawn et
        if (playerPrefab == null)
        {
            Debug.LogError("Player Prefab is null! Set it in GameNetworkManager Inspector!");
            return;
        }

        GameObject player = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
        Debug.Log($"Player instantiated at {spawnPos}");

        // Character bilgilerini ata
        PlayerController controller = player.GetComponent<PlayerController>();
        if (controller != null && playerMatchData.ContainsKey(conn))
        {
            controller.characterId = playerMatchData[conn].characterId;
            controller.matchId = playerMatchData[conn].matchId;
            Debug.Log($"Player spawned with character {controller.characterId} for match {controller.matchId}");
        }

        NetworkServer.AddPlayerForConnection(conn, player);
    }

    public override Transform GetStartPosition()
    {
        // SpawnManager'dan pozisyon al
        if (SpawnManager.Instance != null)
        {
            Vector3 spawnPos = SpawnManager.Instance.GetSpawnPosition(numPlayers);
            Transform tempTransform = new GameObject("TempSpawn").transform;
            tempTransform.position = spawnPos;
            return tempTransform;
        }

        return base.GetStartPosition();
    }

    // Character selection'dan çaðrýlacak
    public void SetPlayerMatchData(NetworkConnectionToClient conn, int matchId, int characterId)
    {
        if (!playerMatchData.ContainsKey(conn))
        {
            playerMatchData[conn] = new PlayerMatchData();
        }

        playerMatchData[conn].matchId = matchId;
        playerMatchData[conn].characterId = characterId;
        Debug.Log($"Stored match data for connection {conn.connectionId}: Match {matchId}, Character {characterId}");
    }

}