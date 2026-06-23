using System;

public class Ship
{
    private int durability;
    private bool deployed;

    public Ship(int durability, bool deployed = false)
    {
        if (durability <= 0)
            throw new Exception("incorrect value");
            
        this.durability = durability;
        this.deployed = deployed;
    }

    public int GetDurability()
    {
        return durability;
    }

    public bool IsDeploy()
    {
        return deployed;
    }

    public void Deploy()
    {
        this.deployed = true;
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