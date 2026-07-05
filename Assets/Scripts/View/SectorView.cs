using UnityEngine;

public class SectorView : MonoBehaviour
{
    private int x;
    private int y;
    private BattlePresenter battlePresenter;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;

    public void Init(int x, int y, BattlePresenter battlePresenter)
    {
        this.x = x;
        this.y = y;
        this.battlePresenter = battlePresenter;
        
    }

    public void DisplayMiss()
    {
        spriteRenderer.color = Color.white;
    }

    public void DisplayHit()
    {
        spriteRenderer.color = Color.red;
    }

    public void ShowShip()
    {
        spriteRenderer.color = Color.gray;
    }

    public void SetClicked(bool value)
    {
        boxCollider2D.enabled = value;
    }
   
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnMouseDown()
    {   
        battlePresenter.AttackSector(this, x, y);
    }

    
}