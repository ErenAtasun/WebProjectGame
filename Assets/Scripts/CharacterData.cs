using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Game/Character Data")]
public class CharacterData : ScriptableObject
{
    public int characterId;
    public string characterName;
    
    [Header("Head Sprites")]
    public Sprite headBack;
    public Sprite headForward;
    
    [Header("Body Forward Sprites")]
    public Sprite bodyForward;
    public Sprite bodyTopLeft;
    public Sprite bodyTopRight;
    public Sprite bodyBottomLeft;
    public Sprite bodyBottomRight;
    
    [Header("Body Back Sprites")]
    public Sprite bodyBack;
    
    [Header("Body Side Sprites")]
    public Sprite bodyRight;
    public Sprite bodyLeft;
    
    [Header("Head Side Sprites")]
    public Sprite headRight;
    public Sprite headLeft;
    public Sprite headBottomRight;
    public Sprite headBottomLeft;
    public Sprite headTopRight;
    public Sprite headTopLeft;
    
    [Header("Hand")]
    public Sprite hand;
    
    [Header("Stats - SERVER ONLY")]
    public float moveSpeed = 5f;
    public int maxHealth = 100;
}