using System;

public class BattleController
{
    private Fleet fleet1;
    private Fleet fleet2;
    private Fleet winner;

    public BattleController()
    {
        this.fleet1 = new Fleet();
        this.fleet2 = new Fleet();
    }

    public Fleet GetFleet1()
    {
        return fleet1;
    }

    public Fleet GetFleet2()
    {
        return fleet2;
    }

    public Fleet GetFleetNotDeployed()
    {
        if (!(fleet1.IsDeployed()))
            return fleet1;

        else if (!(fleet2.IsDeployed()))
            return fleet2;
        
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
        if (!(fleet1.HasSurvivors()))
            {
                winner = fleet2;
                return true;
            }

        if (!(fleet2.HasSurvivors()))
        {
            winner = fleet1;
            return true;
        }
            
        return false;
    }
}