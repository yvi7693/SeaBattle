public class Commander
{
    private PlansOfficer plansOfficer;
    private Assignee assignee;

    public Commander(PlansOfficer plansOfficer, Assignee assignee)
    {
        this.plansOfficer = plansOfficer;
        this.assignee = assignee;
    }

    public MissionResult AssignMission(int targetX, int targetY)
    {
        (bool permission, Sector targetSector) = plansOfficer.PlanOrder(targetX, targetY);

        if (permission)
        {
            StatusSector newStatus = assignee.AttackOrder(targetSector);
            
            if (newStatus == StatusSector.Hit)
                return MissionResult.Hit;

            else
                return MissionResult.Miss;
        }

        return MissionResult.UnsucessfulShot;
    }
}