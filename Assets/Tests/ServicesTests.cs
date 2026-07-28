using NUnit.Framework;
using System;
using System.Collections.Generic;

public class AttackResolverTests
{
    private AttackResolver resolver;
    private Sector sector;


    [SetUp]
    public void CreateResolver()
    {
        resolver = new AttackResolver();
        sector = new Sector(0, 0);
    }


    // =====================================================================
    // Позитивные
    // =====================================================================

    [Test]
    public void TestResolveReturnsMissForEmptySector()
    {
        Assert.AreEqual(StatusSector.Miss, resolver.Resolve(sector));
    }


    [Test]
    public void TestResolveReturnsHitForSectorWithShip()
    {
        Sector shipSector = new Sector(1, 1, new Ship(1));

        Assert.AreEqual(StatusSector.Hit, resolver.Resolve(shipSector));
    }


    [Test]
    public void TestResolveDoesNotMutateSectorStatus()
    {
        resolver.Resolve(sector);

        Assert.AreEqual(StatusSector.Empty, sector.GetStatus());
    }


    // =====================================================================
    // Граничные
    // =====================================================================

    [Test]
    public void TestResolveOnManuallySetShipStatusReturnsHit()
    {
        sector.SetStatus(StatusSector.Ship);

        Assert.AreEqual(StatusSector.Hit, resolver.Resolve(sector));
    }


    // =====================================================================
    // Негативные
    // =====================================================================

    [Test]
    public void TestResolveOnHitSectorThrows()
    {
        sector.SetStatus(StatusSector.Hit);

        Assert.Throws<Exception>(() => resolver.Resolve(sector));
    }


    [Test]
    public void TestResolveOnMissSectorThrows()
    {
        sector.SetStatus(StatusSector.Miss);

        Assert.Throws<Exception>(() => resolver.Resolve(sector));
    }
}


public class DeploymentOfficerTests
{
    private DeploymentOfficer deploymentOfficer;
    private Sea sea;


    [SetUp]
    public void CreateDeploymentOfficer()
    {
        deploymentOfficer = new DeploymentOfficer();
        sea = new Sea(new Fleet());
    }


    private List<Sector> GetLine(int startX, int startY, int count, bool horizontal)
    {
        List<Sector> sectors = new List<Sector>();

        for (int i = 0; i < count; i++)
        {
            if (horizontal)
                sectors.Add(sea.GetSector(startX + i, startY));
            else
                sectors.Add(sea.GetSector(startX, startY + i));
        }

        return sectors;
    }


    // =====================================================================
    // Позитивные
    // =====================================================================

    [Test]
    public void TestValidateDeployTrueForIsolatedEmptyPosition()
    {
        List<(int, int)> positions = new List<(int, int)> { (5, 5) };

        Assert.IsTrue(deploymentOfficer.ValidateDeploy(sea, positions));
    }


    [Test]
    public void TestValidatePlaceTrueForValidHorizontalLine()
    {
        List<Sector> sectors = GetLine(2, 2, 3, true);

        Assert.IsTrue(deploymentOfficer.ValidatePlace(sectors));
    }


    [Test]
    public void TestValidateEqualSectorsTrueForDistinctSectors()
    {
        List<Sector> sectors = GetLine(0, 0, 3, true);

        Assert.IsTrue(deploymentOfficer.ValidateEqualSectors(sectors));
    }


    [Test]
    public void TestValidateNearbySectorsTrueForHorizontalLine()
    {
        List<Sector> sectors = GetLine(0, 0, 4, true);

        Assert.IsTrue(deploymentOfficer.ValidateNearbySectors(sectors));
    }


    [Test]
    public void TestValidateNearbySectorsTrueForVerticalLine()
    {
        List<Sector> sectors = GetLine(0, 0, 4, false);

        Assert.IsTrue(deploymentOfficer.ValidateNearbySectors(sectors));
    }


    [Test]
    public void TestGetNearbySectorAtCenterReturnsNineSectorsIncludingSelf()
    {
        Sector center = sea.GetSector(5, 5);

        List<Sector> nearby = deploymentOfficer.GetNearbySector(sea, center);

        Assert.AreEqual(9, nearby.Count);
        Assert.Contains(center, nearby);
    }


    [Test]
    public void TestNormalizeCoordsSortsByXThenY()
    {
        List<(int x, int y)> coords = new List<(int, int)> { (2, 1), (0, 5), (0, 1) };

        List<(int, int)> normalized = deploymentOfficer.NormalizeCoords(coords);

        Assert.AreEqual((0, 1), normalized[0]);
        Assert.AreEqual((0, 5), normalized[1]);
        Assert.AreEqual((2, 1), normalized[2]);
    }


    // =====================================================================
    // Граничные
    // =====================================================================

    [Test]
    public void TestValidateDeploySingleCellShipValidOnFreshBoard()
    {
        List<(int, int)> positions = new List<(int, int)> { (0, 0) };

        Assert.IsTrue(deploymentOfficer.ValidateDeploy(sea, positions));
    }


    [Test]
    public void TestValidateDeployFalseWhenTargetSectorAlreadyOccupied()
    {
        sea.GetSector(4, 4).Occupy(new Ship(1));

        Assert.IsFalse(deploymentOfficer.ValidateDeploy(sea, new List<(int, int)> { (4, 4) }));
    }


    [Test]
    public void TestValidateDeployFalseWhenNeighborSectorOccupied()
    {
        sea.GetSector(4, 5).Occupy(new Ship(1));

        Assert.IsFalse(deploymentOfficer.ValidateDeploy(sea, new List<(int, int)> { (4, 4) }));
    }


    [Test]
    public void TestValidateNearbySectorsTrueForSingleSector()
    {
        List<Sector> sectors = new List<Sector> { sea.GetSector(3, 3) };

        Assert.IsTrue(deploymentOfficer.ValidateNearbySectors(sectors));
    }


    [Test]
    public void TestValidateNearbySectorsFalseWhenGapInLine()
    {
        List<Sector> sectors = new List<Sector> { sea.GetSector(0, 0), sea.GetSector(0, 2) };

        Assert.IsFalse(deploymentOfficer.ValidateNearbySectors(sectors));
    }


    [Test]
    public void TestGetNearbySectorAtCornerReturnsFourSectors()
    {
        List<Sector> nearby = deploymentOfficer.GetNearbySector(sea, sea.GetSector(0, 0));

        Assert.AreEqual(4, nearby.Count);
    }


    [Test]
    public void TestGetNearbySectorAtEdgeReturnsSixSectors()
    {
        List<Sector> nearby = deploymentOfficer.GetNearbySector(sea, sea.GetSector(0, 5));

        Assert.AreEqual(6, nearby.Count);
    }


    [Test]
    public void TestNormalizeCoordsMutatesAndReturnsSameListReference()
    {
        List<(int x, int y)> coords = new List<(int, int)> { (5, 5), (1, 1) };

        List<(int, int)> normalized = deploymentOfficer.NormalizeCoords(coords);

        Assert.AreSame(coords, normalized);
        Assert.AreEqual((1, 1), coords[0]);
    }


    // =====================================================================
    // Негативные
    // =====================================================================

    [Test]
    public void TestValidateNearbySectorsFalseWhenScattered()
    {
        List<Sector> sectors = new List<Sector> { sea.GetSector(0, 0), sea.GetSector(5, 5) };

        Assert.IsFalse(deploymentOfficer.ValidateNearbySectors(sectors));
    }


    [Test]
    public void TestValidateEqualSectorsFalseForDuplicateSector()
    {
        Sector duplicate = sea.GetSector(2, 2);
        List<Sector> sectors = new List<Sector> { duplicate, duplicate };

        Assert.IsFalse(deploymentOfficer.ValidateEqualSectors(sectors));
    }


    [Test]
    public void TestValidateEqualSectorsFalseForNullEntry()
    {
        List<Sector> sectors = new List<Sector> { sea.GetSector(2, 2), null };

        Assert.IsFalse(deploymentOfficer.ValidateEqualSectors(sectors));
    }


    [Test]
    public void TestValidatePlaceFalseWhenSectorsHaveGap()
    {
        List<Sector> sectors = new List<Sector>
        {
            sea.GetSector(0, 0),
            sea.GetSector(0, 1),
            sea.GetSector(0, 3)
        };

        Assert.IsFalse(deploymentOfficer.ValidatePlace(sectors));
    }


    [Test]
    public void TestValidateDeployThrowsForOutOfBoundsPosition()
    {
        List<(int, int)> positions = new List<(int, int)> { (99, 99) };

        Assert.Throws<ArgumentException>(() => deploymentOfficer.ValidateDeploy(sea, positions));
    }
}


public class SinkerTests
{
    private DeploymentOfficer deploymentOfficer;
    private Sinker sinker;
    private Sea sea;


    [SetUp]
    public void CreateSinker()
    {
        deploymentOfficer = new DeploymentOfficer();
        sinker = new Sinker(deploymentOfficer);
        sea = new Sea(new Fleet());
    }


    private void DeployAndOccupy(Ship ship, List<Sector> cells)
    {
        ship.Deploy(cells);

        foreach (Sector cell in cells)
            cell.Occupy(ship);
    }


    // =====================================================================
    // Позитивные
    // =====================================================================

    [Test]
    public void TestFloodShipMarksSurroundingEmptySectorsAsMiss()
    {
        Ship ship = new Ship(1);
        DeployAndOccupy(ship, new List<Sector> { sea.GetSector(5, 5) });

        sinker.FloodShip(ship, sea);

        Assert.AreEqual(StatusSector.Miss, sea.GetSector(4, 4).GetStatus());
        Assert.AreEqual(StatusSector.Miss, sea.GetSector(6, 6).GetStatus());
        Assert.AreEqual(StatusSector.Miss, sea.GetSector(5, 4).GetStatus());
        Assert.AreEqual(StatusSector.Miss, sea.GetSector(5, 6).GetStatus());
    }


    [Test]
    public void TestFloodShipDoesNotChangeShipOwnSectorStatus()
    {
        Ship ship = new Ship(1);
        DeployAndOccupy(ship, new List<Sector> { sea.GetSector(5, 5) });

        sinker.FloodShip(ship, sea);

        Assert.AreEqual(StatusSector.Ship, sea.GetSector(5, 5).GetStatus());
    }


    [Test]
    public void TestFloodShipDoesNotOverwriteHitSectors()
    {
        Ship ship = new Ship(1);
        DeployAndOccupy(ship, new List<Sector> { sea.GetSector(5, 5) });
        sea.GetSector(4, 4).SetStatus(StatusSector.Hit);

        sinker.FloodShip(ship, sea);

        Assert.AreEqual(StatusSector.Hit, sea.GetSector(4, 4).GetStatus());
    }


    [Test]
    public void TestFloodShipForMultiCellShipFloodsAroundEntireLine()
    {
        Ship ship = new Ship(3);
        DeployAndOccupy(ship, new List<Sector>
        {
            sea.GetSector(2, 2),
            sea.GetSector(3, 2),
            sea.GetSector(4, 2)
        });

        sinker.FloodShip(ship, sea);

        Assert.AreEqual(StatusSector.Miss, sea.GetSector(1, 2).GetStatus());
        Assert.AreEqual(StatusSector.Miss, sea.GetSector(5, 2).GetStatus());
        Assert.AreEqual(StatusSector.Miss, sea.GetSector(2, 1).GetStatus());
        Assert.AreEqual(StatusSector.Miss, sea.GetSector(4, 3).GetStatus());
    }


    // =====================================================================
    // Граничные
    // =====================================================================

    [Test]
    public void TestFloodShipAtCornerOnlyFloodsInBoundsNeighbors()
    {
        Ship ship = new Ship(1);
        DeployAndOccupy(ship, new List<Sector> { sea.GetSector(0, 0) });

        Assert.DoesNotThrow(() => sinker.FloodShip(ship, sea));

        Assert.AreEqual(StatusSector.Miss, sea.GetSector(1, 0).GetStatus());
        Assert.AreEqual(StatusSector.Miss, sea.GetSector(0, 1).GetStatus());
        Assert.AreEqual(StatusSector.Miss, sea.GetSector(1, 1).GetStatus());
    }


    [Test]
    public void TestFloodShipWithEmptyPlaceDoesNothing()
    {
        Ship ship = new Ship(1);

        Assert.DoesNotThrow(() => sinker.FloodShip(ship, sea));
    }


    // =====================================================================
    // Негативные
    // =====================================================================

    [Test]
    public void TestFloodShipWithNullShipThrows()
    {
        Assert.Throws<NullReferenceException>(() => sinker.FloodShip(null, sea));
    }


    [Test]
    public void TestFloodShipWithNullSeaThrows()
    {
        Ship ship = new Ship(1);
        DeployAndOccupy(ship, new List<Sector> { sea.GetSector(5, 5) });

        Assert.Throws<NullReferenceException>(() => sinker.FloodShip(ship, null));
    }
}
