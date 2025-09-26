using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class CharacterSelectionUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject characterSelectionPanel;
    public Button character1Button;
    public Button character2Button;
    public Button character3Button;
    public Text statusText;

    private int currentMatchId;
    private int selectedCharacterId = -1;

    void Start()
    {
        // Baþta panel kapalý
        characterSelectionPanel.SetActive(false);

        // Button listener'lar
        character1Button.onClick.AddListener(() => SelectCharacter(1));
        character2Button.onClick.AddListener(() => SelectCharacter(2));
        character3Button.onClick.AddListener(() => SelectCharacter(3));

        // AllPlayersReady mesajýný dinle
        NetworkClient.RegisterHandler<AllPlayersReadyMessage>(OnAllPlayersReady);
    }

    public void ShowCharacterSelection(int matchId)
    {
        currentMatchId = matchId;
        characterSelectionPanel.SetActive(true);
        statusText.text = "Select your character";
    }

    void SelectCharacter(int characterId)
    {
        selectedCharacterId = characterId;

        // Server'a seçimi gönder
        NetworkClient.Send(new CharacterSelectMessage
        {
            characterId = characterId,
            matchId = currentMatchId
        });

        // UI güncelle
        statusText.text = $"Character {characterId} selected. Waiting for other players...";

        // Butonlarý deaktif et
        character1Button.interactable = false;
        character2Button.interactable = false;
        character3Button.interactable = false;
    }

    void OnAllPlayersReady(AllPlayersReadyMessage msg)
    {
        statusText.text = "All players ready! Starting game...";

        // 2 saniye sonra game scene'e geç (sonraki adýmda yapacaðýz)
        Invoke("StartGame", 2f);
    }

    void StartGame()
    {
        Debug.Log("Starting game...");
        // Game scene loading buraya gelecek
    }
}