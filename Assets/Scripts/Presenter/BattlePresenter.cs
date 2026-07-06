using System;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class BattlePresenter : MonoBehaviour
{
    protected Staff staff;
    [FormerlySerializedAs("RightBoard")]
    [SerializeField] protected BoardView rightBoard;
    [FormerlySerializedAs("LeftBoard")]
    [SerializeField] protected BoardView leftBoard;
    [SerializeField] protected TMP_Text player2;
    [SerializeField] protected GameUI[] leftCount;
    [SerializeField] protected GameUI[] rightCount;

    protected TurnRecon turnRecon;

    private void Start()
    {
        staff = GameSession.Instance.staff;

        rightBoard.Init(this);
        leftBoard.Init(this);

        Init(staff.GetTurnRecon());
        UpdateCount();
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


    public void Init(TurnRecon turnRecon)
    {
        this.turnRecon = turnRecon;
        SwitchMove();

        if (GameSession.Instance.GetMode() == Mode.Ai)
            rightBoard.ShowShips(turnRecon.GetRightSea());
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

        else if(result == MissionResult.HaveWinner)
            
            GameSession.Instance.End();
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
