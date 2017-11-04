using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Writes the default GameSettings preferences to the disk when the game is
/// started for the first time on the computer.
/// </summary>

/// <remarks>The default preferences might also be applied in some cases if the
/// preferences' data have been corrupted.</remarks>
public class DefaultSettingsApplier : MonoBehaviour {
    public static int DefaultPlayTime = 4;

    private string ExistingPlayerPrefsKey = "ExistingPlayerPrefs";

    /// <summary>
    /// From the main menu, writes the default settings to the disk if no
    /// preferences are found on the disk.
    /// </summary>
    void Awake()
    {
        if (PlayerPrefs.GetInt(ExistingPlayerPrefsKey, -1) == 1)
            return;

        ApplyDefaultSettings();
        PlayerPrefs.SetInt(ExistingPlayerPrefsKey, 1);
    }

    /// <summary>
    /// Writes the default settings to the disk.
    /// </summary>
    public static void ApplyDefaultSettings()
    {
        GameSettings defaultSettings;
        defaultSettings.PlayTime = DefaultPlayTime;

        GameSettingsToDiskInterface.Write(defaultSettings);
    }
}
