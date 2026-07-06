

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

        if (staff.GetTurnRecon().GetTargetSea() == staff.GetTurnRecon().GetRightSea())
            StartCoroutine(MoveDelay());
    }

    public void AiMove()
    {
        BoardView activeBoard = GetActiveBoard();
        Sea targetSea = staff.GetTurnRecon().GetTargetSea();

        (int x, int y) = homingWeapon.Guidance(targetSea);

        MissionResult result = staff.TacticalDirective(x, y);
        UpdateCount();
        
        SectorView updateSector = activeBoard.GetSector(x, y);

        UpdateView(result, updateSector);
        UpdateMiss(targetSea, activeBoard.GetSectors());

        SwitchMove();

        if (result == MissionResult.Hit)
            StartCoroutine(MoveDelay());
    }


    protected override void SwitchMove()
    {
        Sea targetSea = turnRecon.GetTargetSea();

        if (targetSea == turnRecon.GetRightSea())
        {
            rightBoard.SetClicked(false);
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

    private IEnumerator MoveDelay()
    {
        yield return new WaitForSeconds(1f);

        AiMove();
    }
}
