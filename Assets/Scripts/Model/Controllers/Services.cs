using System;
using System.Collections.Generic;

public class DeploymentOfficer
{
    public bool ValidateDeploy()
    {
        return true;
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

    public void FloodShip(Ship ship, Sea sea)
    {
        List<Sector> place = ship.GetPlace();

        for(int i = 0; i < place.Count; i++)
        {
            (int xt, int yt) = place[i].GetCoord();

            for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                if (!(sea.ValidateBorder(xt+x, yt+y)))
                    continue;

                Sector checkSector = sea.GetSector(xt+x, yt+y);

                if (checkSector.GetStatus() == StatusSector.Empty)
                    checkSector.Attack(StatusSector.Miss);

            }
        }
        }

        
    }
    
}