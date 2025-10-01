using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro; // BUNU EKLE

public class CharacterSelectionUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject characterSelectionPanel;
    public Button character1Button;
    public Button character2Button;
    public Button character3Button;
    public TMP_Text statusText; // Text yerine TMP_Text

    private int currentMatchId;
    private int selectedCharacterId = -1;

    void Start()
    {
        characterSelectionPanel.SetActive(false);

        character1Button.onClick.AddListener(() => SelectCharacter(1));
        character2Button.onClick.AddListener(() => SelectCharacter(2));
        character3Button.onClick.AddListener(() => SelectCharacter(3));

        NetworkClient.RegisterHandler<AllPlayersReadyMessage>(OnAllPlayersReady);
    }

    public void ShowCharacterSelection(int matchId)
    {
        Debug.Log($"ShowCharacterSelection called with matchId: {matchId}");

        currentMatchId = matchId;
        UpdateCharacterButtonNames();

        if (characterSelectionPanel != null)
        {
            characterSelectionPanel.SetActive(true);

            if (statusText != null)
            {
                statusText.text = "Select your character";
            }
        }
    }

    void UpdateCharacterButtonNames()
    {
        if (CharacterDatabase.Instance == null)
        {
            Debug.LogWarning("CharacterDatabase.Instance is null!");
            return;
        }

        if (character1Button != null)
        {
            var charData = CharacterDatabase.Instance.GetCharacterData(1);
            if (charData != null)
            {
                TMP_Text buttonText = character1Button.GetComponentInChildren<TMP_Text>();
                if (buttonText != null)
                {
                    buttonText.text = charData.characterName;
                }
            }
        }

        if (character2Button != null)
        {
            var charData = CharacterDatabase.Instance.GetCharacterData(2);
            if (charData != null)
            {
                TMP_Text buttonText = character2Button.GetComponentInChildren<TMP_Text>();
                if (buttonText != null)
                {
                    buttonText.text = charData.characterName;
                }
            }
        }
    }

    void SelectCharacter(int characterId)
    {
        selectedCharacterId = characterId;

        NetworkClient.Send(new CharacterSelectMessage
        {
            characterId = characterId,
            matchId = currentMatchId
        });

        if (statusText != null)
        {
            statusText.text = $"Character selected. Waiting for other players...";
        }

        character1Button.interactable = false;
        character2Button.interactable = false;
        character3Button.interactable = false;
    }

    void OnAllPlayersReady(AllPlayersReadyMessage msg)
    {
        if (statusText != null)
        {
            statusText.text = "All players ready! Starting game...";
        }

        Invoke(nameof(StartGame), 2f);
    }

    void StartGame()
    {
        Debug.Log("Starting game...");
    }
}