using System;
using System.Collections.Generic;

public class Sea
{
    private int size;
    private Fleet fleet;
    private Sector[,] sectors;

    public Sea(Fleet fleet, int size = 10)
    {
        if (size < 10)
            throw new ArgumentException("incorrect value");

        this.size = size;
        this.fleet = fleet;
        sectors = new Sector[size, size];

        SetUp();
    }

    public Sector GetSector(int x, int y)
    {
        if (!(ValidateBorder(x,y)))
            throw new ArgumentException("incorrect value");
        
        return sectors[x,y];
            
    }

    public bool ValidateBorder(int x, int y)
    {
        return ((x > size || y > size) || (x < 0 || y < 0));
    }

    public bool IsShipsDeploy()
    {
        return fleet.IsDeployed();
    }


    public List<Sector> CollectSectors(List <(int x, int y)> positions)
    {
        List<Sector> sectors = new List<Sector>();

        for (int i = 0; i < positions.Count; i++)
        {
            (int targetX, int targetY) = positions[i];

            sectors.Add(this.GetSector(targetX, targetY));
        }

        return sectors;
    }

    public bool IsAttackedSector(int targetX, int targetY)
    {
        return sectors[targetX, targetY].IsAttacked();
    }

    private void SetUp()
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                sectors[x,y] = new Sector(x,y);
            }
        }
    }

    
}