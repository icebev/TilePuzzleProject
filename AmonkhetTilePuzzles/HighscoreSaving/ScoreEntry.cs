using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmonkhetTilePuzzles
{
    public class ScoreEntry
    {
        public int GridSize { get; set; }
        public int PuzzleImageIndex { get; set; }
        public int BestMoves { get; set; }
        public int BestTime { get; set; }

    }
}
