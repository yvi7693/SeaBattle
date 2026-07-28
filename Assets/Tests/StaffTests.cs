using NUnit.Framework;
using System;
using System.Collections.Generic;

public class StaffTests
{
    private Staff staff;


    [SetUp]
    public void CreateStaff()
    {
        staff = new Staff();
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


    // =====================================================================
    // Позитивные
    // =====================================================================

    [Test]
    public void TestConstructorWiresGettersToConsistentInstances()
    {
        Assert.IsNotNull(staff.GetTurnRecon());
        Assert.IsNotNull(staff.GetBattleController());
        Assert.IsNotNull(staff.GetDeploymentOfficer());
        Assert.IsNotNull(staff.GetHomingWeapon());

        Assert.AreSame(staff.GetTurnRecon(), staff.GetTurnRecon());
        Assert.AreSame(staff.GetBattleController(), staff.GetBattleController());
    }


    [Test]
    public void TestDeployDirectiveDeploysShipOnDeploySea()
    {
        Sea deploySea = staff.GetTurnRecon().GetDeploySea();

        staff.DeployDirective(new List<(int x, int y)> { (0, 0) });

        Assert.IsTrue(deploySea.GetSector(0, 0).HaveShip());
    }


    [Test]
    public void TestDeployFleetFullyDeploysFirstFleet()
    {
        staff.DeployFleet();

        Assert.IsTrue(staff.GetBattleController().GetRightFleet().IsDeployed());
    }


    [Test]
    public void TestTacticalDirectiveReturnsMissForEmptyTargetSector()
    {
        MissionResult result = staff.TacticalDirective(3, 3);

        Assert.AreEqual(MissionResult.Miss, result);
    }


    [Test]
    public void TestTacticalDirectiveReturnsHitForDeployedShipSector()
    {
        staff.DeployDirective(new List<(int x, int y)> { (0, 0) });

        MissionResult result = staff.TacticalDirective(0, 0);

        Assert.AreEqual(MissionResult.Hit, result);
    }


    // =====================================================================
    // Граничные
    // =====================================================================

    [Test]
    public void TestTacticalDirectiveSwitchesTargetSeaOnMiss()
    {
        Sea seaBeforeMiss = staff.GetTurnRecon().GetTargetSea();

        staff.TacticalDirective(3, 3);

        Assert.AreNotSame(seaBeforeMiss, staff.GetTurnRecon().GetTargetSea());
    }


    [Test]
    public void TestDeployDirectiveOnOccupiedNeighborSilentlyDoesNothing()
    {
        Sea deploySea = staff.GetTurnRecon().GetDeploySea();
        deploySea.GetSector(5, 6).Occupy(new Ship(1));

        Assert.DoesNotThrow(() => staff.DeployDirective(new List<(int x, int y)> { (5, 5) }));
        Assert.IsFalse(deploySea.GetSector(5, 5).HaveShip());
    }


    [Test]
    public void TestTacticalDirectiveOnAlreadyAttackedSectorReturnsUnsucessfulShot()
    {
        staff.TacticalDirective(3, 3);

        MissionResult result = staff.TacticalDirective(3, 3);

        Assert.AreEqual(MissionResult.UnsucessfulShot, result);
    }


    [Test]
    public void TestTacticalDirectiveReturnsHaveWinnerWhenLastShipOfFleetSinksEndToEnd()
    {
        Fleet targetFleet = staff.GetBattleController().GetLeftFleet();
        Sea targetSea = staff.GetTurnRecon().GetLeftSea();
        Ship[] ships = targetFleet.GetShips();

        for (int row = 0; row < ships.Length - 1; row++)
        {
            Ship ship = ships[row];
            List<Sector> cells = DeployAndOccupy(ship, targetSea, 0, row);

            for (int i = 0; i < ship.GetSize(); i++)
                ship.Damage();
        }

        Ship lastShip = ships[9];
        List<Sector> lastCells = DeployAndOccupy(lastShip, targetSea, 0, 9);

        MissionResult result = MissionResult.Miss;

        foreach (Sector cell in lastCells)
        {
            (int x, int y) = cell.GetCoord();
            result = staff.TacticalDirective(x, y);
        }

        Assert.AreEqual(MissionResult.HaveWinner, result);
        Assert.AreEqual(PlayerName.Player1, staff.GetBattleController().GetWinner());
    }


    // =====================================================================
    // Негативные
    // =====================================================================

    [Test]
    public void TestTacticalDirectiveThrowsForNegativeXCoordinate()
    {
        Assert.Throws<ArgumentException>(() => staff.TacticalDirective(-1, 0));
    }


    [Test]
    public void TestTacticalDirectiveThrowsForNegativeYCoordinate()
    {
        Assert.Throws<ArgumentException>(() => staff.TacticalDirective(0, -1));
    }


    [Test]
    public void TestTacticalDirectiveDoesNotGuardAgainstTooLargeCoordinates()
    {
        // TacticalDirective only checks for negative coordinates - values beyond the board
        // are not guarded by Staff itself and only fail deeper, inside Sea.GetSector
        Assert.Throws<ArgumentException>(() => staff.TacticalDirective(99, 99));
    }
}
