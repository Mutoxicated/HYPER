using UnityEngine;

public class ButtonInput
{
    public string ID;
    public bool lastFrame;
    public bool currentFrame;

    public ButtonInput(string id)
    {
        ID = id;
    }

    public void Update()
    {
        lastFrame = currentFrame;
        currentFrame = Input.GetButton(ID);
    }

    public bool GetInputDown()
    {
        return !lastFrame && currentFrame;
    }

    public bool GetInput()
    {
        return currentFrame;
    }
}
