using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Game/Character Data")]
public class CharacterData : ScriptableObject
{
    public int characterId;
    public string characterName;
    public Sprite characterSprite; // 3D model yerine sprite
    public Color characterTint = Color.white; // Sprite'a renk tonu

    // Karakter özellikleri (server-side)
    public float moveSpeed = 5f;
    public int maxHealth = 100;
}