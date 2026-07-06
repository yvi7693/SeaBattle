using System;
using UnityEngine;
using TMPro;

public class BattlePresenter : MonoBehaviour
{
    protected Staff staff;
    [SerializeField] private BoardView RightBoard;
    [SerializeField] private BoardView LeftBoard;
    [SerializeField] private GameUI gameUi;
    [SerializeField] private TMP_Text player2;
    private TurnRecon turnRecon;

    private void Start()
    {
        staff = GameSession.Instance.staff;

        RightBoard.Init(this);
        LeftBoard.Init(this);

        Init(staff.GetTurnRecon());

        if (GameSession.Instance.GetMode() == Mode.Ai)
            player2.text = "AI";
    }


    public void Init(TurnRecon turnRecon)
    {
        this.turnRecon = turnRecon;
        SwitchMove();

        if (GameSession.Instance.GetMode() == Mode.Ai)
            RightBoard.ShowShips(turnRecon.GetSea1());
        
    }


    public BoardView GetActiveBoard()
    {
        if (RightBoard.GetClicked())
            return RightBoard;
        
        else
            return LeftBoard;
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
            RightBoard.SetClicked(true);
            LeftBoard.SetClicked(false);
        }

        else if (queue == turnRecon.GetSea2())
        {
            LeftBoard.SetClicked(true);
            RightBoard.SetClicked(false);
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

