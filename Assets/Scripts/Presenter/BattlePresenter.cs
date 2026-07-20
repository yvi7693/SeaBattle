using System;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class BattlePresenter : MonoBehaviour
{
    protected Staff staff;
    [SerializeField] protected BoardView rightBoard;
    [SerializeField] protected BoardView leftBoard;
    [SerializeField] protected TMP_Text player2;
    [SerializeField] protected GameUI[] leftCount;
    [SerializeField] protected GameUI[] rightCount;

    protected TurnRecon turnRecon;

    protected virtual void Start()
    {
        staff = GameSession.Instance.staff;

        rightBoard.Init(this);
        leftBoard.Init(this);

        Init(staff.GetTurnRecon());
        UpdateCount();
    }


    public void Init(TurnRecon turnRecon)
    {
        this.turnRecon = turnRecon;
        SwitchMove();

        if (GameSession.Instance.GetMode() == Mode.Ai)
            ShowShips(turnRecon.GetRightSea(), rightBoard);
    }


    public void ShowShips(Sea sea, BoardView board){
        Fleet fleet = sea.GetFleet();
        Ship[] ships = fleet.GetShips();

        for (int i = 0; i < 10; i++)
        {
            List<Sector> sectors = ships[i].GetPlace();
            List<(int, int)> coords = new List<(int, int)>();

            for (int j = 0; j < sectors.Count; j++)
            {
                coords.Add(sectors[j].GetCoord());
            }

            List<(int, int)> sortedCoords = NormalizeCoords(coords);

            (int x, int y) = sortedCoords[0];

            SectorView sector = board.GetSector(x, y);
            int size = ships[i].GetDurability();
            bool isVertical = IsVertical(sortedCoords);

           board.PlaceShip(sector, size, isVertical);

        }
    }

    private List<(int , int)> NormalizeCoords(List<(int x, int y)> coords)
    {
        coords.Sort((coord1, coord2) =>
        {
        if (coord1.x != coord2.x)
            return coord1.x.CompareTo(coord2.x);

        return coord1.y.CompareTo(coord2.y);

        });

        return coords;
    }


    private bool IsVertical(List<(int, int)> coords)
    {
        if (coords.Count < 2)
            return false;

       (int x1, int y1) = coords[0];
       (int x2, int y2) = coords[1];

       if (x1 != x2)
            return false;
        
        else if (y1 != y2)
            return true;
        
        else
            throw new Exception("incorrect coords");
    }


    public void UpdateCount()
    {
       BattleController battleController = staff.GetBattleController();

       Fleet leftFleet = battleController.GetLeftFleet();
       SetCount(leftFleet, leftCount);

       Fleet rightFleet = battleController.GetRightFleet();
       SetCount(rightFleet, rightCount);
    }

    private void SetCount(Fleet fleet, GameUI[] gameUi)
    {
        for(int size = 1; size <= 4; size++)
        {
            int count = fleet.CalculateCount(size);
            GameUI counterUi = GetUiWithSize(size, gameUi);

            counterUi.SetCountShips(count);
        }
    }


    public BoardView GetActiveBoard()
    {
        if (rightBoard.GetActive())
            return rightBoard;
        
        else
            return leftBoard;
    }


    public virtual void AttackSector(SectorView sectorView, int targetX, int targetY)
    {
        BoardView activeBoard = GetActiveBoard();
        Sea targetSea = staff.GetTurnRecon().GetTargetSea();

        MissionResult result = staff.TacticalDirective(targetX, targetY);

        UpdateCount();
        UpdateView(result, sectorView);

        if (TryEnd(result))
            return;

        UpdateMiss(targetSea, activeBoard.GetSectors());
        SwitchMove();
    }


    protected void UpdateMiss(Sea sea, SectorView[,] sectors)
    {
        for(int x = 0; x < 10; x++)
        {
            for(int y = 0; y < 10; y++)
            {
                Sector sector = sea.GetSector(x,y);
                
                if (sector.GetStatus() == StatusSector.Miss)
                    sectors[x,y].DisplayMiss();
            }
        }
    }


    protected virtual void SwitchMove()
    {
        Sea targetSea = turnRecon.GetTargetSea();

        if (targetSea == turnRecon.GetRightSea())
        {
            rightBoard.SetClicked(true);
            rightBoard.SetActive(true);

            leftBoard.SetClicked(false);
            leftBoard.SetActive(false);
        }

        else if (targetSea == turnRecon.GetLeftSea())
        {
            leftBoard.SetClicked(true);
            leftBoard.SetActive(true);
            
            rightBoard.SetClicked(false);
            rightBoard.SetActive(false);
        }
            
        else
            throw new Exception("wrong target sea");
        
    }


    protected void UpdateView(MissionResult result, SectorView sectorView)
    {
        if (result == MissionResult.Miss)

            sectorView.DisplayMiss();

        else if(result == MissionResult.Hit)
            sectorView.DisplayHit();
    }


    protected bool TryEnd(MissionResult result)
    {
        if(result == MissionResult.HaveWinner)
        {
            GameSession.Instance.End();
            return true;
        }
            
        return false;
    }


    private GameUI GetUiWithSize(int size, GameUI[] gameUi)
    {
        for (int i = 0; i < gameUi.Length; i++)
        {
            if (gameUi[i].GetSize() == size)
                return gameUi[i];
        }

        throw new Exception("ship counter UI was not found");

    }

}
