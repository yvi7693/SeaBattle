using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeployPresenter : MonoBehaviour
{
    [SerializeField] private DeployBoard deployBoard;
    [SerializeField] private GameObject parentShip;
    private Staff staff;
    private DeploymentOfficer deploymentOfficer;
    private DeployShip[] deployShips;
    


  public void Start()
  {
    
    staff = GameSession.Instance.staff;
    deploymentOfficer = staff.GetDeploymentOfficer();

    deployBoard.CreateBoard();

    InitShips();
  }

  public void Next()
  {
    DeployAllShips();
    GameSession.Instance.Next();
  }


  public void DeployFleet() 
  {
    staff.DeployFleet();
  }
  
  private void DeployAllShips()
  {
    for (int i = 0; i < deployShips.Length; i++)
    {
      deployShips[i].Deploy();
    }
  }


  public void DeployShip(List<DeploySector> place)
  {
    if (place.Count <= 0 || place.Count > 4) throw new Exception("an uncomparable number of sectors");

    List<(int, int)> coords = new List<(int, int)>();

    for (int i = 0; i < place.Count; i++)
    {
      int x = place[i].GetX();
      int y = place[i].GetY();

      coords.Add((x, y));
    }

    staff.DeployDirective(coords);

    // Временный код

    for (int i = 0; i < coords.Count; i++)
    {
        Debug.Log($"Позиция {i}: {coords[i]}");
    }
  }


  public bool ValidateDeploy(List<DeploySector> sectors)
  {
    List<(int, int)> positions = ConvertSectors(sectors);

     for (int i = 0; i < positions.Count; i++)
        {
            (int x, int y) = positions[i];

            DeploySector sector = deployBoard.GetSector(x, y);

            if (!(sector.IsEmpty()))
              return false;
                

            List<DeploySector> nearbySectors = GetNearbySector(sector);

            for (int j = 0; j < nearbySectors.Count; j++)
            {
                if (!(nearbySectors[j].IsEmpty()))
                  return false;   
            }
        }
      
        return true;
  }


  public List<DeploySector> GetNearbySector(DeploySector sector)
    {

        List<DeploySector> sectors = new List<DeploySector>();

        (int xt, int yt) = sector.GetCoord();

        for(int x = -1; x <= 1; x++)
            {
                for(int y = -1; y <= 1; y++)
                {
                    if (!(deployBoard.ValidateBorder(xt+x, yt+y)))
                        continue;

                    DeploySector checkSector = deployBoard.GetSector(xt+x, yt+y);
                    sectors.Add(checkSector);
                }
            }
        return sectors;
    }


  private void InitShips()
  {

    DeployShip[] ships = parentShip.GetComponentsInChildren<DeployShip>();
    deployShips = ships;

    for(int i = 0; i < ships.Length; i++)
    {
      ships[i].Init(this);

      
    }
  }


  private List<(int, int)> ConvertSectors(List<DeploySector> sectors)
  {
    List<(int, int)> coords = new List<(int, int)>();

    for (int i = 0; i < sectors.Count; i++)
    {
      int x = sectors[i].GetX();
      int y = sectors[i].GetY();

      coords.Add((x,y));
    }

    return coords;
  }


 
}
