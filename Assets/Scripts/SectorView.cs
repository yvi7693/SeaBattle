using UnityEngine;

public class SectorView : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
   
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        spriteRenderer.color = Color.white;
    }
    
}