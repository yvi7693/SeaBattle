using NUnit.Framework;
using System;
using System.Collections.Generic;

public class CommanderTests
{
    private BattleController battleController;
    private TurnRecon turnRecon;
    private Commander commander;


    [SetUp]
    public void CreateCommander()
    {
        battleController = new BattleController();
        DeploymentOfficer deploymentOfficer = new DeploymentOfficer();
        AttackResolver attackResolver = new AttackResolver();
        Sinker sinker = new Sinker(deploymentOfficer);
        turnRecon = new TurnRecon(battleController.GetRightFleet(), battleController.GetLeftFleet());
        Assignee assignee = new Assignee(attackResolver, sinker, turnRecon);
        PlansOfficer plansOfficer = new PlansOfficer(turnRecon, assignee, deploymentOfficer, battleController);

        commander = new Commander(plansOfficer, assignee, turnRecon, battleController);
    }


    private List<Sector> DeployAndOccupy(Ship ship, Sea sea, int startX, int startY)
    {
        List<Sector> cells = new List<Sector>();

        for (int i = 0; i < ship.GetSize(); i++)
            cells.Add(sea.GetSector(startX + i, startY));

        ship.Deploy(cells);

        foreach (Sector cell in cells)
            cell.Occupy(ship);

        return cells;
    }


    private void SinkAllShipsExceptLast(Fleet fleet, Sea sea)
    {
        Ship[] ships = fleet.GetShips();

        for (int row = 0; row < ships.Length - 1; row++)
        {
            Ship ship = ships[row];
            List<Sector> cells = DeployAndOccupy(ship, sea, 0, row);

            for (int i = 0; i < ship.GetSize(); i++)
                ship.Damage();
        }
    }


    // =====================================================================
    // Позитивные
    // =====================================================================

    [Test]
    public void TestAssignMissionReturnsMissForEmptySectorAndSwitchesTargetSea()
    {
        Sea originalTargetSea = turnRecon.GetTargetSea();

        MissionResult result = commander.AssignMission(3, 3);

        Assert.AreEqual(MissionResult.Miss, result);
        Assert.AreNotSame(originalTargetSea, turnRecon.GetTargetSea());
    }


    [Test]
    public void TestAssignMissionReturnsHitForShipCellWithoutSinkingAndDoesNotSwitchTargetSea()
    {
        Sea targetSea = turnRecon.GetTargetSea();
        Ship ship = new Ship(2);
        List<Sector> cells = DeployAndOccupy(ship, targetSea, 2, 2);
        (int x, int y) = cells[0].GetCoord();

        MissionResult result = commander.AssignMission(x, y);

        Assert.AreEqual(MissionResult.Hit, result);
        Assert.AreSame(targetSea, turnRecon.GetTargetSea());
    }


    [Test]
    public void TestAssignMissionReturnsHaveWinnerWhenLastShipOfFleetSinks()
    {
        Fleet targetFleet = battleController.GetLeftFleet();
        Sea targetSea = turnRecon.GetLeftSea();
        SinkAllShipsExceptLast(targetFleet, targetSea);

        Ship lastShip = targetFleet.GetShips()[9];
        List<Sector> cells = DeployAndOccupy(lastShip, targetSea, 0, 9);

        MissionResult result = MissionResult.Miss;

        foreach (Sector cell in cells)
        {
            (int x, int y) = cell.GetCoord();
            result = commander.AssignMission(x, y);
        }

        Assert.AreEqual(MissionResult.HaveWinner, result);
        Assert.AreEqual(PlayerName.Player1, battleController.GetWinner());
    }


    // =====================================================================
    // Граничные
    // =====================================================================

    [Test]
    public void TestAssignMissionReturnsUnsucessfulShotForAlreadyAttackedSector()
    {
        commander.AssignMission(3, 3);

        MissionResult result = commander.AssignMission(3, 3);

        Assert.AreEqual(MissionResult.UnsucessfulShot, result);
    }


    [Test]
    public void TestAssignMissionUnsucessfulShotDoesNotSwitchTargetSea()
    {
        commander.AssignMission(3, 3);
        Sea targetSeaAfterMiss = turnRecon.GetTargetSea();

        commander.AssignMission(3, 3);

        Assert.AreSame(targetSeaAfterMiss, turnRecon.GetTargetSea());
    }


    [Test]
    public void TestAssignMissionSinkingOneShipWithoutDecidingGameReturnsHit()
    {
        Sea targetSea = turnRecon.GetTargetSea();
        Ship ship = new Ship(1);
        List<Sector> cells = DeployAndOccupy(ship, targetSea, 5, 5);
        (int x, int y) = cells[0].GetCoord();

        MissionResult result = commander.AssignMission(x, y);

        Assert.AreEqual(MissionResult.Hit, result);
        Assert.IsTrue(ship.IsSunken());
        Assert.IsFalse(battleController.IsDeclareWinner());
    }


    // =====================================================================
    // Негативные
    // =====================================================================

    [Test]
    public void TestAssignMissionThrowsForOutOfBoundsCoordinates()
    {
        Assert.Throws<ArgumentException>(() => commander.AssignMission(99, 99));
    }
}
