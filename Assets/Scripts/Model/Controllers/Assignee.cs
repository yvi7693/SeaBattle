using System;
using System.Collections.Generic;

public class Assignee
{
    private AttackResolver attackResolver;
    private Sinker sinker;

    public Assignee(AttackResolver attackResolver, Sinker sinker)
    {
        this.attackResolver  = attackResolver;
        this.sinker = sinker;
    }

    public StatusSector AttackOrder(Sector target)
    {
        StatusSector newStatus = attackResolver.Resolve(target);
        target.Attack(newStatus);

        if (!(target.HaveShip()))
            return newStatus;

        Ship ship = target.GetShip();
        ship.Damage();

        if (!(ship.IsSunken()))
            return newStatus;

        sinker.FloodShip();

        return newStatus;
    }

    public void DeployOrder(List<Sector> station, Ship ship)
    {
        for (int i = 0; i < station.Count; i++)
        {
            station[i].Occupy(ship);
        }
    }

}