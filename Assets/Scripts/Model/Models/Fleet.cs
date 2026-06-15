using System;

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
}