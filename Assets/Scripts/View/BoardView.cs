using UnityEngine;

public class BoardView : MonoBehaviour
{
    [SerializeField]private GameObject sectorPrefab;
    [SerializeField]private int sizeSea = 10;
    [SerializeField]private float spacing = 0.5f;
    private BattlePresenter battlePresenter;
    private SectorView [,] sectors;
    [SerializeField]private bool clicked;
    [SerializeField]private GameObject activeFrame;

    public void Init(BattlePresenter battlePresenter, bool clicked = true)
    {
        this.battlePresenter = battlePresenter;

        this.clicked = clicked;

        sectors = new SectorView[sizeSea, sizeSea];

        activeFrame.SetActive(false);
        
        CreateBoard();
    }

    public bool GetClicked()
    {
        return clicked;
    }

    public void SetClicked(bool value)
    {
        for (int x = 0; x < sizeSea; x++)
        {
            for (int y = 0; y < sizeSea; y++)
            {
                sectors[x,y].SetClicked(value);
            }
        }

        activeFrame.SetActive(value);
        clicked = value;
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

                sectors[x, y] = sectorView;

                sectorObject.name = $"Sector_{x}_{y}";
            }
        }
    }
}
