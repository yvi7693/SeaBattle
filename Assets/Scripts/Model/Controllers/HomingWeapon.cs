using System;
using System.Collections.Generic;

public class HomingWeapon
{
    private int min;
    private int max;
    private Sector lastAttack;
    private Sector lastHit;
     private Sector memoryHit;
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
            memoryHit = lastHit;
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
        Sector nearbySector = NearbySearch(sea, lastAttack);

        if (nearbySector is null)
        {
            if (!lastHit.GetShip().IsSunken())
            {
                BackHit();

                Sector sector = NearbySearch(sea, lastHit);

                if (sector != null)
                {
                    lastAttack = sector;
                    return sector.GetCoord();
                }
            }

            DropMemory();
            return RandomAttack(sea);
        }

        else
        {
            lastAttack = nearbySector;
            return nearbySector.GetCoord();
        }
    }


    private Sector NearbySearch(Sea sea, Sector sector)
    {
        List<Sector> nearbySectors = GetPredictTarget(sea, sector);

        if (nearbySectors.Count == 0)
            return null;

        if (HaveTwoHits())
            return LinearAttack(sea);

        Random random = new Random();

        int indexRandom = random.Next(0, nearbySectors.Count);

        return nearbySectors[indexRandom];
    }


    private List<Sector> GetPredictTarget(Sea sea, Sector sector)
    {
        List<Sector> targets = new List<Sector>();

        (int x, int y) = sector.GetCoord();

        for (int i = -1; i <= 1; i++)
        {
            if (i == 0)
                continue;

            if (IsMayAttack(sea, x+i, y))
                targets.Add(sea.GetSector(x+i, y));

            if (IsMayAttack(sea, x, y+i))
                targets.Add(sea.GetSector(x, y+i));
        }

        return targets;
    }

    private Sector LinearAttack(Sea sea)
    {
        (int x1, int y1) = lastHit.GetCoord();
        (int x2, int y2) = memoryHit.GetCoord();

        if (x1 != x2 && y1 == y2)
        {
            if (x1 > x2)
                return TryGetTarget(sea, x1+1, y1);

            if (x1 < x2)
                return TryGetTarget(sea, x1-1, y1);
        }

        else if (y1 != y2 && x1 == x2)
        {
            if (y1 > y2)
                 return TryGetTarget(sea, x1, y1+1);

            else if (y1 < y2)
                 return TryGetTarget(sea, x1, y1-1);
        }

        throw new Exception("The coordinates are not on the same line");  
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


    private void DropMemory()
    {
        lastAttack = null;
        lastHit = null;
        memoryHit = null;
    }


    private bool HaveTwoHits()
    {
        return lastAttack.GetStatus() == StatusSector.Hit && lastHit != null && memoryHit != null;
    }


    private Sector TryGetTarget(Sea sea, int x, int y)
    {
        if (IsMayAttack(sea, x, y))
             return sea.GetSector(x, y);

        return null;
    }


    private bool IsMayAttack(Sea sea, int x, int y)
    {
        return sea.ValidateBorder(x, y) && !sea.GetSector(x, y).IsAttacked();
    }


    private void BackHit()
    {
        Sector buff = memoryHit;

        memoryHit = lastHit;
        lastHit = buff;
    }
}
