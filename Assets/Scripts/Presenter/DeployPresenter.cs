using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeployPresenter : MonoBehaviour
{
    [SerializeField] private DeployBoard deployBoard;
    [SerializeField] private GameObject parentShip;
    [SerializeField] private TMP_Text playerText;
    private Staff staff;
    private DeploymentOfficer deploymentOfficer;
    private DeployShip[] deployShips;
    private bool isAutoDeployed = false;
    private bool hasDeployShips = false;
    private Sea deploySea;
    


  public void Start()
  {
    staff = GameSession.Instance.staff;
    deploymentOfficer = staff.GetDeploymentOfficer();

    deployBoard.CreateBoard();

    InitShips();

    playerText.text = $"{GameSession.Instance.playerDeploy}";

    deploySea = staff.GetTurnRecon().GetDeploySea();
  }


  public void Next()
  {
    if (!isAutoDeployed)
      PushShips();

    GameSession.Instance.Next();
  }


  public void DeployFleet() 
  {
    if (isAutoDeployed || deployBoard.HasShip())
    {
      deploySea.Clear();
      deploySea.RecallFleet();
      UnDeployShips();
    }
      
      
    staff.DeployFleet();

    Fleet fleet = deploySea.GetFleet();
    Ship[] shipsModel = fleet.GetShips();

    EnumerationShip(shipsModel);

    isAutoDeployed = true;

  }


  private void EnumerationShip(Ship[] shipsModel)
  {
    for (int i = 0; i < shipsModel.Length; i++)
    {
      List<(int x, int y)> coords = new List<(int, int)>(); 
      List<Sector> placeModel = shipsModel[i].GetPlace();

      for (int j = 0; j < placeModel.Count; j++)
      {
        (int x, int y) = placeModel[j].GetCoord();
        coords.Add((x, y));
      }

      List<(int , int)> sortCoords = NormalizeCoords(coords);

      (int xFirst, int yFirst) = sortCoords[0];

      DeploySector deploySector = deployBoard.GetSector(xFirst, yFirst);
      Collider2D colliderTarget = deploySector.GetComponent<Collider2D>();

      SyncPlaceShip(sortCoords, colliderTarget);
    }
  }

  
  private List<(int , int)> NormalizeCoords(List<(int x, int y)> coords)
  {
    coords.Sort((coord1, coord2) =>
    {
      if (coord1.x != coord2.x)
        return coord1.x.CompareTo(coord2.x);

      return coord1.y.CompareTo(coord2.y);

    });

    return coords;
  }


  private void SyncPlaceShip(List<(int, int)> coords, Collider2D target)
  {
    for (int i = 0; i < deployShips.Length; i++)
    {
      if (deployShips[i].GetDurability() == coords.Count && !deployShips[i].IsDeploy())
      {
        bool isVertical = false;

        if (coords.Count > 1)
        {
          (int x1, int y1) = coords[0];
          (int x2, int y2) = coords[1];

          isVertical = (x1 == x2);
        }

        deployShips[i].SyncPlace(target, isVertical);
        break;
      }

      
    }
  }
  

  private void PushShips()
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

    hasDeployShips = true;
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
      ships[i].Init(this);
    
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

  private void UnDeployShips()
  {
    for (int i = 0; i < deployShips.Length; i++)
    {
      deployShips[i].UnDeploy();
    }
  }

}
