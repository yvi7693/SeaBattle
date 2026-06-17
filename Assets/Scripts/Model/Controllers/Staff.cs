public class Staff
{

    private Commander commander;
    private BattleController battleController;
    private Assignee assignee;
    private PlansOfficer plansOfficer;
    private TurnRecon turnRecon;
    private DeploymentOfficer deploymentOfficer;
    private Sinker sinker;
    private AttackResolver attackResolver;

    public Staff()
    {
        battleController = new BattleController();
        attackResolver = new AttackResolver();
        sinker = new Sinker();
        assignee = new Assignee(attackResolver, sinker);
        deploymentOfficer = new DeploymentOfficer();
        turnRecon = new TurnRecon();
        plansOfficer = new PlansOfficer(turnRecon, assignee, deploymentOfficer);
        commander = new Commander(plansOfficer, assignee);
    }

    public bool TacticalDirective(int targetX, int targetY)
    {
        return commander.TryAssignMission(targetX, targetY);

    }


    
}

