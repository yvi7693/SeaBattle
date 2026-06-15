public class Commander
{
    private PlansOfficer plansOfficer;
    private Assignee assignee;

    public Commander(PlansOfficer plansOfficer, Assignee assignee)
    {
        this.plansOfficer = plansOfficer;
        this.assignee = assignee;
    }

    public bool TryAssignMission(int targetX, int targetY)
    {
        (bool permission, Sector targetSector) = plansOfficer.PlanOrder(targetX, targetY);

        if (permission)
            assignee.AttackOrder(targetSector);

        return permission;
    }
}