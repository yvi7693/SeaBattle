using NUnit.Framework;
using System;
using System.Collections.Generic;

public class SeaTests
{
    private Fleet fleet;
    private Sea sea;


    [SetUp]
    public void CreateSea()
    {
        fleet = new Fleet();
        sea = new Sea(fleet);
    }


    [TearDown]
    public void DestroySea()
    {
        fleet = null;
        sea = null;
    }


    private void DeployEntireFleet(Sea targetSea)
    {
        Ship[] ships = targetSea.GetFleet().GetShips();

        for (int row = 0; row < ships.Length; row++)
        {
            int shipSize = ships[row].GetDurability();
            List<Sector> cells = new List<Sector>();

            for (int col = 0; col < shipSize; col++)
                cells.Add(targetSea.GetSector(col, row));

            ships[row].Deploy(cells);
        }
    }


    // =====================================================================
    // Позитивные
    // =====================================================================

    [Test]
    public void TestConstructorCreatesGridOfDefaultSize()
    {
        Assert.AreEqual(StatusSector.Empty, sea.GetSector(0, 0).GetStatus());
        Assert.AreEqual(StatusSector.Empty, sea.GetSector(9, 9).GetStatus());
    }


    [Test]
    public void TestGetFleetReturnsSameFleetInstance()
    {
        Assert.AreSame(fleet, sea.GetFleet());
    }


    [Test]
    public void TestGetSectorReturnsCorrectCoordinates()
    {
        Assert.AreEqual((3, 4), sea.GetSector(3, 4).GetCoord());
    }


    [Test]
    public void TestIsShipsDeployFalseWhenFleetNotDeployed()
    {
        Assert.IsFalse(sea.IsShipsDeploy());
    }


    [Test]
    public void TestIsShipsDeployTrueWhenEntireFleetDeployed()
    {
        DeployEntireFleet(sea);

        Assert.IsTrue(sea.IsShipsDeploy());
    }


    [Test]
    public void TestCollectSectorsReturnsSectorsInOrder()
    {
        List<(int x, int y)> positions = new List<(int, int)> { (0, 0), (1, 1), (2, 2) };

        List<Sector> collected = sea.CollectSectors(positions);

        Assert.AreSame(sea.GetSector(0, 0), collected[0]);
        Assert.AreSame(sea.GetSector(1, 1), collected[1]);
        Assert.AreSame(sea.GetSector(2, 2), collected[2]);
    }


    [Test]
    public void TestIsAttackedSectorFalseForUntouchedSector()
    {
        Assert.IsFalse(sea.IsAttackedSector(0, 0));
    }


    [Test]
    public void TestIsAttackedSectorTrueAfterStatusSet()
    {
        sea.GetSector(0, 0).SetStatus(StatusSector.Hit);

        Assert.IsTrue(sea.IsAttackedSector(0, 0));
    }


    [Test]
    public void TestIsSunkenFalseWhenNoShipOnSector()
    {
        Assert.IsFalse(sea.IsSunken(0, 0));
    }


    [Test]
    public void TestIsSunkenReflectsShipState()
    {
        Ship ship = new Ship(1);
        sea.GetSector(0, 0).Occupy(ship);

        Assert.IsFalse(sea.IsSunken(0, 0));

        ship.Damage();

        Assert.IsTrue(sea.IsSunken(0, 0));
    }


    [Test]
    public void TestClearResetsAllSectorsToEmptyOnDefaultSizeSea()
    {
        sea.GetSector(0, 0).SetStatus(StatusSector.Hit);
        sea.GetSector(5, 5).Occupy(new Ship(1));

        sea.Clear();

        Assert.AreEqual(StatusSector.Empty, sea.GetSector(0, 0).GetStatus());
        Assert.AreEqual(StatusSector.Empty, sea.GetSector(5, 5).GetStatus());
        Assert.IsFalse(sea.GetSector(5, 5).HaveShip());
    }


    [Test]
    public void TestRecallFleetResetsDeployedFlagOnAllShips()
    {
        DeployEntireFleet(sea);
        Assert.IsTrue(sea.IsShipsDeploy());

        sea.RecallFleet();

        Assert.IsFalse(sea.IsShipsDeploy());

        foreach (Ship ship in sea.GetFleet().GetShips())
            Assert.IsFalse(ship.IsDeploy());
    }


    // =====================================================================
    // Граничные
    // =====================================================================

    [Test]
    public void TestConstructorMinimalSizeTen()
    {
        Sea minimalSea = new Sea(new Fleet(), 10);

        Assert.AreEqual(StatusSector.Empty, minimalSea.GetSector(9, 9).GetStatus());
    }


    [Test]
    public void TestConstructorLargerThanDefaultSizeSucceeds()
    {
        Sea bigSea = new Sea(new Fleet(), 15);

        Assert.AreEqual(StatusSector.Empty, bigSea.GetSector(14, 14).GetStatus());
    }


    [TestCase(0, 0)]
    [TestCase(0, 1)]
    [TestCase(5, 5)]
    [TestCase(9, 0)]
    [TestCase(9, 9)]
    public void TestValidateBorderPositiveTrue(int x, int y)
    {
        Assert.IsTrue(sea.ValidateBorder(x, y));
    }


    [TestCase(0, 10)]
    [TestCase(0, -1)]
    [TestCase(-5, -5)]
    [TestCase(11, 11)]
    [TestCase(300, 300)]
    public void TestValidateBorderPositiveFalse(int x, int y)
    {
        Assert.IsFalse(sea.ValidateBorder(x, y));
    }


    [TestCase(0, 0)]
    [TestCase(9, 9)]
    public void TestGetSectorAtCornersSucceeds(int x, int y)
    {
        Assert.DoesNotThrow(() => sea.GetSector(x, y));
    }


    [Test]
    public void TestCollectSectorsWithEmptyPositionsReturnsEmptyList()
    {
        List<Sector> collected = sea.CollectSectors(new List<(int, int)>());

        Assert.AreEqual(0, collected.Count);
    }


    [Test]
    public void TestCollectSectorsWithDuplicatePositionsReturnsDuplicateReferences()
    {
        List<(int x, int y)> positions = new List<(int, int)> { (3, 3), (3, 3) };

        List<Sector> collected = sea.CollectSectors(positions);

        Assert.AreEqual(2, collected.Count);
        Assert.AreSame(collected[0], collected[1]);
    }


    [Test]
    public void TestClearDoesNotResetSectorsBeyondHardcodedTenOnLargerSea()
    {
        Sea bigSea = new Sea(new Fleet(), 15);
        bigSea.GetSector(12, 12).SetStatus(StatusSector.Hit);

        bigSea.Clear();

        Assert.AreEqual(StatusSector.Hit, bigSea.GetSector(12, 12).GetStatus());
    }


    [Test]
    public void TestIsShipsDeployFalseWhenFleetPartiallyDeployed()
    {
        Ship[] ships = sea.GetFleet().GetShips();
        List<Sector> cells = new List<Sector> { sea.GetSector(0, 0) };
        ships[0].Deploy(cells);

        Assert.IsFalse(sea.IsShipsDeploy());
    }


    // =====================================================================
    // Негативные
    // =====================================================================

    [TestCase(9)]
    [TestCase(0)]
    [TestCase(-5)]
    public void TestConstructorSizeBelowMinimumThrows(int size)
    {
        Assert.Throws<ArgumentException>(() => new Sea(new Fleet(), size));
    }


    [TestCase(-1, 0)]
    [TestCase(0, -1)]
    [TestCase(10, 0)]
    [TestCase(0, 10)]
    [TestCase(-1, -1)]
    [TestCase(10, 10)]
    public void TestGetSectorOutOfBoundsThrows(int x, int y)
    {
        Assert.Throws<ArgumentException>(() => sea.GetSector(x, y));
    }


    [Test]
    public void TestCollectSectorsWithOutOfBoundsPositionThrows()
    {
        List<(int x, int y)> positions = new List<(int, int)> { (0, 0), (99, 99) };

        Assert.Throws<ArgumentException>(() => sea.CollectSectors(positions));
    }


    [Test]
    public void TestIsSunkenOutOfBoundsThrowsArgumentExceptionLikeGetSector()
    {
        Assert.Throws<ArgumentException>(() => sea.IsSunken(-1, -1));
    }


    [Test]
    public void TestIsAttackedSectorOutOfBoundsThrowsArgumentExceptionLikeGetSector()
    {
        Assert.Throws<ArgumentException>(() => sea.IsAttackedSector(-1, -1));
    }


    [Test]
    public void TestIsShipsDeployThrowsWhenFleetIsNull()
    {
        Sea nullFleetSea = new Sea(null);

        Assert.Throws<NullReferenceException>(() => nullFleetSea.IsShipsDeploy());
    }


    [Test]
    public void TestRecallFleetThrowsWhenFleetIsNull()
    {
        Sea nullFleetSea = new Sea(null);

        Assert.Throws<NullReferenceException>(() => nullFleetSea.RecallFleet());
    }
}
