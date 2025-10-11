using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    [Header("Head Renderers")]
    public SpriteRenderer headBack;
    public SpriteRenderer headForward;
    public SpriteRenderer headRight;
    public SpriteRenderer headLeft;
    public SpriteRenderer headBottomRight;
    public SpriteRenderer headBottomLeft;
    public SpriteRenderer headTopRight;
    public SpriteRenderer headTopLeft;
    
    [Header("Body Renderers")]
    public SpriteRenderer bodyForward;
    public SpriteRenderer bodyBack;
    public SpriteRenderer bodyRight;
    public SpriteRenderer bodyLeft;
    public SpriteRenderer bodyTopLeft;
    public SpriteRenderer bodyTopRight;
    public SpriteRenderer bodyBottomLeft;
    public SpriteRenderer bodyBottomRight;
    
    [Header("Hand")]
    public SpriteRenderer hand;
    
    public void ApplyCharacterSprites(CharacterData charData)
    {
        if (charData == null)
        {
            Debug.LogError("CharacterData is null!");
            return;
        }
        
        // Head sprites
        if (headBack != null && charData.headBack != null)
            headBack.sprite = charData.headBack;
        if (headForward != null && charData.headForward != null)
            headForward.sprite = charData.headForward;
        if (headRight != null && charData.headRight != null)
            headRight.sprite = charData.headRight;
        if (headLeft != null && charData.headLeft != null)
            headLeft.sprite = charData.headLeft;
        if (headBottomRight != null && charData.headBottomRight != null)
            headBottomRight.sprite = charData.headBottomRight;
        if (headBottomLeft != null && charData.headBottomLeft != null)
            headBottomLeft.sprite = charData.headBottomLeft;
        if (headTopRight != null && charData.headTopRight != null)
            headTopRight.sprite = charData.headTopRight;
        if (headTopLeft != null && charData.headTopLeft != null)
            headTopLeft.sprite = charData.headTopLeft;
        
        // Body sprites
        if (bodyForward != null && charData.bodyForward != null)
            bodyForward.sprite = charData.bodyForward;
        if (bodyBack != null && charData.bodyBack != null)
            bodyBack.sprite = charData.bodyBack;
        if (bodyRight != null && charData.bodyRight != null)
            bodyRight.sprite = charData.bodyRight;
        if (bodyLeft != null && charData.bodyLeft != null)
            bodyLeft.sprite = charData.bodyLeft;
        if (bodyTopLeft != null && charData.bodyTopLeft != null)
            bodyTopLeft.sprite = charData.bodyTopLeft;
        if (bodyTopRight != null && charData.bodyTopRight != null)
            bodyTopRight.sprite = charData.bodyTopRight;
        if (bodyBottomLeft != null && charData.bodyBottomLeft != null)
            bodyBottomLeft.sprite = charData.bodyBottomLeft;
        if (bodyBottomRight != null && charData.bodyBottomRight != null)
            bodyBottomRight.sprite = charData.bodyBottomRight;
        
        // Hand
        if (hand != null && charData.hand != null)
            hand.sprite = charData.hand;
        
        Debug.Log($"Applied {charData.characterName} sprites to all body parts");
    }
    
    // Hangi parçaların görüneceğini kontrol et (yön bazlı)
    public void SetDirection(Vector2 moveDirection)
    {
        // Tüm parçaları gizle
        HideAllParts();
        
        // Hareket yönüne göre uygun parçaları göster
        if (moveDirection.y > 0.5f) // Yukarı
        {
            ShowPart(headBack);
            ShowPart(bodyBack);
        }
        else if (moveDirection.y < -0.5f) // Aşağı
        {
            ShowPart(headForward);
            ShowPart(bodyForward);
        }
        else if (moveDirection.x > 0.1f) // Sağ
        {
            ShowPart(headRight);
            ShowPart(bodyRight);
        }
        else if (moveDirection.x < -0.1f) // Sol
        {
            ShowPart(headLeft);
            ShowPart(bodyLeft);
        }
        else // Idle - önden göster
        {
            ShowPart(headForward);
            ShowPart(bodyForward);
        }
        
        // El her zaman görünsün (opsiyonel)
        ShowPart(hand);
    }
    
    void HideAllParts()
    {
        if (headBack != null) headBack.enabled = false;
        if (headForward != null) headForward.enabled = false;
        if (headRight != null) headRight.enabled = false;
        if (headLeft != null) headLeft.enabled = false;
        if (headBottomRight != null) headBottomRight.enabled = false;
        if (headBottomLeft != null) headBottomLeft.enabled = false;
        if (headTopRight != null) headTopRight.enabled = false;
        if (headTopLeft != null) headTopLeft.enabled = false;
        
        if (bodyForward != null) bodyForward.enabled = false;
        if (bodyBack != null) bodyBack.enabled = false;
        if (bodyRight != null) bodyRight.enabled = false;
        if (bodyLeft != null) bodyLeft.enabled = false;
        if (bodyTopLeft != null) bodyTopLeft.enabled = false;
        if (bodyTopRight != null) bodyTopRight.enabled = false;
        if (bodyBottomLeft != null) bodyBottomLeft.enabled = false;
        if (bodyBottomRight != null) bodyBottomRight.enabled = false;
        
        if (hand != null) hand.enabled = false;
    }
    
    void ShowPart(SpriteRenderer renderer)
    {
        if (renderer != null)
            renderer.enabled = true;
    }
}