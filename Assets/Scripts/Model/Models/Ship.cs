using System;
using System.Collections.Generic;

public class Ship
{
    private int durability;
    private int size;
    private bool deployed;
    private DeploymentOfficer deploymentOfficer;
    private List<Sector> place;

    public Ship(int size, bool deployed = false)
    {
        if (size <= 0)
            throw new Exception("incorrect value");
            
        this.deployed = deployed;
        this.size = size;
        this.durability = size;
        place = new List<Sector>();

        deploymentOfficer = new DeploymentOfficer();
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
        if (place.Count != size)
            throw new Exception("size mismatch");

        if (IsSunken())
            throw new Exception("ships is sunken");

        if (!deploymentOfficer.ValidatePlace(place))
            throw new Exception("incorrect sectors");

        this.place = place;
        deployed = true;
    }


    public void Damage()
    {
        if (IsSunken())
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
