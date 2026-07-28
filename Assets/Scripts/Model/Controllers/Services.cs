using System;
using System.Collections.Generic;

public class DeploymentOfficer
{
    public bool ValidateDeploy(Sea sea, List<(int, int)> positions)
    {
        for (int i = 0; i < positions.Count; i++)
        {
            (int x, int y) = positions[i];

            Sector sector = sea.GetSector(x, y);

            if (!(sector.IsEmpty()))
                return false;

            List<Sector> nearbySectors = GetNearbySector(sea, sector);

            for (int j = 0; j < nearbySectors.Count; j++)
            {
                if (!(nearbySectors[j].IsEmpty()))
                    return false;
            }
        }

        return true;
    }


    public bool ValidatePlace(List<Sector> sectors)
    {

        if (!ValidateEqualSectors(sectors))
            return false;

        if (!ValidateNearbySectors(sectors))
            return false;

        return true; 
    }


    public bool ValidateEqualSectors(List<Sector> sectors)
    {
         for (int i = 0; i < sectors.Count; i++)
        {
            for (int j = i; j < sectors.Count; j++)
            {
                if ((sectors[i] == sectors[j] && i != j) || sectors[i] == null)
                    return false;
            }
        }

        return true;

    }


    public bool ValidateNearbySectors(List<Sector> sectors)
    {
        List<(int, int)> coords = new List<(int, int)>();

        for (int i = 0; i < sectors.Count; i++)
        {
            (int x, int y) = sectors[i].GetCoord();

            coords.Add((x, y));
        }

        List<(int, int)> normCoords = NormalizeCoords(coords);

        List<int> coordX = new List<int>();
        List<int> coordY = new List<int>();

        for(int i = 0; i < normCoords.Count; i++)
        {
            (int x, int y) = normCoords[i];

            coordX.Add(x);
            coordY.Add(y);
        }

        bool sameX = coordX[0] == coordX[coordX.Count - 1];
        bool sameY = coordY[0] == coordY[coordY.Count - 1];

        if (!sameX && !sameY)
            return false;

        if (sameX)
        {
            if (coordY[coordY.Count - 1] - coordY[0] != coordY.Count - 1)
                return false;
        }
        else
        {
            if (coordX[coordX.Count - 1] - coordX[0] != coordX.Count - 1)
                return false;
        }

        return true;
    }

    public List<Sector> GetNearbySector(Sea sea, Sector sector)
    {

        List<Sector> sectors = new List<Sector>();

        (int xt, int yt) = sector.GetCoord();

        for(int x = -1; x <= 1; x++)
            {
                for(int y = -1; y <= 1; y++)
                {
                    if (!(sea.ValidateBorder(xt+x, yt+y)))
                        continue;

                    Sector checkSector = sea.GetSector(xt+x, yt+y);
                    sectors.Add(checkSector);
                }
            }
        return sectors;
    }


    public List<(int , int)> NormalizeCoords(List<(int x, int y)> coords)
    {
        coords.Sort((coord1, coord2) =>
        {
        if (coord1.x != coord2.x)
            return coord1.x.CompareTo(coord2.x);

        return coord1.y.CompareTo(coord2.y);

        });

        return coords;
    }
}

public class AttackResolver
{
    public StatusSector Resolve(Sector target)
    {
        StatusSector currentStatus = target.GetStatus();

        if (currentStatus == StatusSector.Hit || currentStatus == StatusSector.Miss)
            throw new Exception("It's already been attacked");

        if (currentStatus == StatusSector.Ship)
            return StatusSector.Hit;
        
        return StatusSector.Miss;
    }
}

public class Sinker
{
    private DeploymentOfficer deploymentOfficer;


    public Sinker(DeploymentOfficer deploymentOfficer)
    {
        this.deploymentOfficer = deploymentOfficer;
    }


    public void FloodShip(Ship ship, Sea sea)
    {
        List<Sector> place = ship.GetPlace();

        for(int i = 0; i < place.Count; i++)
        {

            List<Sector> nearbySectors = deploymentOfficer.GetNearbySector(sea, place[i]);

            for (int j = 0; j < nearbySectors.Count; j++)
            {
                if (nearbySectors[j].GetStatus() == StatusSector.Empty)
                    nearbySectors[j].Attack(StatusSector.Miss);
            }
        }
    }
}


    
