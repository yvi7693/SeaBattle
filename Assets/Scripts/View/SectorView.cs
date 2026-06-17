using UnityEngine;

public class SectorView : MonoBehaviour
{
    private int x;
    private int y;
    private BattlePresenter battlePresenter;
    private SpriteRenderer spriteRenderer;

    public void Init(int x, int y, BattlePresenter battlePresenter)
    {
        this.x = x;
        this.y = y;
        this.battlePresenter = battlePresenter;
    }
   
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {   
        battlePresenter.AttackSector(x, y);
    }
    
}