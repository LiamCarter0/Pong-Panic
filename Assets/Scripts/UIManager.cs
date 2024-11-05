using System;
using UnityEngine;
using UnityEngine.UI;


//ultimately this class is kind of a failure because i cant figure out how to do actions that have any parameters 
//as i wanted to be able to use these to call themselves for easy access of screen to screen management. It shines
//for turning screens on and off if the resulting buttons don't lead to another screen but instead gameplay.

//I changed my mind, the first two functions are obsolete, would be useful maybe for bigger projects, but i found it simpler and straightforward to do it by hand

public class UIManager : MonoBehaviour
{
    // Method to assign actions to loaded buttons
    public void ShowPanelWithListeners(GameObject panel, Action listener)
    {
        panel.SetActive(true); // Show the panel
        Button[] buttons = panel.GetComponentsInChildren<Button>(true);

        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => listener());
        }
    }

    // Overloaded for different actions for the buttons
    public void ShowPanelWithListeners(GameObject panel, Action[] listeners)
    {
        panel.SetActive(true); 
        Button[] buttons = panel.GetComponentsInChildren<Button>(true);

        for (int i = 0; i < buttons.Length; i++)
        {
            if (i < listeners.Length) // so we don't go out of bounds
            {
                buttons[i].onClick.AddListener(() => listeners[i]());
            }
        }
    }

    // Function to hide the panel and remove listeners from all buttons in the panel
    public void HidePanelWithListeners(GameObject panel)
    {
        if (panel == null)
        {
            Debug.LogWarning("Panel is null. Cannot hide or remove listeners.");
            return;
        }

        Button[] buttons = panel.GetComponentsInChildren<Button>(true); // Includes inactive children

        foreach (Button button in buttons)
        {
            button.onClick.RemoveAllListeners();
        }

        panel.SetActive(false); // Hide the panel
    }

}
