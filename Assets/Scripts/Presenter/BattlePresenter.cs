using UnityEngine;

public class BattlePresenter : MonoBehaviour
{
    private Staff staff;
    private SectorView sectorView;

    public BattlePresenter(Staff staff, SectorView sectorView)
    {
        this.staff = staff;
        this.sectorView = sectorView;
    }

    public MissionResult AttackSector(int targetX, int targetY)
    {
        return staff.TacticalDirective(targetX, targetY);
    }
}

