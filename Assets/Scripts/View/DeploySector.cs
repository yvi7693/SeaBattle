using UnityEngine;

public class DeploySector : MonoBehaviour
{
    private int x;
    private int y;
    private DeployPresenter deployPresenter;

    public void Init(int x, int y, DeployPresenter deployPresenter)
    {
        this.x = x;
        this.y = y;
        this.deployPresenter = deployPresenter;
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

