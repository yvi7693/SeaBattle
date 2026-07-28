using NUnit.Framework;
using System;
using System.Collections.Generic;

public class FleetTests
{
    private Fleet fleet;


    [SetUp]
    public void CreateFleet()
    {
        fleet = new Fleet();
    }


    [TearDown]
    public void DestroyFleet()
    {
        fleet = null;
    }


    private void DeployShip(Ship ship)
    {
        List<Sector> cells = new List<Sector>();

        for (int i = 0; i < ship.GetSize(); i++)
            cells.Add(new Sector(i, 0));

        ship.Deploy(cells);
    }


    // =====================================================================
    // Позитивные
    // =====================================================================

    [Test]
    public void TestConstructorCreatesTenShips()
    {
        Assert.AreEqual(10, fleet.GetShips().Length);
    }


    [TestCase(1, 4)]
    [TestCase(2, 3)]
    [TestCase(3, 2)]
    [TestCase(4, 1)]
    public void TestCalculateCountReturnsCorrectCountForEachSize(int size, int expectedCount)
    {
        Assert.AreEqual(expectedCount, fleet.CalculateCount(size));
    }


    [Test]
    public void TestGetShipReturnsShipWithMatchingDurability()
    {
        Ship ship = fleet.GetShip(1);

        Assert.AreEqual(1, ship.GetDurability());
        Assert.IsFalse(ship.IsDeploy());
    }


    [Test]
    public void TestGetShipReturnsSameShipOnRepeatedCallsWithoutDeploying()
    {
        Ship first = fleet.GetShip(1);
        Ship second = fleet.GetShip(1);

        Assert.AreSame(first, second);
    }


    [Test]
    public void TestGetShipAfterDeployingFirstReturnsNextShip()
    {
        Ship first = fleet.GetShip(1);
        DeployShip(first);

        Ship second = fleet.GetShip(1);

        Assert.AreNotSame(first, second);
        Assert.AreEqual(1, second.GetDurability());
    }


    [Test]
    public void TestHasSurvivorsTrueWhenFleetFresh()
    {
        Assert.IsTrue(fleet.HasSurvivors());
    }


    [Test]
    public void TestIsDeployedFalseWhenFleetFresh()
    {
        Assert.IsFalse(fleet.IsDeployed());
    }


    [Test]
    public void TestRecallResetsAllShipsDeployedFlag()
    {
        DeployShip(fleet.GetShip(1));
        DeployShip(fleet.GetShip(2));

        fleet.Recall();

        foreach (Ship ship in fleet.GetShips())
            Assert.IsFalse(ship.IsDeploy());
    }


    // =====================================================================
    // Граничные
    // =====================================================================

    [TestCase(0)]
    [TestCase(5)]
    [TestCase(100)]
    public void TestCalculateCountForSizeNotPresentReturnsZero(int size)
    {
        Assert.AreEqual(0, fleet.CalculateCount(size));
    }


    [Test]
    public void TestCalculateCountExcludesSunkenShips()
    {
        foreach (Ship ship in fleet.GetShips())
        {
            if (ship.GetSize() == 1)
            {
                DeployShip(ship);
                ship.Damage();
            }
        }

        Assert.AreEqual(0, fleet.CalculateCount(1));
    }


    [Test]
    public void TestGetShipsReturnsSameArrayReferenceAndReflectsMutation()
    {
        Ship[] first = fleet.GetShips();
        Ship[] second = fleet.GetShips();

        Assert.AreSame(first, second);

        DeployShip(first[0]);
        first[0].Damage();

        Assert.AreEqual(0, second[0].GetDurability());
    }


    [Test]
    public void TestIsDeployedTrueWhenAllTenShipsDeployed()
    {
        foreach (Ship ship in fleet.GetShips())
            DeployShip(ship);

        Assert.IsTrue(fleet.IsDeployed());
    }


    [Test]
    public void TestGetShipThrowsWhenAllShipsOfDurabilityAreDeployed()
    {
        for (int i = 0; i < 4; i++)
            DeployShip(fleet.GetShip(1));

        Assert.Throws<ArgumentException>(() => fleet.GetShip(1));
    }


    [Test]
    public void TestHasSurvivorsFalseWhenAllShipsSunk()
    {
        foreach (Ship ship in fleet.GetShips())
        {
            DeployShip(ship);

            for (int i = 0; i < ship.GetSize(); i++)
                ship.Damage();
        }

        Assert.IsFalse(fleet.HasSurvivors());
    }


    [Test]
    public void TestHasSurvivorsTrueWhenOnlySomeShipsSunk()
    {
        Ship oneShip = fleet.GetShips()[0];
        DeployShip(oneShip);
        oneShip.Damage();

        Assert.IsTrue(fleet.HasSurvivors());
    }


    [Test]
    public void TestRecallDoesNotResetDurabilityOfAnyShip()
    {
        Ship damagedShip = fleet.GetShips()[9];
        DeployShip(damagedShip);
        damagedShip.Damage();
        DeployShip(fleet.GetShip(3));

        fleet.Recall();

        Assert.AreEqual(3, damagedShip.GetDurability());
    }


    [Test]
    public void TestGetShipMatchesByCurrentDurabilityNotOriginalSize()
    {
        for (int i = 0; i < 4; i++)
            DeployShip(fleet.GetShip(1));

        Ship damagedShip = fleet.GetShip(2);
        DeployShip(damagedShip);
        damagedShip.Damage();
        damagedShip.Recall();

        Ship found = fleet.GetShip(1);

        Assert.AreSame(damagedShip, found);
        Assert.AreEqual(2, found.GetSize());
    }


    // =====================================================================
    // Негативные
    // =====================================================================

    [TestCase(99)]
    [TestCase(-1)]
    [TestCase(0)]
    public void TestGetShipThrowsForNonExistentDurability(int durability)
    {
        Assert.Throws<ArgumentException>(() => fleet.GetShip(durability));
    }


    [Test]
    public void TestGetShipThrowsForSunkenShipDurability()
    {
        Ship ship = fleet.GetShips()[0];
        DeployShip(ship);
        ship.Damage();
        ship.Recall();

        Assert.IsTrue(ship.IsSunken());
        Assert.IsFalse(ship.IsDeploy());
        Assert.Throws<ArgumentException>(() => fleet.GetShip(0));
    }
}
