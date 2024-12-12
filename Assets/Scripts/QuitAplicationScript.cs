using UnityEngine;
using UnityEngine.InputSystem;

public class QuitAplication : MonoBehaviour
{
    void Update()
    {
        if(Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Application.Quit();
        }
    }
}
