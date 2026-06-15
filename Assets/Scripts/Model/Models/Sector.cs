using System;

public class Sector
{
    private int x;
    private int y;
    private Ship ship;
    private StatusSector status;

    public Sector(int x, int y, Ship ship = null)
    {
        this.x = x;
        this.y = y;
        this.ship = ship;

        if (this.ship is null)
            this.status = StatusSector.Empty;
        
        else
            this.status = StatusSector.Ship;

    }

    public Ship GetShip() {return ship;}

    public StatusSector GetStatus() {return status;}

    public void Attack(StatusSector newStatus)
    {
        this.status = newStatus;
    }

    public bool IsAttacked()
    {
        return status == StatusSector.Hit || status == StatusSector.Miss;
    }

    public bool HaveShip()
    {
        return !(ship is null);
    }

    public void Occupy(Ship ship)
    {
        if (!(this.ship is null))
            throw new Exception("The ship is already there");

        this.ship = ship;

    }
}

public enum StatusSector
{
    Empty,
    Ship,
    Miss,
    Hit
}