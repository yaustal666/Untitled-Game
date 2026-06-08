using System;

public class DialogueChannel
{
    public event Action<string> DialogRequest;

    public void StartDialog(string knot)
    {
        DialogRequest?.Invoke(knot);
    }
}