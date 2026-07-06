using System;

public class BattleController
{
    private Fleet rightFleet;
    private Fleet leftFleet;
    private PlayerName winner;

    public BattleController()
    {
        rightFleet = new Fleet();
        leftFleet = new Fleet();
    }

    public PlayerName GetWinner()
    {
        return winner;
    }

    public Fleet GetRightFleet()
    {
        return rightFleet;
    }

    public Fleet GetLeftFleet()
    {
        return leftFleet;
    }

    public Fleet GetFleetNotDeployed()
    {
        if (!(rightFleet.IsDeployed()))
            return rightFleet;

        else if (!(leftFleet.IsDeployed()))
            return leftFleet;
        
        else
            throw new Exception("all ships is deployed");
            
    }

    public Ship GetShip(int durability)
    {
        Fleet fleet = GetFleetNotDeployed();

        return fleet.GetShip(durability);
    }

    
    public bool IsDeclareWinner()
    {
        if (!(rightFleet.HasSurvivors()))
            {
                winner = PlayerName.Player2;
                return true;
            }

        if (!(leftFleet.HasSurvivors()))
            {
                winner = PlayerName.Player1;
                return true;
            }
            
        return false;
    }
}
