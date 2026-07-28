using NUnit.Framework;
using System;
using System.Collections.Generic;

public class AssigneeTests
{
    private Assignee assignee;
    private TurnRecon turnRecon;


    [SetUp]
    public void CreateAssignee()
    {
        DeploymentOfficer deploymentOfficer = new DeploymentOfficer();
        AttackResolver attackResolver = new AttackResolver();
        Sinker sinker = new Sinker(deploymentOfficer);
        turnRecon = new TurnRecon(new Fleet(), new Fleet());

        assignee = new Assignee(attackResolver, sinker, turnRecon);
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
    public void TestAttackOrderReturnsMissForEmptySector()
    {
        Sea targetSea = turnRecon.GetTargetSea();
        Sector target = targetSea.GetSector(3, 3);

        StatusSector result = assignee.AttackOrder(target);

        Assert.AreEqual(StatusSector.Miss, result);
        Assert.AreEqual(StatusSector.Miss, target.GetStatus());
    }


    [Test]
    public void TestAttackOrderReturnsHitForShipSectorWithoutSinking()
    {
        Sea targetSea = turnRecon.GetTargetSea();
        Ship ship = new Ship(2);
        List<Sector> cells = DeployAndOccupy(ship, targetSea, 2, 2);

        StatusSector result = assignee.AttackOrder(cells[0]);

        Assert.AreEqual(StatusSector.Hit, result);
        Assert.AreEqual(StatusSector.Hit, cells[0].GetStatus());
        Assert.AreEqual(1, ship.GetDurability());
        Assert.IsFalse(ship.IsSunken());
        Assert.AreEqual(StatusSector.Ship, cells[1].GetStatus());
    }


    [Test]
    public void TestAttackOrderSinksShipAndFloodsSurroundingSectorsWhenLastHit()
    {
        Sea targetSea = turnRecon.GetTargetSea();
        Ship ship = new Ship(1);
        List<Sector> cells = DeployAndOccupy(ship, targetSea, 5, 5);

        assignee.AttackOrder(cells[0]);

        Assert.IsTrue(ship.IsSunken());
        Assert.AreEqual(StatusSector.Miss, targetSea.GetSector(4, 4).GetStatus());
        Assert.AreEqual(StatusSector.Miss, targetSea.GetSector(6, 6).GetStatus());
    }


    [Test]
    public void TestDeployOrderOccupiesAllStationSectors()
    {
        Ship ship = new Ship(3);
        List<Sector> station = new List<Sector>
        {
            new Sector(0, 0),
            new Sector(1, 0),
            new Sector(2, 0)
        };

        assignee.DeployOrder(station, ship);

        foreach (Sector sector in station)
        {
            Assert.IsTrue(sector.HaveShip());
            Assert.AreSame(ship, sector.GetShip());
        }
    }


    // =====================================================================
    // Граничные
    // =====================================================================

    [Test]
    public void TestAttackOrderOnMultiCellShipDoesNotSinkUntilAllCellsHit()
    {
        Sea targetSea = turnRecon.GetTargetSea();
        Ship ship = new Ship(2);
        List<Sector> cells = DeployAndOccupy(ship, targetSea, 2, 2);

        assignee.AttackOrder(cells[0]);
        Assert.IsFalse(ship.IsSunken());

        assignee.AttackOrder(cells[1]);
        Assert.IsTrue(ship.IsSunken());
    }


    [Test]
    public void TestAttackOrderDoesNotFloodWhenShipNotYetSunk()
    {
        Sea targetSea = turnRecon.GetTargetSea();
        Ship ship = new Ship(2);
        List<Sector> cells = DeployAndOccupy(ship, targetSea, 2, 2);

        assignee.AttackOrder(cells[0]);

        Assert.AreEqual(StatusSector.Empty, targetSea.GetSector(1, 1).GetStatus());
    }


    [Test]
    public void TestAttackOrderAtCornerFloodsOnlyInBoundsNeighbors()
    {
        Sea targetSea = turnRecon.GetTargetSea();
        Ship ship = new Ship(1);
        List<Sector> cells = DeployAndOccupy(ship, targetSea, 0, 0);

        Assert.DoesNotThrow(() => assignee.AttackOrder(cells[0]));
        Assert.AreEqual(StatusSector.Miss, targetSea.GetSector(1, 0).GetStatus());
    }


    // =====================================================================
    // Негативные
    // =====================================================================

    [Test]
    public void TestAttackOrderOnAlreadyAttackedSectorThrows()
    {
        Sea targetSea = turnRecon.GetTargetSea();
        Sector target = targetSea.GetSector(3, 3);
        assignee.AttackOrder(target);

        Assert.Throws<Exception>(() => assignee.AttackOrder(target));
    }


    [Test]
    public void TestAttackOrderThrowsWhenShipOnSectorWasNeverDeployed()
    {
        Sea targetSea = turnRecon.GetTargetSea();
        Sector target = targetSea.GetSector(3, 3);
        target.Occupy(new Ship(1));

        Assert.Throws<Exception>(() => assignee.AttackOrder(target));
    }


    [Test]
    public void TestDeployOrderThrowsWhenSectorAlreadyOccupied()
    {
        Sector occupied = new Sector(0, 0, new Ship(1));
        List<Sector> station = new List<Sector> { occupied };

        Assert.Throws<Exception>(() => assignee.DeployOrder(station, new Ship(1)));
    }
}
