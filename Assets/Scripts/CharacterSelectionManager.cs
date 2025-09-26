using System.Collections.Generic;
using UnityEngine;
using Mirror;

public struct CharacterSelectMessage : NetworkMessage
{
    public int characterId;
    public int matchId;
}

public struct AllPlayersReadyMessage : NetworkMessage
{
    public int matchId;
}

public class CharacterSelectionManager : NetworkBehaviour
{
    [System.Serializable]
    public class MatchPlayerSelection
    {
        public int matchId;
        public Dictionary<NetworkConnectionToClient, int> playerSelections = new Dictionary<NetworkConnectionToClient, int>();
        public bool allPlayersReady = false;
    }

    [Header("Character Settings")]
    public int availableCharacters = 3; // Ka� karakter var?

    private Dictionary<int, MatchPlayerSelection> matchSelections = new Dictionary<int, MatchPlayerSelection>();

    public static CharacterSelectionManager Instance { get; private set; }

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
        NetworkServer.RegisterHandler<CharacterSelectMessage>(OnCharacterSelectMessage);
        Debug.Log("CharacterSelectionManager server started");
    }

    void OnCharacterSelectMessage(NetworkConnectionToClient conn, CharacterSelectMessage msg)
    {
        Debug.Log($"Player {conn.connectionId} selected character {msg.characterId} for match {msg.matchId}");

        // Match'� bul veya olu�tur
        if (!matchSelections.ContainsKey(msg.matchId))
        {
            matchSelections[msg.matchId] = new MatchPlayerSelection { matchId = msg.matchId };
        }

        var match = matchSelections[msg.matchId];
        match.playerSelections[conn] = msg.characterId;

        // T�m oyuncular se�ti mi kontrol et
        CheckIfAllPlayersReady(msg.matchId);
    }

    void CheckIfAllPlayersReady(int matchId)
    {
        var match = matchSelections[matchId];

        // Bu match i�in ka� oyuncu olmas� gerekiyor? (QueueManager'dan al�nabilir)
        
        int expectedPlayers = QueueManager.Instance.maxPlayersPerMatch;

        if (match.playerSelections.Count >= expectedPlayers && !match.allPlayersReady)
        {
            match.allPlayersReady = true;
            Debug.Log($"All players ready for match {matchId}! Starting game...");

            // T�m oyunculara haz�r mesaj� g�nder
            foreach (var player in match.playerSelections.Keys)
            {
                player.Send(new AllPlayersReadyMessage { matchId = matchId });
            }

            // Game scene'ine ge� (sonraki ad�mda yapaca��z)
            // StartGameForMatch(matchId);
        }
    }
}