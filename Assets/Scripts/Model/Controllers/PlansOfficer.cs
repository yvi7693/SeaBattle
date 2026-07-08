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
       Sea targetSea = turnRecon.GetTargetSea();

       Sector targetSector = targetSea.GetSector(targetX, targetY);

       if (targetSea.IsAttackedSector(targetX, targetY))
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
        Sea deploySea = turnRecon.GetDeploySea();
        Ship[] ships = fleet.GetShips();

        for (int restart = 0; restart < 1000; restart++)
        {
            Random random = new Random();

            bool isFleetDeployed = true;

            for (int i = 0; i < ships.Length; i++)
            {
                bool isWork = true;
                
                int attempts = 0;

                while (isWork && attempts < 1000)
                {
                    attempts ++;

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

                    if (TryDeployShip(deploySea, coords))
                        isWork = false;
                }

                if (isWork)
                {
                    isFleetDeployed = false;
                    deploySea.Clear();
                    deploySea.RecallFleet();
                    break;
                }

            }

            if (isFleetDeployed)
                return;

        }

        throw new Exception("restarts limits have expired");
        
    }  
}
