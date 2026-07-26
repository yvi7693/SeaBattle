using System;
using System.Collections.Generic;
using UnityEngine;

public class DeployBoard : MonoBehaviour
{
    [SerializeField]private GameObject sectorPrefab;
    [SerializeField]private int sizeSea = 10;
    [SerializeField]private float spacing = 0.5f;
    private DeploySector [,] sectors;

    public DeploySector GetSector(int x, int y)
    {
        if (!(ValidateBorder(x,y)))
            throw new ArgumentException("incorrect value");
        
        return sectors[x,y];
    }

    public List<DeploySector> GetSector(List<(int, int)> coords)
    {
        List<DeploySector> sectors = new List<DeploySector>();

        for (int i = 0; i < coords.Count; i++)
        {
            (int x , int y) = coords[i];
            sectors.Add(GetSector(x, y));
        }

        return sectors;
    }

    public void CreateBoard()
    {
        if (sizeSea < 5) throw new ArgumentException("incorrect value");
        
        sectors = new DeploySector[sizeSea, sizeSea];

        for (int x = 0; x < sizeSea; x++)
        {
            for (int y = 0; y < sizeSea; y++)
            {
                GameObject sectorObject = Instantiate(sectorPrefab, transform);
                sectorObject.transform.localPosition = new Vector3(x * spacing, y * spacing, 0);

                DeploySector deploySector = sectorObject.GetComponent<DeploySector>();
                deploySector.Init(x ,y);

                sectors[x,y] = deploySector;

                 sectorObject.name = $"Sector_{x}_{y}";
            }
        }

    }

    public bool ValidateBorder(int x, int y)
    {
        return x >= 0 && x < sizeSea && y >= 0 && y < sizeSea;
    }


    public bool HasShip()
    {
        for (int i = 0; i < sectors.GetLength(0); i++)
        {
            for (int j = 0; j < sectors.GetLength(1); j++)
            {
                if (sectors[i,j].GetStatus() == StatusSector.Ship)
                    return true;
            }
        }

        return false;
    }

    
}
