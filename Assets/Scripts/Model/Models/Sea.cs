using System;

public class Sea
{
    private int size;
    private Sector[,] sectors;

    public Sea(int size)
    {
        if (size < 10)
            throw new ArgumentException("incorrect value");

        this.size = size;
        sectors = new Sector[size, size];
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

    public Sector GetSector(int x, int y)
    {
        if (x > size  && y > size)
            return sectors[x,y];
        
        else
            throw new ArgumentException("incorrect value");
    }
}