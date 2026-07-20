using System;
using System.Collections.Generic;

public class Ship
{
    private int durability;
    private int size;
    private bool deployed;
    private List<Sector> place;

    public Ship(int size, bool deployed = false)
    {
        if (size <= 0)
            throw new Exception("incorrect value");
            
        this.deployed = deployed;
        this.size = size;
        this.durability = size;
        place = new List<Sector>();
    }

    public List<Sector> GetPlace()
    {
        return place;
    }


    public int GetSize()
    {
        return size;
    }


    public int GetDurability()
    {
        return durability;
    }


    public bool IsDeploy()
    {
        return deployed;
    }


    public void Deploy(List<Sector> place)
    {
        this.place = place;
        deployed = true;
    }


    public void Damage()
    {
        if (durability == 0)
            throw new Exception("The ship has already been destroyed");

        durability -= 1;
    }


    public bool IsSunken()
    {
        return durability == 0;
    }


    public void Recall()
    {
        place = new List<Sector>();
        deployed = false;
    }
}
