using System;
using System.Collections.Generic;

public class Fleet
{
    private Ship[] ships;

    public Fleet()
    {
        ships = new Ship[10];

        for (int i = 0; i < 4; i++)
            ships[i] = new Ship(1);

        for (int i = 4; i < 7; i++)
            ships[i] = new Ship(2);

        for (int i = 7; i < 9; i++)
            ships[i] = new Ship(3);

        ships[9] = new Ship(4);
    }

    public int CalculateCount(int size)
    {
        int count = 0;

        for (int i = 0; i < ships.Length; i++)
        {
            if (ships[i].GetSize() == size && !ships[i].IsSunken())
                count += 1;
        }

        return count;
    }


    public Ship GetShip(int durability)
    {
        for (int i = 0; i < 10; i++)
        {
            if (ships[i].GetDurability() == durability && !(ships[i].IsDeploy())) 
                return ships[i];
        }

        throw new ArgumentException("dont have ship with the durability");
    }

    public Ship[] GetShips()
    {
        return ships;
    }

    public bool IsDeployed()
    {
        for (int i = 0; i < ships.Length; i++)
        {
            if (!(ships[i].IsDeploy()))
                return false;
        }

        return true;
    }

    public bool HasSurvivors()
    {
        for (int i = 0; i < ships.Length; i++)
        {
            if (!(ships[i].IsSunken()))
                return true;
        }

        return false;
    }

    public void Recall()
    {
        for (int i = 0; i < ships.Length; i++)
        {
            ships[i].Recall();
        }
    }
}