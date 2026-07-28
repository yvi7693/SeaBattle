using NUnit.Framework;
using System;
using System.Collections.Generic;

public class BattleControllerTests
{
    private BattleController battleController;


    [SetUp]
    public void CreateBattleController()
    {
        battleController = new BattleController();
    }


    private void DeployShip(Ship ship)
    {
        List<Sector> cells = new List<Sector>();

        for (int i = 0; i < ship.GetSize(); i++)
            cells.Add(new Sector(i, 0));

        ship.Deploy(cells);
    }


    private void DeployAllShips(Fleet fleet)
    {
        foreach (Ship ship in fleet.GetShips())
            DeployShip(ship);
    }


    private void SinkAllShips(Fleet fleet)
    {
        foreach (Ship ship in fleet.GetShips())
        {
            DeployShip(ship);

            for (int i = 0; i < ship.GetSize(); i++)
                ship.Damage();
        }
    }


    // =====================================================================
    // Позитивные
    // =====================================================================

    [Test]
    public void TestConstructorCreatesTwoDistinctFleets()
    {
        Assert.AreNotSame(battleController.GetRightFleet(), battleController.GetLeftFleet());
        Assert.AreEqual(10, battleController.GetRightFleet().GetShips().Length);
        Assert.AreEqual(10, battleController.GetLeftFleet().GetShips().Length);
    }


    [Test]
    public void TestGetRightFleetReturnsSameInstanceOnRepeatedCalls()
    {
        Assert.AreSame(battleController.GetRightFleet(), battleController.GetRightFleet());
    }


    [Test]
    public void TestGetLeftFleetReturnsSameInstanceOnRepeatedCalls()
    {
        Assert.AreSame(battleController.GetLeftFleet(), battleController.GetLeftFleet());
    }


    [Test]
    public void TestGetFleetNotDeployedReturnsRightFleetWhenNeitherDeployed()
    {
        Assert.AreSame(battleController.GetRightFleet(), battleController.GetFleetNotDeployed());
    }


    [Test]
    public void TestGetShipReturnsShipFromRightFleetByDefault()
    {
        Ship ship = battleController.GetShip(1);

        Assert.Contains(ship, battleController.GetRightFleet().GetShips());
    }


    [Test]
    public void TestIsDeclareWinnerFalseWhenBothFleetsHaveSurvivors()
    {
        Assert.IsFalse(battleController.IsDeclareWinner());
    }


    [Test]
    public void TestIsDeclareWinnerTrueAndPlayer2WinsWhenRightFleetFullySunk()
    {
        SinkAllShips(battleController.GetRightFleet());

        Assert.IsTrue(battleController.IsDeclareWinner());
        Assert.AreEqual(PlayerName.Player2, battleController.GetWinner());
    }


    [Test]
    public void TestIsDeclareWinnerTrueAndPlayer1WinsWhenLeftFleetFullySunk()
    {
        SinkAllShips(battleController.GetLeftFleet());

        Assert.IsTrue(battleController.IsDeclareWinner());
        Assert.AreEqual(PlayerName.Player1, battleController.GetWinner());
    }


    // =====================================================================
    // Граничные
    // =====================================================================

    [Test]
    public void TestGetFleetNotDeployedReturnsLeftFleetWhenRightAlreadyDeployed()
    {
        DeployAllShips(battleController.GetRightFleet());

        Assert.AreSame(battleController.GetLeftFleet(), battleController.GetFleetNotDeployed());
    }


    [Test]
    public void TestGetShipMovesToLeftFleetAfterRightFullyDeployed()
    {
        DeployAllShips(battleController.GetRightFleet());

        Ship ship = battleController.GetShip(1);

        Assert.Contains(ship, battleController.GetLeftFleet().GetShips());
    }


    [Test]
    public void TestGetWinnerDefaultsToPlayer1BeforeAnyWinnerDeclared()
    {
        Assert.AreEqual(PlayerName.Player1, battleController.GetWinner());
    }


    [Test]
    public void TestIsDeclareWinnerDoesNotChangeWinnerWhenNoOneDefeated()
    {
        battleController.IsDeclareWinner();

        Assert.AreEqual(PlayerName.Player1, battleController.GetWinner());
    }


    [Test]
    public void TestIsDeclareWinnerChecksRightFleetFirstWhenBothSunk()
    {
        SinkAllShips(battleController.GetRightFleet());
        SinkAllShips(battleController.GetLeftFleet());

        Assert.IsTrue(battleController.IsDeclareWinner());
        Assert.AreEqual(PlayerName.Player2, battleController.GetWinner());
    }


    // =====================================================================
    // Негативные
    // =====================================================================

    [Test]
    public void TestGetFleetNotDeployedThrowsWhenBothDeployed()
    {
        DeployAllShips(battleController.GetRightFleet());
        DeployAllShips(battleController.GetLeftFleet());

        Assert.Throws<Exception>(() => battleController.GetFleetNotDeployed());
    }


    [Test]
    public void TestGetShipThrowsWhenBothFleetsDeployed()
    {
        DeployAllShips(battleController.GetRightFleet());
        DeployAllShips(battleController.GetLeftFleet());

        Assert.Throws<Exception>(() => battleController.GetShip(1));
    }


    [Test]
    public void TestGetShipThrowsForNonExistentDurability()
    {
        Assert.Throws<ArgumentException>(() => battleController.GetShip(99));
    }
}
