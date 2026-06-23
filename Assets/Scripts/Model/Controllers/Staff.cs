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
        assignee = new Assignee(attackResolver, sinker);
        deploymentOfficer = new DeploymentOfficer();
        turnRecon = new TurnRecon(battleController.GetFleet1(), battleController.GetFleet2());
        plansOfficer = new PlansOfficer(turnRecon, assignee, deploymentOfficer);
        commander = new Commander(plansOfficer, assignee);

        // временный код

        Fleet fleet1 = battleController.GetFleet1();
        Fleet fleet2 = battleController.GetFleet2();

        plansOfficer.TryDeployShip(fleet1.GetShip(9), new List<(int, int)> { (0, 0), (1, 0), (2, 0), (3, 0) });
        plansOfficer.TryDeployShip(fleet1.GetShip(8), new List<(int, int)> { (0, 2), (1, 2), (2, 2) });
        plansOfficer.TryDeployShip(fleet1.GetShip(7), new List<(int, int)> { (5, 0), (5, 1), (5, 2) });
        plansOfficer.TryDeployShip(fleet1.GetShip(6), new List<(int, int)> { (0, 4), (1, 4) });
        plansOfficer.TryDeployShip(fleet1.GetShip(5), new List<(int, int)> { (3, 4), (4, 4) });
        plansOfficer.TryDeployShip(fleet1.GetShip(4), new List<(int, int)> { (7, 0), (8, 0) });
        plansOfficer.TryDeployShip(fleet1.GetShip(3), new List<(int, int)> { (0, 6) });
        plansOfficer.TryDeployShip(fleet1.GetShip(2), new List<(int, int)> { (2, 6) });
        plansOfficer.TryDeployShip(fleet1.GetShip(1), new List<(int, int)> { (4, 6) });
        plansOfficer.TryDeployShip(fleet1.GetShip(0), new List<(int, int)> { (6, 6) });

        plansOfficer.TryDeployShip(fleet2.GetShip(9), new List<(int, int)> { (0, 0), (1, 0), (2, 0), (3, 0) });
        plansOfficer.TryDeployShip(fleet2.GetShip(8), new List<(int, int)> { (0, 2), (1, 2), (2, 2) });
        plansOfficer.TryDeployShip(fleet2.GetShip(7), new List<(int, int)> { (5, 0), (5, 1), (5, 2) });
        plansOfficer.TryDeployShip(fleet2.GetShip(6), new List<(int, int)> { (0, 4), (1, 4) });
        plansOfficer.TryDeployShip(fleet2.GetShip(5), new List<(int, int)> { (3, 4), (4, 4) });
        plansOfficer.TryDeployShip(fleet2.GetShip(4), new List<(int, int)> { (7, 0), (8, 0) });
        plansOfficer.TryDeployShip(fleet2.GetShip(3), new List<(int, int)> { (0, 6) });
        plansOfficer.TryDeployShip(fleet2.GetShip(2), new List<(int, int)> { (2, 6) });
        plansOfficer.TryDeployShip(fleet2.GetShip(1), new List<(int, int)> { (4, 6) });
        plansOfficer.TryDeployShip(fleet2.GetShip(0), new List<(int, int)> { (6, 6) });

        
    }

    public MissionResult TacticalDirective(int targetX, int targetY)
    {
        return commander.AssignMission(targetX, targetY);

    }
}

public enum MissionResult
{
    HaveWinner,
    Hit,
    Miss,
    UnsucessfulShot
}