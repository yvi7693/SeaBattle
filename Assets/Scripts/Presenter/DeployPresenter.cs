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
    deployBoard.CreateBoard();

    InitShips();
  }

  public void Next()
  {
    DeployAllShips();
    GameSession.Instance.Next();
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


  public bool ValidateDeploy(List<DeploySector> positions)
  {
    TurnRecon turnRecon = staff.GetTurnRecon();
    Sea sea = turnRecon.GetSeaDeploy();

    List<(int, int)> coord = ConvertSectors(positions);

    return deploymentOfficer.ValidateDeploy(sea, coord);
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
