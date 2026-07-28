using NUnit.Framework;
using System;
using System.Collections.Generic;

public class TurnReconTests
{
    private TurnRecon turnRecon;


    [SetUp]
    public void CreateTurnRecon()
    {
        turnRecon = new TurnRecon(new Fleet(), new Fleet());
    }


    private void DeployAllShips(Sea sea)
    {
        Ship[] ships = sea.GetFleet().GetShips();

        for (int row = 0; row < ships.Length; row++)
        {
            int shipSize = ships[row].GetDurability();
            List<Sector> cells = new List<Sector>();

            for (int col = 0; col < shipSize; col++)
                cells.Add(sea.GetSector(col, row));

            ships[row].Deploy(cells);
        }
    }


    // =====================================================================
    // Позитивные
    // =====================================================================

    [Test]
    public void TestConstructorTargetsLeftSeaByDefault()
    {
        Assert.AreSame(turnRecon.GetLeftSea(), turnRecon.GetTargetSea());
    }


    [Test]
    public void TestGetAttackerSeaReturnsRightSeaByDefault()
    {
        Assert.AreSame(turnRecon.GetRightSea(), turnRecon.GetAttackerSea());
    }


    [Test]
    public void TestGetRightSeaAndGetLeftSeaAreDistinct()
    {
        Assert.AreNotSame(turnRecon.GetRightSea(), turnRecon.GetLeftSea());
    }


    [Test]
    public void TestSwitchTargetSeaTogglesFromLeftToRight()
    {
        turnRecon.SwitchTargetSea();

        Assert.AreSame(turnRecon.GetRightSea(), turnRecon.GetTargetSea());
    }


    [Test]
    public void TestSwitchTargetSeaTogglesBackToLeft()
    {
        turnRecon.SwitchTargetSea();
        turnRecon.SwitchTargetSea();

        Assert.AreSame(turnRecon.GetLeftSea(), turnRecon.GetTargetSea());
    }


    [Test]
    public void TestGetDeploySeaReturnsRightSeaWhenNeitherDeployed()
    {
        Assert.AreSame(turnRecon.GetRightSea(), turnRecon.GetDeploySea());
    }


    // =====================================================================
    // Граничные
    // =====================================================================

    [Test]
    public void TestGetDeploySeaReturnsLeftSeaWhenRightFullyDeployed()
    {
        DeployAllShips(turnRecon.GetRightSea());

        Assert.AreSame(turnRecon.GetLeftSea(), turnRecon.GetDeploySea());
    }


    [Test]
    public void TestGetAttackerSeaFollowsTargetAfterSwitch()
    {
        turnRecon.SwitchTargetSea();

        Assert.AreSame(turnRecon.GetLeftSea(), turnRecon.GetAttackerSea());
    }


    [Test]
    public void TestSwitchTargetSeaMultipleTimesAlternatesCorrectly()
    {
        turnRecon.SwitchTargetSea();
        turnRecon.SwitchTargetSea();
        turnRecon.SwitchTargetSea();

        Assert.AreSame(turnRecon.GetRightSea(), turnRecon.GetTargetSea());
    }


    [Test]
    public void TestConstructorCreatesSeasWithDefaultBoardSize()
    {
        Assert.DoesNotThrow(() => turnRecon.GetRightSea().GetSector(9, 9));
        Assert.DoesNotThrow(() => turnRecon.GetLeftSea().GetSector(9, 9));
    }


    // =====================================================================
    // Негативные
    // =====================================================================

    [Test]
    public void TestGetDeploySeaThrowsWhenBothFullyDeployed()
    {
        DeployAllShips(turnRecon.GetRightSea());
        DeployAllShips(turnRecon.GetLeftSea());

        Assert.Throws<Exception>(() => turnRecon.GetDeploySea());
    }


    [Test]
    public void TestConstructorThrowsWhenRightFleetIsNull()
    {
        Assert.Throws<ArgumentException>(() => new TurnRecon(null, new Fleet()));
    }


    [Test]
    public void TestConstructorThrowsWhenLeftFleetIsNull()
    {
        Assert.Throws<ArgumentException>(() => new TurnRecon(new Fleet(), null));
    }
}
