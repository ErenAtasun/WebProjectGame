using System.Collections.Generic;
using UnityEngine;
using Mirror;

public struct QueueJoinMessage : NetworkMessage { }
public struct QueueLeaveMessage : NetworkMessage { }
public struct MatchFoundMessage : NetworkMessage
{
    public int matchId;
}

public class QueueManager : NetworkBehaviour
{
    [System.Serializable]
    public class QueuedPlayer
    {
        public NetworkConnectionToClient connection;
        public string playerName;
        public float joinTime;

        public QueuedPlayer(NetworkConnectionToClient conn, string name)
        {
            connection = conn;
            playerName = name;
            joinTime = Time.time;
        }
    }

    [Header("Queue Settings")]
    public int maxPlayersPerMatch = 1;

    private List<QueuedPlayer> waitingPlayers = new List<QueuedPlayer>();
    private int nextMatchId = 1;

    public static QueueManager Instance { get; private set; }

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

    public override void OnStartServer()
    {
        base.OnStartServer();

        // Server mesajlarýný dinle
        NetworkServer.RegisterHandler<QueueJoinMessage>(OnQueueJoinMessage);
        NetworkServer.RegisterHandler<QueueLeaveMessage>(OnQueueLeaveMessage);

        Debug.Log("QueueManager server started");
    }

    void OnQueueJoinMessage(NetworkConnectionToClient conn, QueueJoinMessage msg)
    {
        AddPlayerToQueue(conn);
    }

    void OnQueueLeaveMessage(NetworkConnectionToClient conn, QueueLeaveMessage msg)
    {
        RemovePlayerFromQueue(conn);
    }

    public void AddPlayerToQueue(NetworkConnectionToClient connection)
    {
        // Zaten queue'da mý kontrol et
        if (waitingPlayers.Exists(p => p.connection == connection))
        {
            Debug.Log($"Player {connection.connectionId} already in queue");
            return;
        }

        // Queue'ya ekle
        QueuedPlayer newPlayer = new QueuedPlayer(connection, $"Player_{connection.connectionId}");
        waitingPlayers.Add(newPlayer);

        Debug.Log($"Player {connection.connectionId} joined queue. Queue size: {waitingPlayers.Count}");

        // Eþleþme kontrol et
        CheckForMatches();
    }

    public void RemovePlayerFromQueue(NetworkConnectionToClient connection)
    {
        waitingPlayers.RemoveAll(p => p.connection == connection);
        Debug.Log($"Player {connection.connectionId} left queue. Queue size: {waitingPlayers.Count}");
    }

    void CheckForMatches()
    {
        // Yeterli oyuncu var mý?
        if (waitingPlayers.Count >= maxPlayersPerMatch)
        {
            // Ýlk X oyuncuyu al
            List<QueuedPlayer> matchPlayers = new List<QueuedPlayer>();
            for (int i = 0; i < maxPlayersPerMatch; i++)
            {
                matchPlayers.Add(waitingPlayers[i]);
            }

            // Bu oyuncularý queue'dan çýkar
            foreach (var player in matchPlayers)
            {
                waitingPlayers.Remove(player);
            }

            // Maç oluþtur
            CreateMatch(matchPlayers);
        }
    }

    void CreateMatch(List<QueuedPlayer> players)
    {
        int matchId = nextMatchId++;

        Debug.Log($"Match {matchId} created with {players.Count} players");

        // Oyunculara maç bulundu mesajý gönder
        foreach (var player in players)
        {
            player.connection.Send(new MatchFoundMessage { matchId = matchId });
        }
    }
}