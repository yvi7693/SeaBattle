using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeployPresenter : MonoBehaviour
{
    [SerializeField] private DeployBoard deployBoard;
    [SerializeField] private GameObject parentShip;
    [SerializeField] private TMP_Text playerText;
    [SerializeField] private MessagePanel panel;
    private Staff staff;
    private DeployShip[] deployShips;

    private Sea deploySea;
    


  public void Start()
  {
    staff = GameSession.Instance.staff;

    deployBoard.CreateBoard();

    InitShips();

    playerText.text = $"{GameSession.Instance.playerDeploy}";

    deploySea = staff.GetTurnRecon().GetDeploySea();
  }


  public void Next()
  {
    
    PushShips();

    GameSession.Instance.Next();
  }


  public void RandomPlace()
  {
    UnDeployShips();
    deployBoard.UnDeploySectors();
    PlaceAllShips();
  }


  public MessagePanel GetPanel()
  {
    return panel;
  }


  public void SetStatusSector(StatusSector newStatus, List<DeploySector> sectors)
  {
      for(int i = 0; i < sectors.Count; i++)
          sectors[i].SetStatus(newStatus);
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

  }


  public bool ValidateDeploy(List<DeploySector> sectors)
  {
  
     for (int i = 0; i < sectors.Count; i++)
        {
            if (!(sectors[i].IsEmpty()))
              return false;
              
            List<DeploySector> nearbySectors = GetNearbySector(sectors[i]);

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


  private void UnDeployShips()
  {
    for (int i = 0; i < deployShips.Length; i++)
    {
      deployShips[i].UnDeploy();
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


  private void PlaceAllShips()
    {

        for (int restart = 0; restart < 1000; restart++)
        {
            System.Random random = new System.Random();

            bool isFleetDeployed = true;

            for (int i = 0; i < deployShips.Length; i++)
            {
                bool isWork = true;
                
                int attempts = 0;

                while (isWork && attempts < 1000)
                {
                    attempts ++;

                    List<(int, int)> coords = new List<(int, int)>();

                    int durability = deployShips[i].GetDurability();

                    int x = random.Next(0, 10);
                    int y = random.Next(0, 10);

                    for (int j = 0; j < durability; j++)
                    {
                        if (x + durability < 10)   
                            coords.Add((x+j, y));
                        else if (y + durability < 10)
                            coords.Add((x, y+j));

                        else if (x - durability > 0)
                            coords.Add((x-j, y));
                        
                        else if (y - durability > 0)
                            coords.Add((x, y-j));
                    }

                    if (ValidateDeploy(deployBoard.GetSector(coords)))
                    {
                      isWork = false;
                      SyncPlaceShip(coords);
                    }
                }

                if (isWork)
                {
                    isFleetDeployed = false;
                    UnDeployShips();
                    break;
                }

            }

            if (isFleetDeployed)
                return;

        }

        throw new Exception("restarts limits have expired");
        
    }


    private void SetPlaceShip(List<(int, int)> coords, Collider2D target)
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

        deployShips[i].SetPlace(target, isVertical);
        break;
      }  
    }
  }
  

  private void PushShips()
  {
    for (int i = 0; i < deployShips.Length; i++)
    {
       List<DeploySector> coord = deployShips[i].SearchSectors();
        DeployShip(coord);
    }
  }


  private Collider2D SearchTargetCollider(List<(int,int)> coords) // отсортированные координаты слева - на право или всерху - вниз
  {
      (int xFirst, int yFirst) = coords[0];

      DeploySector firstDeploySector = deployBoard.GetSector(xFirst, yFirst);
      Collider2D colliderTarget = firstDeploySector.GetComponent<Collider2D>();

      return colliderTarget;
  }


  private void SyncPlaceShip(List<(int, int)> coords) 
  {
    List<(int , int)> sortCoords = NormalizeCoords(coords);

    SetStatusSector(StatusSector.Ship, deployBoard.GetSector(coords));

    Collider2D colliderTarget = SearchTargetCollider(sortCoords);
    
    SetPlaceShip(sortCoords, colliderTarget);
    
  }
}
