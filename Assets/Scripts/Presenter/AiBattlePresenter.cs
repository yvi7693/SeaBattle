using System;
using System.Collections;
using UnityEngine;

public class AiBattlePresenter : BattlePresenter
{
    private HomingWeapon homingWeapon;


    protected override void Start()
    {
        base.Start();

        homingWeapon = GameSession.Instance.staff.GetHomingWeapon();
        player2.text = "AI";
       
    }


    protected override void AttackResolved(MissionResult result)
    {
        if (IsAiMove())
            StartCoroutine(MoveAiDelay());
    }


    private IEnumerator MoveAiDelay()
    {
        yield return new WaitForSeconds(1f);
        MoveAi();
    }


    private void MoveAi()
    {

        rightBoard.SetClicked(false);

        Sea targetSea = staff.GetTurnRecon().GetTargetSea();
        (int x, int y) = homingWeapon.Guidance(targetSea);

        base.AttackSector(x, y);
    }


    private bool IsAiMove()
    {
        BattleController battleController = staff.GetBattleController();
        return (staff.GetTurnRecon().GetTargetSea() == staff.GetTurnRecon().GetRightSea()) && !battleController.IsDeclareWinner();
    }
}
