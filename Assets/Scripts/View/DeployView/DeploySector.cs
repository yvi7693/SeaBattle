using UnityEngine;

public class DeploySector : MonoBehaviour
{
    private int x;
    private int y;
    private StatusSector status;
    

    public void Init(int x, int y)
    {
        this.x = x;
        this.y = y;
        this.status = StatusSector.Empty;
    }


    public int GetX()
    {
        return x;
    }


    public int GetY()
    {
        return y;
    }


    public void SetStatus(StatusSector newStatus)
    {
        status = newStatus;
    }


    public StatusSector GetStatus()
    {
        return status;
    }


    public bool IsEmpty()
    {
        return status == StatusSector.Empty;
    }


     public (int, int) GetCoord()
    {
        return (x,y);
    }
}

