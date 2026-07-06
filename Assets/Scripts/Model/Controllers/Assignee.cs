using System;
using System.Collections.Generic;

public class Assignee
{
    private AttackResolver attackResolver;
    private Sinker sinker;
    private TurnRecon turnRecon;

    public Assignee(AttackResolver attackResolver, Sinker sinker, TurnRecon turnRecon)
    {
        this.attackResolver  = attackResolver;
        this.sinker = sinker;
        this.turnRecon = turnRecon;
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
            
        sinker.FloodShip(ship, turnRecon.GetTargetSea());
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
