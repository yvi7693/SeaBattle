using System;

public class Ship
{
    private int durability;

    public Ship(int durability)
    {
        if (durability <= 0)
            throw new Exception("incorrect value");
            
        this.durability = durability;
    }

    public void Damage()
    {
        if (durability == 0)
            throw new Exception("The ship has already been destroyed");

        durability -= 1;
    }

    public Boolean IsSunken()
    {
        return durability == 0;
    }
}