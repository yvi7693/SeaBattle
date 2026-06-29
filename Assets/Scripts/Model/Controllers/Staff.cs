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

    public Staff()
    {
        battleController = new BattleController();
        attackResolver = new AttackResolver();
        sinker = new Sinker();
        turnRecon = new TurnRecon(battleController.GetFleet1(), battleController.GetFleet2());
        assignee = new Assignee(attackResolver, sinker, turnRecon);
        deploymentOfficer = new DeploymentOfficer();
        plansOfficer = new PlansOfficer(turnRecon, assignee, deploymentOfficer);
        commander = new Commander(plansOfficer, assignee, turnRecon);

        // временный код

        

        plansOfficer.TryDeployShip(new List<(int, int)> { (0, 0), (1, 0), (2, 0), (3, 0) });
        plansOfficer.TryDeployShip(new List<(int, int)> { (0, 2), (1, 2), (2, 2) });
        plansOfficer.TryDeployShip(new List<(int, int)> { (5, 0), (5, 1), (5, 2) });
        plansOfficer.TryDeployShip( new List<(int, int)> { (0, 4), (1, 4) });
        plansOfficer.TryDeployShip(new List<(int, int)> { (3, 4), (4, 4) });
        plansOfficer.TryDeployShip( new List<(int, int)> { (7, 0), (8, 0) });
        plansOfficer.TryDeployShip(new List<(int, int)> { (0, 6) });
        plansOfficer.TryDeployShip(new List<(int, int)> { (2, 6) });
        plansOfficer.TryDeployShip( new List<(int, int)> { (4, 6) });
        plansOfficer.TryDeployShip( new List<(int, int)> { (6, 6) });

        plansOfficer.TryDeployShip(new List<(int, int)> { (0, 0), (1, 0), (2, 0), (3, 0) });
        plansOfficer.TryDeployShip( new List<(int, int)> { (0, 2), (1, 2), (2, 2) });
        plansOfficer.TryDeployShip( new List<(int, int)> { (5, 0), (5, 1), (5, 2) });
        plansOfficer.TryDeployShip(new List<(int, int)> { (0, 4), (1, 4) });
        plansOfficer.TryDeployShip( new List<(int, int)> { (3, 4), (4, 4) });
        plansOfficer.TryDeployShip( new List<(int, int)> { (7, 0), (8, 0) });
        plansOfficer.TryDeployShip( new List<(int, int)> { (0, 6) });
        plansOfficer.TryDeployShip(new List<(int, int)> { (2, 6) });
        plansOfficer.TryDeployShip( new List<(int, int)> { (4, 6) });
        plansOfficer.TryDeployShip( new List<(int, int)> { (6, 6) });

        
    }

    public TurnRecon GetTurnRecon()
    {
        return turnRecon;
    }

    public MissionResult TacticalDirective(int targetX, int targetY)
    {
        if (targetX < 0 || targetY < 0) throw new ArgumentException("incorrect value");

        return commander.AssignMission(targetX, targetY);

    }

    public void DeployDirective( List <(int x, int y)> positions)
    {
        Ship ship = battleController.GetShip(positions.Count);

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
