using System;

public class Assignee
{
    private AttackResolver attackResolver;
    private Sinker sinker;

    public void AttackSector(Sector target)
    {
        StatusSector newStatus = attackResolver.Resolve(target);
        target.Attack(newStatus);

        if (!(target.HaveShip()))
            return;

        Ship ship = target.GetShip();
        ship.Damage();

        if (!(ship.IsSunken()))
            return;

        sinker.FloodShip();
    }

    public void DeployOrder()
    {
        
    }

}