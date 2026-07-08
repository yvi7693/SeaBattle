public class Commander
{
    private PlansOfficer plansOfficer;
    private Assignee assignee;
    private TurnRecon turnRecon;
    private BattleController battleController;
    

    public Commander(PlansOfficer plansOfficer, 
                    Assignee assignee, 
                    TurnRecon turnRecon,
                    BattleController battleController)
    {
        this.plansOfficer = plansOfficer;
        this.assignee = assignee;
        this.turnRecon = turnRecon;
        this.battleController = battleController;
    }

    public MissionResult AssignMission(int targetX, int targetY)
    {
        (bool permission, Sector targetSector) = plansOfficer.PlanOrder(targetX, targetY);

        if (permission)
        {
            StatusSector newStatus = assignee.AttackOrder(targetSector);

            if (battleController.IsDeclareWinner())
            return MissionResult.HaveWinner;
            
            if (newStatus == StatusSector.Hit)
                return MissionResult.Hit;

            else
            {
                turnRecon.SwitchTargetSea();
                
                return MissionResult.Miss;
            }
                
        }

        return MissionResult.UnsucessfulShot;
    }
}
