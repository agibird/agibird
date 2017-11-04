using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Represents some instance of the game settings. Here, game settings
/// means player customizable settings such as the playing time.
/// </summary>
public struct GameSettings
{
    // Time of a round in minutes. Must be greater than or equal to 1.
    public int PlayTime;
}