using UnityEngine;

public class BasicCubeHoldable : HoldableObject, IInteractable
{
    public int testNum;

    public override bool HoldableInteractionTest(Component testAgainst)
    {
        BasicCubeInteractable b = testAgainst as BasicCubeInteractable;
        if (b == null) return false;
        
        return b.testNum == testNum;
    }

    public void Interact()
    {
        
    }
}
