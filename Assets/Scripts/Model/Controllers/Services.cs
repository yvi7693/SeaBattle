using System;

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