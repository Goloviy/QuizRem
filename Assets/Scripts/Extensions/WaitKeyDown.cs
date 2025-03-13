using UnityEngine;

public class WaitKeyDown : CustomYieldInstruction
{
    private KeyCode keyCode;

    public WaitKeyDown(KeyCode _keyCode)
    {
        keyCode = _keyCode;
    }

    public override bool keepWaiting => Input.GetKeyDown(keyCode) == false;
}