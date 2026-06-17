using UnityEngine;

public class BattlePresenter : MonoBehaviour
{
    private Staff staff;
    [SerializeField] private BoardView boardView;

    public void AttackSector(SectorView sectorView, int targetX, int targetY)
    {
        MissionResult result = staff.TacticalDirective(targetX, targetY);

        if (result == MissionResult.Miss)

            sectorView.DisplayMiss();

        else if(result == MissionResult.Hit)
            sectorView.DisplayHit();
    }

    private void Start()
    {
        boardView.Init(this);
    }
}

