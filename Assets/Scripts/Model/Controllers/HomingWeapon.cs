using System;
using System.Collections.Generic;

public class HomingWeapon
{
    private int min;
    private int max;
    private Sector lastAttack;
    private Sector lastHit;
    private DeploymentOfficer deploymentOfficer;

    public HomingWeapon(DeploymentOfficer deploymentOfficer, int min = 0, int max = 10)
    {
        this.min = min;
        this.max = max;
        this.deploymentOfficer = deploymentOfficer;
        lastAttack = null;
        lastHit = null;
    }


    public (int, int) Guidance(Sea sea)
    {
        if (lastAttack is null)
            return RandomAttack(sea);

        else if (lastAttack.GetStatus() == StatusSector.Hit)
        {
            lastHit = lastAttack;
            return NearbyAttack(sea);
        }

        else if (lastHit != null && lastAttack.GetStatus() == StatusSector.Miss)
        {
            lastAttack = lastHit;
            return NearbyAttack(sea);
        }
            
        else
            return RandomAttack(sea);
            
    }

    private (int, int) NearbyAttack(Sea sea)
    {
        Sector sector = NearbySearch(sea, lastAttack);

            if (sector is null)
            {
                lastAttack = null;
                lastHit = null;
                return RandomAttack(sea);
            }

            else
            {
                lastAttack = sector;
                return sector.GetCoord();
            }
    }


    private Sector NearbySearch(Sea sea, Sector sector)
    {
        List<Sector> nearbySectors = GetPredictTarget(sea, sector);

        if (nearbySectors.Count == 0)
            return null;

        Random random = new Random();

        int indexRandom = random.Next(0, nearbySectors.Count);

        return nearbySectors[indexRandom];
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


    private List<Sector> GetPredictTarget(Sea sea, Sector sector)
    {
        List<Sector> targets = new List<Sector>();

        (int x, int y) = sector.GetCoord();

        for (int i = -1; i <= 1; i++)
        {
            if (i == 0)
                continue;

            if (sea.ValidateBorder(x+i, y) && !sea.GetSector(x+i, y).IsAttacked())
                targets.Add(sea.GetSector(x+i, y));

            if (sea.ValidateBorder(x, y+i) && !sea.GetSector(x, y+i).IsAttacked())
                targets.Add(sea.GetSector(x, y+i));
        }

        return targets;
    }
}