using System;
using System.Collections.Generic;

public class HomingWeapon
{
    private int min;
    private int max;
    private Sector lastAttack;
    private DeploymentOfficer deploymentOfficer;

    public HomingWeapon(DeploymentOfficer deploymentOfficer, int min = 0, int max = 10)
    {
        this.min = min;
        this.max = max;
        this.deploymentOfficer = deploymentOfficer;
        lastAttack = null;
    }


    public (int, int) Guidance(Sea sea)
    {
        if (lastAttack is null)
            return RandomAttack(sea);

        else if (lastAttack.GetStatus() == StatusSector.Hit)
        {
            Sector sector = NearbyAttack(sea, lastAttack);

            if (sector is null)
            {
                lastAttack = null;
                return RandomAttack(sea);
            }
                
            else
                lastAttack = sector;
                return sector.GetCoord();
        }

        else
            return RandomAttack(sea);
            
    }

    private Sector NearbyAttack(Sea sea, Sector sector)
    {
        List<Sector> nearbySectors = deploymentOfficer.GetNearbySector(sea, sector);

        for (int i = 0; i < nearbySectors.Count; i++)
        {
            if (!nearbySectors[i].IsAttacked())
                return nearbySectors[i];
        }

        return null;
    }

    private (int, int) RandomAttack(Sea sea)
    {
        Random random = new Random();
        while(true)
            {
                int x = random.Next(min, max);
                int y = random.Next(min, max);

                Sector sector = sea.GetSector(x, y);

                if (!sector.IsAttacked())
                {
                    lastAttack = sector;
                        return (x, y);
                }
                    
            }
    }
}