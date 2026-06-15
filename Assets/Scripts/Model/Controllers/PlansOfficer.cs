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
       Sea sea = turnRecon.ReconOrder();

       Sector targetSector = sea.GetSector(targetX, targetY);

       if (sea.IsAttackedSector(targetX, targetY))
            return (false, targetSector);

        return (true, targetSector);
    }

    public bool TryDeployShip(Ship ship, List <(int x, int y)> positions)
    {
        
    }
}