using System;

public class HomingWeapon
{
    private int min;
    private int max;

    public HomingWeapon(int min = 0, int max = 9)
    {
        this.min = min;
        this.max = max;
    }


    public (int, int) Guidance(Sea sea)
    {
        Random random = new Random();

        while(true){
            int x = random.Next(min, max);
            int y = random.Next(min, max);

            Sector sector = sea.GetSector(x, y);

            if (!sector.IsHit())
                return (x, y);
        }
    }
}