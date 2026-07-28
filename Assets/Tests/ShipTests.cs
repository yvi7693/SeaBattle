using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

public class ShipTests
{
    private Ship ship;
    private Sea sea;


    [SetUp]
    public void CreateShip()
    {
        ship = new Ship(4);
        Fleet fleet = new Fleet();
        sea = new Sea(fleet);
    }


    [TearDown]
    public void DestroyShip()
    {
        ship = null;
        sea = null;
    }


    private List<Sector> GetSectors(int count)
    {
        List<Sector> sectors = new List<Sector>();

        for (int i = 0; i < count; i++)
            sectors.Add(sea.GetSector(i % 10, i / 10));

        return sectors;
    }


    // =====================================================================
    // Позитивные
    // =====================================================================

    [Test]
    public void TestDeployShipPositive()
    {
        List<Sector> sectors = GetSectors(4);

        ship.Deploy(sectors);

        Assert.IsTrue(ship.IsDeploy());
        Assert.AreEqual(sectors, ship.GetPlace());
    }


    [Test]
    public void TestDeploySetsDeployedFlagTrue()
    {
        Assert.IsFalse(ship.IsDeploy());

        ship.Deploy(GetSectors(4));

        Assert.IsTrue(ship.IsDeploy());
    }


    [TestCase(1)]
    [TestCase(2)]
    [TestCase(3)]
    [TestCase(4)]
    public void TestDeployAllValidShipSizes(int size)
    {
        Ship sizedShip = new Ship(size);

        sizedShip.Deploy(GetSectors(size));

        Assert.IsTrue(sizedShip.IsDeploy());
        Assert.AreEqual(size, sizedShip.GetPlace().Count);
    }


    [Test]
    public void TestDeployDoesNotCopyInputList()
    {
        List<Sector> sectors = GetSectors(4);
        ship.Deploy(sectors);

        sectors.RemoveAt(0);

        Assert.AreEqual(3, ship.GetPlace().Count);
    }


    [Test]
    public void TestDamagePositive()
    {
        Assert.AreEqual(4, ship.GetDurability());

        ship.Damage();

        Assert.AreEqual(3, ship.GetDurability());
    }


    [Test]
    public void TestDamageMultipleTimesDecrementsCorrectly()
    {
        ship.Damage();
        ship.Damage();
        ship.Damage();

        Assert.AreEqual(1, ship.GetDurability());
        Assert.IsFalse(ship.IsSunken());
    }


    [Test]
    public void TestDamageDoesNotChangeSizeOrDeployedFlag()
    {
        ship.Damage();

        Assert.AreEqual(4, ship.GetSize());
        Assert.IsFalse(ship.IsDeploy());
    }


    [Test]
    public void TestConstructorInitializesFieldsCorrectly()
    {
        Assert.AreEqual(4, ship.GetSize());
        Assert.AreEqual(4, ship.GetDurability());
        Assert.IsFalse(ship.IsDeploy());
        Assert.IsFalse(ship.IsSunken());
        Assert.IsNotNull(ship.GetPlace());
        Assert.AreEqual(0, ship.GetPlace().Count);
    }


    [Test]
    public void TestRecallAfterDeployResetsState()
    {
        ship.Deploy(GetSectors(4));

        ship.Recall();

        Assert.IsFalse(ship.IsDeploy());
        Assert.AreEqual(0, ship.GetPlace().Count);
    }


    // =====================================================================
    // Граничные
    // =====================================================================

    [Test]
    public void TestDeployMinimalShipSingleSector()
    {
        Ship smallShip = new Ship(1);

        smallShip.Deploy(GetSectors(1));

        Assert.IsTrue(smallShip.IsDeploy());
        Assert.AreEqual(1, smallShip.GetPlace().Count);
    }


    [Test]
    public void TestDeployExactSizeMatchForMaxShip()
    {
        List<Sector> sectors = GetSectors(ship.GetSize());

        ship.Deploy(sectors);

        Assert.AreEqual(ship.GetSize(), ship.GetPlace().Count);
    }


    [Test]
    public void TestDeployEmptyListThrows()
    {
        Assert.Throws<Exception>(() => ship.Deploy(new List<Sector>()));
        Assert.IsFalse(ship.IsDeploy());
    }


    [Test]
    public void TestDeployOneSectorLessThanSizeThrows()
    {
        List<Sector> sectors = GetSectors(ship.GetSize() - 1);

        Assert.Throws<Exception>(() => ship.Deploy(sectors));
        Assert.IsFalse(ship.IsDeploy());
    }


    [Test]
    public void TestDeployOneSectorMoreThanSizeThrows()
    {
        List<Sector> sectors = GetSectors(ship.GetSize() + 1);

        Assert.Throws<Exception>(() => ship.Deploy(sectors));
        Assert.IsFalse(ship.IsDeploy());
    }


    [Test]
    public void TestDamageShipDestroy()
    {
        Ship ship = new Ship(1);

        ship.Damage();

        Assert.AreEqual(0, ship.GetDurability());
        Assert.IsTrue(ship.IsSunken());
    }


    [Test]
    public void TestDamageStepBeforeSunkIsNotYetSunken()
    {
        ship.Damage();
        ship.Damage();
        ship.Damage();

        Assert.AreEqual(1, ship.GetDurability());
        Assert.IsFalse(ship.IsSunken());
    }


    [Test]
    public void TestDamageLastHitSinksShipWithoutThrowing()
    {
        Assert.DoesNotThrow(() =>
        {
            for (int i = 0; i < ship.GetSize(); i++)
                ship.Damage();
        });

        Assert.AreEqual(0, ship.GetDurability());
        Assert.IsTrue(ship.IsSunken());
    }


    [Test]
    public void TestConstructorMinimalSizeOne()
    {
        Ship smallShip = new Ship(1);

        Assert.AreEqual(1, smallShip.GetSize());
        Assert.AreEqual(1, smallShip.GetDurability());
        Assert.IsFalse(smallShip.IsSunken());
    }


    [Test]
    public void TestRecallOnNeverDeployedShipDoesNotThrow()
    {
        Assert.DoesNotThrow(() => ship.Recall());

        Assert.IsFalse(ship.IsDeploy());
        Assert.AreEqual(0, ship.GetPlace().Count);
    }


    [Test]
    public void TestRecallDoesNotResetDurability()
    {
        ship.Damage();
        ship.Deploy(GetSectors(4));

        ship.Recall();

        Assert.AreEqual(3, ship.GetDurability());
    }


    [Test]
    public void TestConstructorDeployedTrueWithoutPlacementIsInconsistent()
    {
        Ship preDeployedShip = new Ship(4, true);

        Assert.IsTrue(preDeployedShip.IsDeploy());
        Assert.AreEqual(0, preDeployedShip.GetPlace().Count);
    }


    // =====================================================================
    // Негативные
    // =====================================================================

    [Test]
    public void TestDeployWithNullListThrows()
    {
        Assert.Throws<NullReferenceException>(() => ship.Deploy(null));
        Assert.IsFalse(ship.IsDeploy());
    }


    [Test]
    public void TestDeployWithDuplicateSectorsShouldBeRejected()
    {
        List<Sector> sectors = GetSectors(4);
        sectors[1] = sectors[0];

        Assert.Throws<Exception>(() => ship.Deploy(sectors));
    }


    [Test]
    public void TestDeployWithNullEntryInsideListShouldBeRejected()
    {
        List<Sector> sectors = GetSectors(4);
        sectors[2] = null;

        Assert.Throws<Exception>(() => ship.Deploy(sectors));
    }


    [Test]
    public void TestDeployWithScatteredSectorsNotInLineShouldBeRejected()
    {
        List<Sector> sectors = new List<Sector>
        {
            sea.GetSector(0, 0),
            sea.GetSector(5, 5),
            sea.GetSector(9, 9),
            sea.GetSector(2, 7)
        };

        Assert.Throws<Exception>(() => ship.Deploy(sectors));
    }


    [Test]
    public void TestDeployOnSunkShipShouldBeRejected()
    {
        for (int i = 0; i < ship.GetSize(); i++)
            ship.Damage();

        Assert.IsTrue(ship.IsSunken());
        Assert.Throws<Exception>(() => ship.Deploy(GetSectors(4)));
    }


    [Test]
    public void TestDamageWithSunkenShip()
    {
        Ship ship = new Ship(1);

        ship.Damage();

        Assert.Throws<Exception>(() => ship.Damage());
    }


    [Test]
    public void TestDamageThrowsOnEverySubsequentCallAfterSunk()
    {
        Ship ship = new Ship(1);
        ship.Damage();

        Assert.Throws<Exception>(() => ship.Damage());
        Assert.Throws<Exception>(() => ship.Damage());
        Assert.AreEqual(0, ship.GetDurability());
    }


    [Test]
    public void TestDamageAfterFullyDamagingLargerShipThrows()
    {
        for (int i = 0; i < ship.GetSize(); i++)
            ship.Damage();

        Assert.Throws<Exception>(() => ship.Damage());
    }


    [Test]
    public void TestConstructorZeroSizeThrows()
    {
        Assert.Throws<Exception>(() => new Ship(0));
    }


    [Test]
    public void TestConstructorNegativeSizeThrows()
    {
        Assert.Throws<Exception>(() => new Ship(-1));
    }
}
