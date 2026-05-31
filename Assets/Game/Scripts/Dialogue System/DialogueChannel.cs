using System;
using UnityEngine;

public class DialogueChannel
{
    public event Action<string> DialogRequest;

    public void StartDialog(string knot)
    {
        Debug.Log("Event Send Request");
        DialogRequest?.Invoke(knot);
    }
}