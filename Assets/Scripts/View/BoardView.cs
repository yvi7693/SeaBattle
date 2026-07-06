using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardView : MonoBehaviour
{
    [SerializeField]private GameObject sectorPrefab;
    [SerializeField]private int sizeSea = 10;
    [SerializeField]private float spacing = 0.5f;
    private BattlePresenter battlePresenter;
    private SectorView [,] sectors;
    [SerializeField]private bool active;
    [SerializeField]private GameObject activeFrame;

    public void Init(BattlePresenter battlePresenter, bool clicked = true)
    {
        this.battlePresenter = battlePresenter;

        this.active = clicked;

        sectors = new SectorView[sizeSea, sizeSea];

        activeFrame.SetActive(false);
        
        CreateBoard();
    }

    public bool GetActive()
    {
        return active;
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

    } 

    public void SetActive(bool value)
    {
        activeFrame.SetActive(value);
        active = value;
    }

    public SectorView[,] GetSectors()
    {
        return sectors;
    }

    public SectorView GetSector(int x, int y)
    {
        if ((x < 0 || x >= 10) || (y < 0 || y >= 10))
            throw new Exception("incorrect value coord");
            
        return sectors[x, y];
    }


    public void CreateBoard()
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

    public void ShowShips(Sea sea){
        for (int i = 0; i < sizeSea; i++)
        {
            for (int j = 0; j < sizeSea; j++)
            {
                if (sea.GetSector(i, j).GetStatus() == StatusSector.Ship)
                    sectors[i, j].ShowShip();
            }
        }
    }
}
