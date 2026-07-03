using System;
using System.Collections.Generic;
using System.Diagnostics;

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
        deploymentOfficer = new DeploymentOfficer();
        sinker = new Sinker(deploymentOfficer);
        turnRecon = new TurnRecon(battleController.GetFleet1(), battleController.GetFleet2());
        assignee = new Assignee(attackResolver, sinker, turnRecon);
        plansOfficer = new PlansOfficer(turnRecon, assignee, deploymentOfficer);
        commander = new Commander(plansOfficer, assignee, turnRecon);
    }

    public TurnRecon GetTurnRecon()
    {
        return turnRecon;
    }

    public DeploymentOfficer GetDeploymentOfficer()
    {
        return deploymentOfficer;
    }

    public MissionResult TacticalDirective(int targetX, int targetY)
    {
        if (targetX < 0 || targetY < 0) throw new ArgumentException("incorrect value");

        if (battleController.IsDeclareWinner())
            return MissionResult.HaveWinner;

        return commander.AssignMission(targetX, targetY);

    }

    public void DeployDirective( List <(int x, int y)> positions)
    {
        plansOfficer.TryDeployShip(positions);
        
    }
}

public enum MissionResult
{
    HaveWinner,
    Hit,
    Miss,
    UnsucessfulShot
}
