using UnityEngine;

public class BoardView : MonoBehaviour
{
    [SerializeField]private GameObject sectorPrefab;
    [SerializeField]private int sizeSea = 10;
    [SerializeField]private float spacing = 0.5f;
    private BattlePresenter battlePresenter;

    public void Init(BattlePresenter battlePresenter)
    {
        this.battlePresenter = battlePresenter;
        
        CreateBoard();
    }

    void CreateBoard()
    {
        for (int x = 0; x < sizeSea; x++)
        {
            for (int y = 0; y < sizeSea; y++)
            {
                GameObject sectorObject = Instantiate(sectorPrefab, transform);
                sectorObject.transform.localPosition = new Vector3(x * spacing, y * spacing, 0);

                SectorView sectorView = sectorObject.GetComponent<SectorView>();
                sectorView.Init(x, y, battlePresenter);

                sectorObject.name = $"Sector_{x}_{y}";
            }
        }
    }
}
