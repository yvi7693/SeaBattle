using System;
using System.Collections.Generic;
using UnityEngine;

public class DeployPresenter : MonoBehaviour
{
    [SerializeField] private DeployBoard deployBoard;
    private Staff staff;

    public void Start()
    {
      staff = GameSession.Instance.staff;
    }

    public void DeployShip(List<DeploySector> place)
  {
    if (place.Count <= 0 || place.Count > 4) throw new Exception("an uncomparable number of sectors");

    for (int i = 0; i < place.Count; i++)
    {
      int x = place[i].GetX();
      int y = place[i].GetY();
    }
  }
}