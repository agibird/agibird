using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Loads the GameSettings preferences from the disk when the Settings panel is
/// opened, and writes them to the disk when the button "Apply" is clicked.
/// </summary>
public class SettingsPanelToSettingsSystemInterface : MonoBehaviour {
    public UnityEngine.UI.Slider PlayTimeSlider;

    /// <summary>
    /// Loads the preferences from the disk and sets the play time slider
    /// value.
    /// </summary>
    /// <remarks>The preferences are defaulted if an error occurs when loading
    /// them.</remarks>
    void OnEnable()
    {
        GameSettings currentSettings =
            GameSettingsToDiskInterface.LoadOrDefault();

        PlayTimeSlider.value = currentSettings.PlayTime;
    }

    /// <summary>
    /// Writes the preferences set in the settings panel to the disk.
    /// </summary>
    public void SaveSettings()
    {
        GameSettings newSettings;
        newSettings.PlayTime = (int) PlayTimeSlider.value;

        GameSettingsToDiskInterface.Write(newSettings);
    }
}
