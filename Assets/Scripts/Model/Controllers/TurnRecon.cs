using System;

public class TurnRecon
{
    private Sea rightSea;
    private Sea leftSea;
    private Sea targetSea;

    public TurnRecon(Fleet rightFleet, Fleet leftFleet)
    {
        rightSea = new Sea(rightFleet);
        leftSea = new Sea(leftFleet);

        targetSea = leftSea;
    }

    public Sea GetTargetSea()
    {
        return targetSea;
    }

    public Sea GetAttackerSea()
    {
        if (targetSea == rightSea)
            return leftSea;

        return rightSea;
    }

    public Sea GetRightSea()
    {
        return rightSea;
    }

    public Sea GetLeftSea()
    {
        return leftSea;
    }

    public Sea GetDeploySea()
    {
        if (rightSea.IsShipsDeploy() && leftSea.IsShipsDeploy())
            throw new Exception("All ships are stationed");

        if (rightSea.IsShipsDeploy())
            return leftSea;

        return rightSea;
        
    }

    public void SwitchTargetSea()
    {
        if (targetSea == rightSea)
        {
            targetSea = leftSea;
        }

        else
        {
            targetSea = rightSea;
        }
    }
}
