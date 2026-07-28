using NUnit.Framework;
using System;
using System.Collections.Generic;

public class HomingWeaponTests
{
    private DeploymentOfficer deploymentOfficer;
    private HomingWeapon homingWeapon;
    private Sea sea;


    [SetUp]
    public void CreateHomingWeapon()
    {
        deploymentOfficer = new DeploymentOfficer();
        sea = new Sea(new Fleet());
        homingWeapon = new HomingWeapon(deploymentOfficer);
    }


    private void MarkAllAttackedExcept(Sea targetSea, params (int x, int y)[] openCells)
    {
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 10; y++)
            {
                bool isOpen = false;

                foreach ((int ox, int oy) in openCells)
                {
                    if (ox == x && oy == y)
                    {
                        isOpen = true;
                        break;
                    }
                }

                if (!isOpen)
                    targetSea.GetSector(x, y).SetStatus(StatusSector.Miss);
            }
        }
    }


    // =====================================================================
    // Позитивные
    // =====================================================================

    [Test]
    public void TestGuidanceFirstCallPicksTheOnlyUnattackedSector()
    {
        MarkAllAttackedExcept(sea, (5, 5));

        (int x, int y) = homingWeapon.Guidance(sea);

        Assert.AreEqual((5, 5), (x, y));
    }


    [Test]
    public void TestGuidanceRandomAttackStaysWithinConfiguredBounds()
    {
        (int x, int y) = homingWeapon.Guidance(sea);

        Assert.IsTrue(x >= 0 && x < 10);
        Assert.IsTrue(y >= 0 && y < 10);
    }


    [Test]
    public void TestGuidanceAfterHitPicksOnlyAvailableOrthogonalNeighbor()
    {
        MarkAllAttackedExcept(sea, (5, 5));
        homingWeapon.Guidance(sea);

        sea.GetSector(5, 5).SetStatus(StatusSector.Hit);
        sea.GetSector(6, 5).SetStatus(StatusSector.Empty);

        (int x, int y) = homingWeapon.Guidance(sea);

        Assert.AreEqual((6, 5), (x, y));
    }


    [Test]
    public void TestGuidanceLinearAttackExtrapolatesAfterTwoHitsInLine()
    {
        MarkAllAttackedExcept(sea, (5, 5));
        homingWeapon.Guidance(sea);

        sea.GetSector(5, 5).SetStatus(StatusSector.Hit);
        sea.GetSector(6, 5).SetStatus(StatusSector.Empty);
        homingWeapon.Guidance(sea);

        sea.GetSector(6, 5).SetStatus(StatusSector.Hit);
        // (7,5) is where LinearAttack should extrapolate to; (6,4) is a decoy
        // that a plain random pick could also return, to prove the linear branch fired
        sea.GetSector(7, 5).SetStatus(StatusSector.Empty);
        sea.GetSector(6, 4).SetStatus(StatusSector.Empty);

        (int x, int y) = homingWeapon.Guidance(sea);

        Assert.AreEqual((7, 5), (x, y));
    }


    // =====================================================================
    // Граничные
    // =====================================================================

    [Test]
    public void TestGuidanceMissAfterHitRetriesAroundLastHitInsteadOfLastMiss()
    {
        MarkAllAttackedExcept(sea, (5, 5));
        homingWeapon.Guidance(sea);

        sea.GetSector(5, 5).SetStatus(StatusSector.Hit);
        sea.GetSector(6, 5).SetStatus(StatusSector.Empty);
        homingWeapon.Guidance(sea);

        sea.GetSector(6, 5).SetStatus(StatusSector.Miss);
        sea.GetSector(5, 4).SetStatus(StatusSector.Empty);

        (int x, int y) = homingWeapon.Guidance(sea);

        Assert.AreEqual((5, 4), (x, y));
    }


    [Test]
    public void TestGuidanceBackHitRedirectsToOppositeDirectionWhenDeadEndReached()
    {
        Ship ship = new Ship(2);
        List<Sector> cells = new List<Sector> { sea.GetSector(4, 5), sea.GetSector(5, 5) };
        ship.Deploy(cells);

        foreach (Sector cell in cells)
            cell.Occupy(ship);

        MarkAllAttackedExcept(sea, (5, 5));
        homingWeapon.Guidance(sea);

        sea.GetSector(5, 5).SetStatus(StatusSector.Hit);
        sea.GetSector(4, 5).SetStatus(StatusSector.Empty);
        homingWeapon.Guidance(sea);

        // (4,5) hit confirmed, but its own neighbourhood (besides (5,5)) stays a dead end -
        // this forces BackHit to pivot and try the opposite direction from (5,5) via (6,5)
        sea.GetSector(4, 5).SetStatus(StatusSector.Hit);
        sea.GetSector(6, 5).SetStatus(StatusSector.Empty);

        (int x, int y) = homingWeapon.Guidance(sea);

        Assert.AreEqual((6, 5), (x, y));
    }


    [Test]
    public void TestGuidanceAtCornerOnlyConsidersInBoundsNeighbors()
    {
        MarkAllAttackedExcept(sea, (0, 0));
        homingWeapon.Guidance(sea);

        sea.GetSector(0, 0).SetStatus(StatusSector.Hit);
        sea.GetSector(1, 0).SetStatus(StatusSector.Empty);

        (int x, int y) = homingWeapon.Guidance(sea);

        Assert.AreEqual((1, 0), (x, y));
    }


    // =====================================================================
    // Негативные
    // =====================================================================

    [Test]
    public void TestGuidanceThrowsWhenHitSectorHasNoAssociatedShip()
    {
        MarkAllAttackedExcept(sea, (5, 5));
        homingWeapon.Guidance(sea);

        // Status is forced to Hit without ever placing a real Ship on the sector -
        // Guidance eventually calls lastHit.GetShip().IsSunken(), which NREs on a null ship
        sea.GetSector(5, 5).SetStatus(StatusSector.Hit);

        Assert.Throws<NullReferenceException>(() => homingWeapon.Guidance(sea));
    }
}
