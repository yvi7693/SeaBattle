using System;
using UnityEngine;

public class BattlePresenter : MonoBehaviour
{
    private Staff staff;
    [SerializeField] private BoardView playerBoard;
    [SerializeField] private BoardView opponentBoard;
     private TurnRecon turnRecon;

    public void Init(TurnRecon turnRecon)
    {
        this.turnRecon = turnRecon;
        SwitchMove();
    }

    public BoardView GetActiveBoard()
    {
        if (playerBoard.GetClicked())
            return playerBoard;
        
        else
            return opponentBoard;
    }

    public void AttackSector(SectorView sectorView, int targetX, int targetY)
    {
        BoardView activeBoard = GetActiveBoard();
        Sea activeSea = staff.GetTurnRecon().GetQueue();

        MissionResult result = staff.TacticalDirective(targetX, targetY);

        if (result == MissionResult.Miss)

            sectorView.DisplayMiss();

        else if(result == MissionResult.Hit)
            sectorView.DisplayHit();

        UpdateMiss(activeSea, activeBoard.GetSectors());
        
        SwitchMove();
    }

    public void UpdateMiss(Sea sea, SectorView[,] sectors)
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

     public void SwitchMove()
    {
        Sea queue = turnRecon.GetQueue();

        if (queue == turnRecon.GetSea1())
        {
            playerBoard.SetClicked(true);
            opponentBoard.SetClicked(false);
        }

        else if (queue == turnRecon.GetSea2())
        {
            opponentBoard.SetClicked(true);
             playerBoard.SetClicked(false);
        }
            
        else
            throw new Exception("wrong queue course");
        
    }

    private void Start()
    {
        staff = GameSession.Instance.staff;

        playerBoard.Init(this);
        opponentBoard.Init(this);

        Init(staff.GetTurnRecon());
    }
}

