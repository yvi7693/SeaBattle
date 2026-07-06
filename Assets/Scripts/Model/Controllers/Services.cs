using System;
using System.Collections.Generic;

public class DeploymentOfficer
{
    public bool ValidateDeploy(Sea sea, List<(int, int)> positions)
    {
        for (int i = 0; i < positions.Count; i++)
        {
            (int x, int y) = positions[i];

            Sector sector = sea.GetSector(x, y);

            if (!(sector.IsEmpty()))
                return false;

            List<Sector> nearbySectors = GetNearbySector(sea, sector);

            for (int j = 0; j < nearbySectors.Count; j++)
            {
                if (!(nearbySectors[j].IsEmpty()))
                    return false;
            }
        }

        return true;
    }

    public List<Sector> GetNearbySector(Sea sea, Sector sector)
    {

        List<Sector> sectors = new List<Sector>();

        (int xt, int yt) = sector.GetCoord();

        for(int x = -1; x <= 1; x++)
            {
                for(int y = -1; y <= 1; y++)
                {
                    if (!(sea.ValidateBorder(xt+x, yt+y)))
                        continue;

                    Sector checkSector = sea.GetSector(xt+x, yt+y);
                    sectors.Add(checkSector);
                }
            }
        return sectors;
    }
}

public class AttackResolver
{
    public StatusSector Resolve(Sector target)
    {
        StatusSector currentStatus = target.GetStatus();

        if (currentStatus == StatusSector.Hit || currentStatus == StatusSector.Miss)
            throw new Exception("It's already been attacked");

        if (currentStatus == StatusSector.Ship)
            return StatusSector.Hit;
        
        return StatusSector.Miss;
    }
}

public class Sinker
{
    private DeploymentOfficer deploymentOfficer;


    public Sinker(DeploymentOfficer deploymentOfficer)
    {
        this.deploymentOfficer = deploymentOfficer;
    }


    public void FloodShip(Ship ship, Sea sea)
    {
        List<Sector> place = ship.GetPlace();

        for(int i = 0; i < place.Count; i++)
        {

            List<Sector> nearbySectors = deploymentOfficer.GetNearbySector(sea, place[i]);

            for (int j = 0; j < nearbySectors.Count; j++)
            {
                if (nearbySectors[j].GetStatus() == StatusSector.Empty)
                    nearbySectors[j].Attack(StatusSector.Miss);
            }
        }
    }
}


    
