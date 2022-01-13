using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmonkhetTilePuzzles
{
    /* GAMESTATE ENUMERATION
     * Last modified by Joe Bevis 11/01/2022
     ****************************************/

    /// <summary>
    /// Allows switching between different screens by referring to the current state
    /// </summary>
    public enum GameState
    {
        AnimatedTitleScreen,
        MainTitleScreen,
        Credits,
        Instructions,
        OptionsScreen,
        PuzzleSelect,
        PuzzleActive,
        PuzzleComplete
    }
}
