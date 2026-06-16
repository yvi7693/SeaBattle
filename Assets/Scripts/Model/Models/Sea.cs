using System;
using System.Collections.Generic;

public class Sea
{
    private int size;
    private Sector[,] sectors;

    public Sea(int size = 10)
    {
        if (size < 10)
            throw new ArgumentException("incorrect value");

        this.size = size;
        sectors = new Sector[size, size];
    }

    public Sector GetSector(int x, int y)
    {
        if (x > size  && y > size)
            return sectors[x,y];
        
        else
            throw new ArgumentException("incorrect value");
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

    public void SetUp()
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                sectors[x,y] = new Sector(x,y);
            }
        }
    }

    public bool IsAttackedSector(int targetX, int targetY)
    {
        return sectors[targetX, targetY].IsAttacked();
    }

    
}