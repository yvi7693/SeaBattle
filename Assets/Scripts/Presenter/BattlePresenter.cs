using System;
using UnityEngine;

public class BattlePresenter : MonoBehaviour
{
    protected Staff staff;
    [SerializeField] private BoardView playerBoard;
    [SerializeField] private BoardView opponentBoard;
    [SerializeField] private GameUI gameUi;
    private TurnRecon turnRecon;

    private void Start()
    {
        staff = GameSession.Instance.staff;

        playerBoard.Init(this);
        opponentBoard.Init(this);

        Init(staff.GetTurnRecon());
    }


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


    public virtual void AttackSector(SectorView sectorView, int targetX, int targetY)
    {
        BoardView activeBoard = GetActiveBoard();
        Sea activeSea = staff.GetTurnRecon().GetQueue();

        MissionResult result = staff.TacticalDirective(targetX, targetY);

        UpdateView(result, sectorView);
        UpdateMiss(activeSea, activeBoard.GetSectors());

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


    protected void SwitchMove()
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


    protected void UpdateView(MissionResult result, SectorView sectorView)
    {
        if (result == MissionResult.Miss)

            sectorView.DisplayMiss();

        else if(result == MissionResult.Hit)
            sectorView.DisplayHit();

        else if(result == MissionResult.HaveWinner)
            
            GameSession.Instance.End();
    }
}

