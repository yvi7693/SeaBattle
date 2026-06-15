using System.Collections.Generic;

class PlansOfficer
{
    private TurnRecon turnRecon;

    public PlansOfficer(TurnRecon turnRecon)
    {
        this.turnRecon = turnRecon;
    }

    public (bool, Sector) PlanOrder(int targetX, int targetY)
    {
        
    }

    public bool TryDeployShip(Ship ship, List <(int x, int y)> positions)
    {
        
    }
}