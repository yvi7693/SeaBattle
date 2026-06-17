using System.Collections.Generic;

public class PlansOfficer
{
    private TurnRecon turnRecon;
    private Assignee assignee;
    private DeploymentOfficer deploymentOfficer;

    public PlansOfficer(TurnRecon turnRecon, Assignee assignee, DeploymentOfficer deploymentOfficer)
    {
        this.turnRecon = turnRecon;
        this.assignee = assignee;
        this.deploymentOfficer = deploymentOfficer;

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
        Sea sea = turnRecon.ReconOrder();

        List<Sector> sectors = sea.CollectSectors(positions);

        if (deploymentOfficer.ValidateDeploy(sectors, sea))
        {
             assignee.DeployOrder(sectors, ship);

            return true;
        }  

        return false;
    }

    public void DeployFleet(Fleet fleet)
    {
        
    }  
    }
