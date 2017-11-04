using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides features to load GameSettings from disk and to write GameSettings
/// to the disk.
/// </summary>
public class GameSettingsToDiskInterface : MonoBehaviour
{
    private static string PlayTimeKey = "PlayTime";

    /// <summary>
    /// Loads the GameSettings preferences saved on disk via
    /// UnityEngine.GamePrefs.
    /// </summary>
    /// 
    /// <returns>The fetched preferences.</returns>
    /// <exception cref="System.InvalidOperationException">Thrown when the disk
    /// data is either inexistant, or invalid.</exception>
    public static GameSettings Load()
    {
        GameSettings result;

        result.PlayTime = PlayerPrefs.GetInt(PlayTimeKey, -1);
        if (result.PlayTime < 1)
            throw new System.InvalidOperationException(
                "Inexisting or corrupted PlayerPrefs data.");

        return result;
    }

    /// <summary>
    /// Uses Load() to fetch GameSettings preferences and defaults the
    /// preferences on the disk in case of failure.
    /// </summary>
    /// <returns>The loaded value or the defaulted value.</returns>
    public static GameSettings LoadOrDefault()
    {
        GameSettings result;

        try
        {
            result = Load();
        }
        catch (System.InvalidOperationException)
        {
            DefaultSettingsApplier.ApplyDefaultSettings();
            result = Load();
        }

        return result;
    }

    /// <summary>
    /// Writes the GameSettings preferences to the disk via
    /// UnityEngine.GamePrefs.
    /// </summary>
    /// <param name="settings">Settings to wite.</param>
    public static void Write(GameSettings settings)
    {
        PlayerPrefs.SetInt(PlayTimeKey, settings.PlayTime);

        PlayerPrefs.Save();
    }
}
