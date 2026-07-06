

using System;
using System.Collections;
using UnityEngine;

public class AiBattlePresenter : BattlePresenter
{
    private HomingWeapon homingWeapon;

    private void Awake()
    {
        homingWeapon = GameSession.Instance.staff.GetHomingWeapon();
        player2.text = "AI";
       
    }

    public override void AttackSector(SectorView sectorView, int targetX, int targetY)
    {
        base.AttackSector(sectorView, targetX, targetY);

        if (staff.GetTurnRecon().GetQueue() == staff.GetTurnRecon().GetSea1())
            StartCoroutine(MoveDelay());
    }

    public void AiMove()
    {
        BoardView activeBoard = GetActiveBoard();
        Sea activeSea = staff.GetTurnRecon().GetQueue();

        (int x, int y) = homingWeapon.Guidance(activeSea);

        MissionResult result = staff.TacticalDirective(x, y);
        SectorView updateSector = activeBoard.GetSector(x, y);

        UpdateView(result, updateSector);
        UpdateMiss(activeSea, activeBoard.GetSectors());

        SwitchMove();

        if (result == MissionResult.Hit)
            StartCoroutine(MoveDelay());
    }


    protected override void SwitchMove()
    {
        Sea queue = turnRecon.GetQueue();

        if (queue == turnRecon.GetSea1())
        {
            RightBoard.SetClicked(false);
            RightBoard.SetActive(true);

            LeftBoard.SetClicked(false);
            LeftBoard.SetActive(false);
        }

        else if (queue == turnRecon.GetSea2())
        {
            LeftBoard.SetClicked(true);
            LeftBoard.SetActive(true);
            
            RightBoard.SetClicked(false);
            RightBoard.SetActive(false);
        }
            
        else
            throw new Exception("wrong queue course");
    }

    private IEnumerator MoveDelay()
    {
        yield return new WaitForSeconds(1f);

        AiMove();
    }
}