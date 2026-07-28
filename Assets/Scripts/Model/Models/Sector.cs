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

    public (int, int) GetCoord()
    {
        return (x,y);
    }

    public Ship GetShip() {return ship;}

    public StatusSector GetStatus() {return status;}

    public void SetStatus(StatusSector newStatus) { status = newStatus;}

    public bool IsAttacked()
    {
        return status == StatusSector.Hit || status == StatusSector.Miss;
    }

    public bool IsEmpty()
    {
        return status == StatusSector.Empty;
    }

    public bool IsHit()
    {
        return status == StatusSector.Hit;
    }

    public bool HaveShip()
    {
        return !(ship is null);
    }

    public void RecallShip()
    {
        ship = null;
    }

    public void Occupy(Ship ship)
    {
        if (!(this.ship is null))
            throw new Exception("The ship is already there");

        this.ship = ship;
        status = StatusSector.Ship;

    }
}

public enum StatusSector
{
    Empty,
    Ship,
    Miss,
    Hit
}