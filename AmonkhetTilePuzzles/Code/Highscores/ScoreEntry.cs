using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmonkhetTilePuzzles
{
    /* SCOREENTRY CLASS
     * Last modified by Joe Bevis 11/01/2022
     ****************************************/

    /// <summary>
    /// A model used by the Highscore Tracker for saving scores into the .xml file
    /// Each score saved must include the specfic grid size and puzzle image index number
    /// This enables the appropritate scores to be retrieved when reviewing best scores
    /// </summary>
    public class ScoreEntry
    {
        public int GridSize { get; set; }
        public int PuzzleImageIndex { get; set; }
        public int BestMoves { get; set; }
        public int BestTime { get; set; }

    }
}
