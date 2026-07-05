using System;
using System.Collections.Generic;

public class PlansOfficer
{
    private TurnRecon turnRecon;
    private Assignee assignee;
    private DeploymentOfficer deploymentOfficer;
    private BattleController battleController;

    public PlansOfficer(TurnRecon turnRecon, 
                        Assignee assignee, 
                        DeploymentOfficer deploymentOfficer,
                        BattleController battleController)
    {
        this.turnRecon = turnRecon;
        this.assignee = assignee;
        this.deploymentOfficer = deploymentOfficer;
        this.battleController = battleController;
    }

    public (bool, Sector) PlanOrder(int targetX, int targetY)
    {
       Sea sea = turnRecon.GetQueue();

       Sector targetSector = sea.GetSector(targetX, targetY);

       if (sea.IsAttackedSector(targetX, targetY))
            return (false, targetSector);

        return (true, targetSector);
    }

    public bool TryDeployShip(Sea sea, List <(int x, int y)> positions)
    {
        Fleet fleet = sea.GetFleet();

        Ship ship = fleet.GetShip(positions.Count);

        List<Sector> sectors = sea.CollectSectors(positions);

        if (deploymentOfficer.ValidateDeploy(sea, positions))
        {
            assignee.DeployOrder(sectors, ship);

            ship.Deploy(sectors);

            return true;
        }  

        return false;
    }


    public void DeployFleet()
    {
        Fleet fleet = battleController.GetFleetNotDeployed();
        Sea sea = turnRecon.GetSeaDeploy();
        Ship[] ships = fleet.GetShips();

        Random random = new Random();

        for (int i = 0; i < ships.Length; i++)
        {
            bool isWork = true;

            while (isWork)
            {
                List<(int, int)> coords = new List<(int, int)>();

                int durability = ships[i].GetDurability();

                int x = random.Next(0, 10);
                int y = random.Next(0, 10);

                for (int j = 0; j < durability; j++)
                {
                    if (x + durability < 10)   
                        coords.Add((x+j, y));
                    else if (y + durability < 10)
                        coords.Add((x, y+j));

                    else if (x - durability > 0)
                        coords.Add((x-j, y));
                    
                    else if (y - durability > 0)
                        coords.Add((x, y-j));
                }

                if (TryDeployShip(sea, coords))
                    isWork = false;
            }
        }
    }  
}
