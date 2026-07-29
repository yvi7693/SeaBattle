using System;
using System.Collections.Generic;

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
    private HomingWeapon homingWeapon;

    public Staff()
    {
        battleController = new BattleController();
        attackResolver = new AttackResolver();
        deploymentOfficer = new DeploymentOfficer();
        sinker = new Sinker(deploymentOfficer);
        turnRecon = new TurnRecon(battleController.GetRightFleet(), battleController.GetLeftFleet());
        assignee = new Assignee(attackResolver, sinker, turnRecon);
        plansOfficer = new PlansOfficer(turnRecon, assignee, deploymentOfficer, battleController);
        commander = new Commander(plansOfficer, assignee, turnRecon, battleController);
        homingWeapon = new HomingWeapon();
    }

    public TurnRecon GetTurnRecon()
    {
        return turnRecon;
    }

    public DeploymentOfficer GetDeploymentOfficer()
    {
        return deploymentOfficer;
    }

    public BattleController GetBattleController()
    {
        return battleController;
    }

    public HomingWeapon GetHomingWeapon()
    {
        return homingWeapon;
    }

    public MissionResult TacticalDirective(int targetX, int targetY)
    {
        if (targetX < 0 || targetY < 0) 
            throw new ArgumentException("incorrect value");

        return commander.AssignMission(targetX, targetY);

    }

    public void DeployDirective( List <(int x, int y)> positions)
    {
        Sea deploySea = turnRecon.GetDeploySea();
        plansOfficer.TryDeployShip(deploySea, positions);
        
    }

    public void DeployFleet()
    {
        plansOfficer.DeployFleet();
    }
}

public enum MissionResult
{
    HaveWinner,
    Hit,
    Miss,
    UnsucessfulShot
}
