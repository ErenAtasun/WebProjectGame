using UnityEngine;
using System.Collections.Generic;

public class CharacterDatabase : MonoBehaviour
{
    [Header("Available Characters")]
    public CharacterData[] characters;

    public static CharacterDatabase Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public CharacterData GetCharacterData(int characterId)
    {
        foreach (var character in characters)
        {
            if (character.characterId == characterId)
                return character;
        }

        Debug.LogWarning($"Character ID {characterId} not found!");
        return null;
    }
}