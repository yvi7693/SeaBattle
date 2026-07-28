using NUnit.Framework;
using System;


public class SectorTests
{

    private Sector sector;

    [SetUp]
    public void CreateSector()
    {
        sector = new Sector(0, 0);
    }

    [TearDown]
    public void DestroySector()
    {
        sector = null;
    }


    // =====================================================================
    // Позитивные
    // =====================================================================

    [Test]
    public void TestNewSectorIsEmptyByDefault()
    {
        Assert.AreEqual(StatusSector.Empty, sector.GetStatus());
        Assert.IsTrue(sector.IsEmpty());
        Assert.IsFalse(sector.IsAttacked());
        Assert.IsFalse(sector.IsHit());
        Assert.IsFalse(sector.HaveShip());
        Assert.IsNull(sector.GetShip());
    }


    [Test]
    public void TestConstructorWithShipSetsStatusShip()
    {
        Ship ship = new Ship(1);
        Sector occupiedSector = new Sector(2, 3, ship);

        Assert.AreEqual(StatusSector.Ship, occupiedSector.GetStatus());
        Assert.IsTrue(occupiedSector.HaveShip());
        Assert.AreEqual(ship, occupiedSector.GetShip());
    }


    [Test]
    public void TestGetCoordReturnsConstructorValues()
    {
        Sector coordSector = new Sector(4, 7);

        Assert.AreEqual((4, 7), coordSector.GetCoord());
    }


    [Test]
    public void TestSetStatusToHit()
    {
        sector.SetStatus(StatusSector.Hit);

        Assert.AreEqual(StatusSector.Hit, sector.GetStatus());
        Assert.IsTrue(sector.IsHit());
        Assert.IsTrue(sector.IsAttacked());
    }


    [Test]
    public void TestSetStatusToMiss()
    {
        sector.SetStatus(StatusSector.Miss);

        Assert.AreEqual(StatusSector.Miss, sector.GetStatus());
        Assert.IsTrue(sector.IsAttacked());
        Assert.IsFalse(sector.IsHit());
    }


    [Test]
    public void TestOccupySetsShipAndStatus()
    {
        Ship ship = new Ship(1);

        sector.Occupy(ship);

        Assert.IsTrue(sector.HaveShip());
        Assert.AreEqual(ship, sector.GetShip());
        Assert.AreEqual(StatusSector.Ship, sector.GetStatus());
    }


    [Test]
    public void TestRecallShipClearsShipReference()
    {
        sector.Occupy(new Ship(1));

        sector.RecallShip();

        Assert.IsFalse(sector.HaveShip());
        Assert.IsNull(sector.GetShip());
    }


    // =====================================================================
    // Граничные
    // =====================================================================

    [TestCase(StatusSector.Empty)]
    [TestCase(StatusSector.Ship)]
    [TestCase(StatusSector.Miss)]
    [TestCase(StatusSector.Hit)]
    public void TestSetStatusAcceptsAllEnumValues(StatusSector status)
    {
        sector.SetStatus(status);

        Assert.AreEqual(status, sector.GetStatus());
    }


    [Test]
    public void TestConstructorAcceptsNegativeCoordinates()
    {
        Sector negativeSector = new Sector(-5, -5);

        Assert.AreEqual((-5, -5), negativeSector.GetCoord());
    }


    [Test]
    public void TestRecallShipOnSectorWithoutShipDoesNotThrow()
    {
        Assert.DoesNotThrow(() => sector.RecallShip());
        Assert.IsFalse(sector.HaveShip());
    }


    [Test]
    public void TestSetStatusOverwritesPreviousStatus()
    {
        sector.SetStatus(StatusSector.Hit);
        sector.SetStatus(StatusSector.Miss);

        Assert.AreEqual(StatusSector.Miss, sector.GetStatus());
    }


    [Test]
    public void TestRecallShipLeavesStatusInconsistentWithNoShip()
    {
        sector.Occupy(new Ship(1));

        sector.RecallShip();

        Assert.IsFalse(sector.HaveShip());
        Assert.AreEqual(StatusSector.Ship, sector.GetStatus());
    }


    [Test]
    public void TestOccupyWithNullShipCreatesInconsistentState()
    {
        Assert.DoesNotThrow(() => sector.Occupy(null));

        Assert.AreEqual(StatusSector.Ship, sector.GetStatus());
        Assert.IsFalse(sector.HaveShip());
    }


    [Test]
    public void TestOccupySucceedsEvenAfterStatusWasSetToHit()
    {
        sector.SetStatus(StatusSector.Hit);

        sector.Occupy(new Ship(1));

        Assert.IsTrue(sector.HaveShip());
        Assert.AreEqual(StatusSector.Ship, sector.GetStatus());
    }


    // =====================================================================
    // Негативные
    // =====================================================================

    [Test]
    public void TestOccupyOnAlreadyOccupiedSectorThrows()
    {
        sector.Occupy(new Ship(1));

        Assert.Throws<Exception>(() => sector.Occupy(new Ship(1)));
    }
}
