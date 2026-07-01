using System;
using System.Collections.Generic;
using UnityEngine;

public class DeployPresenter : MonoBehaviour
{
    [SerializeField] private DeployBoard deployBoard;
    [SerializeField] private GameObject parentShip;
    private Staff staff;


    public void Start()
    {
      staff = GameSession.Instance.staff;
      deployBoard.CreateBoard();

      InitShips();
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

  private void InitShips()
  {
    DeployShip[] ships = parentShip.GetComponentsInChildren<DeployShip>();

    for(int i = 0; i < 0; i++)
    {
      ships[i].Init(this);
    }
  }
}
