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

    public void AttackSector(SectorView sectorView, int targetX, int targetY)
    {
        MissionResult result = staff.TacticalDirective(targetX, targetY);

        if (result == MissionResult.Miss)

            sectorView.DisplayMiss();

        else if(result == MissionResult.Hit)
            sectorView.DisplayHit();
        
        SwitchMove();
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
        staff = new Staff();

        playerBoard.Init(this);
        opponentBoard.Init(this);

        Init(staff.GetTurnRecon());
    }
}

