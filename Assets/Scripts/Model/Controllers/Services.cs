using System;
using System.Collections.Generic;

public class DeploymentOfficer
{
    public bool ValidateDeploy(List<Sector> station, Sea sea)
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

    public void FloodShip()
    {
        
    }
    
}