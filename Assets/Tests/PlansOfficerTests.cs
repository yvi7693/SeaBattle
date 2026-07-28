using NUnit.Framework;
using System;
using System.Collections.Generic;

public class PlansOfficerTests
{
    private BattleController battleController;
    private TurnRecon turnRecon;
    private DeploymentOfficer deploymentOfficer;
    private Assignee assignee;
    private PlansOfficer plansOfficer;


    [SetUp]
    public void CreatePlansOfficer()
    {
        battleController = new BattleController();
        deploymentOfficer = new DeploymentOfficer();
        AttackResolver attackResolver = new AttackResolver();
        Sinker sinker = new Sinker(deploymentOfficer);
        turnRecon = new TurnRecon(battleController.GetRightFleet(), battleController.GetLeftFleet());
        assignee = new Assignee(attackResolver, sinker, turnRecon);

        plansOfficer = new PlansOfficer(turnRecon, assignee, deploymentOfficer, battleController);
    }


    // =====================================================================
    // Позитивные
    // =====================================================================

    [Test]
    public void TestPlanOrderReturnsTrueForUnattackedSector()
    {
        (bool permission, Sector sector) = plansOfficer.PlanOrder(3, 3);

        Assert.IsTrue(permission);
        Assert.AreSame(turnRecon.GetTargetSea().GetSector(3, 3), sector);
    }


    [Test]
    public void TestPlanOrderReturnsFalseForAlreadyAttackedSector()
    {
        turnRecon.GetTargetSea().GetSector(3, 3).SetStatus(StatusSector.Miss);

        (bool permission, Sector sector) = plansOfficer.PlanOrder(3, 3);

        Assert.IsFalse(permission);
        Assert.AreSame(turnRecon.GetTargetSea().GetSector(3, 3), sector);
    }


    [Test]
    public void TestTryDeployShipReturnsTrueAndDeploysValidPlacement()
    {
        Sea deploySea = turnRecon.GetDeploySea();
        List<(int x, int y)> positions = new List<(int, int)> { (0, 0) };

        bool result = plansOfficer.TryDeployShip(deploySea, positions);

        Assert.IsTrue(result);
        Assert.IsTrue(deploySea.GetSector(0, 0).HaveShip());
    }


    [Test]
    public void TestDeployFleetFullyDeploysFirstAvailableFleet()
    {
        plansOfficer.DeployFleet();

        Assert.IsTrue(battleController.GetRightFleet().IsDeployed());

        foreach (Ship ship in battleController.GetRightFleet().GetShips())
            Assert.IsTrue(ship.IsDeploy());
    }


    // =====================================================================
    // Граничные
    // =====================================================================

    [Test]
    public void TestPlanOrderAfterSwitchTargetSeaQueriesNewTargetSea()
    {
        turnRecon.SwitchTargetSea();

        (bool permission, Sector sector) = plansOfficer.PlanOrder(4, 4);

        Assert.AreSame(turnRecon.GetTargetSea().GetSector(4, 4), sector);
        Assert.AreNotSame(turnRecon.GetAttackerSea().GetSector(4, 4), sector);
    }


    [Test]
    public void TestTryDeployShipReturnsFalseWhenPositionOccupiedLeavesShipUndeployed()
    {
        Sea deploySea = turnRecon.GetDeploySea();
        deploySea.GetSector(5, 5).Occupy(new Ship(1));

        bool result = plansOfficer.TryDeployShip(deploySea, new List<(int, int)> { (5, 5) });

        Assert.IsFalse(result);
    }


    [Test]
    public void TestTryDeployShipReturnsFalseWhenNeighborOccupied()
    {
        Sea deploySea = turnRecon.GetDeploySea();
        deploySea.GetSector(5, 6).Occupy(new Ship(1));

        bool result = plansOfficer.TryDeployShip(deploySea, new List<(int, int)> { (5, 5) });

        Assert.IsFalse(result);
    }


    [Test]
    public void TestTryDeployShipConsumesFirstAvailableShipOfMatchingSize()
    {
        Sea deploySea = turnRecon.GetDeploySea();

        plansOfficer.TryDeployShip(deploySea, new List<(int, int)> { (0, 0) });

        Ship firstShip = deploySea.GetSector(0, 0).GetShip();

        plansOfficer.TryDeployShip(deploySea, new List<(int, int)> { (5, 5) });

        Ship secondShip = deploySea.GetSector(5, 5).GetShip();

        Assert.AreNotSame(firstShip, secondShip);
    }


    [Test]
    public void TestDeployFleetDeploysSecondFleetAfterFirstIsDone()
    {
        plansOfficer.DeployFleet();
        plansOfficer.DeployFleet();

        Assert.IsTrue(battleController.GetRightFleet().IsDeployed());
        Assert.IsTrue(battleController.GetLeftFleet().IsDeployed());
    }


    // =====================================================================
    // Негативные
    // =====================================================================

    [Test]
    public void TestPlanOrderThrowsForOutOfBoundsCoordinates()
    {
        Assert.Throws<ArgumentException>(() => plansOfficer.PlanOrder(99, 99));
    }


    [Test]
    public void TestTryDeployShipThrowsWhenNoShipMatchesPositionsCount()
    {
        Sea deploySea = turnRecon.GetDeploySea();
        List<(int x, int y)> positions = new List<(int, int)> { (0, 0), (1, 0), (2, 0), (3, 0), (4, 0) };

        Assert.Throws<ArgumentException>(() => plansOfficer.TryDeployShip(deploySea, positions));
    }


    [Test]
    public void TestTryDeployShipThrowsForOutOfBoundsPosition()
    {
        Sea deploySea = turnRecon.GetDeploySea();

        Assert.Throws<ArgumentException>(() => plansOfficer.TryDeployShip(deploySea, new List<(int, int)> { (99, 99) }));
    }


    [Test]
    public void TestDeployFleetThrowsWhenBothFleetsAlreadyDeployed()
    {
        plansOfficer.DeployFleet();
        plansOfficer.DeployFleet();

        Assert.Throws<Exception>(() => plansOfficer.DeployFleet());
    }
}
