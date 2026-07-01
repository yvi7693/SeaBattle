using UnityEngine;

public class DeploySector : MonoBehaviour
{
    private int x;
    private int y;

    public void Init(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public int GetX()
    {
        return x;
    }

    public int GetY()
    {
        return y;
    }
}

