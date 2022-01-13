using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AmonkhetTilePuzzles
{
    /* HIGHSCORE TRACKER CLASS
     * Last modified by Joe Bevis 11/01/2022
     ****************************************/
    /// <summary>
    /// Saves and loads the best scores using a .xml file so that they can persist between game sessions
    /// </summary>
    public class HighscoreTracker
    {
        #region Variables
        // Since we don't specify a path the xml file will be saved in the bin folder
        // e.g. ...\TilePuzzleProject\AmonkhetTilePuzzles\bin\DesktopGL\AnyCPU\Debug
        private static readonly string m_scoresFileName = "bestScores.xml"; 
        
        private List<ScoreEntry> m_bestScores;
        #endregion

        #region Properties
        public List<ScoreEntry> BestScores 
        { 
            get
            {
                return this.m_bestScores;
            }
            private set
            {
                this.m_bestScores = value;
            }
        }

        #endregion

        #region Constructors
        /// <summary>
        /// The constructor with zero arguments is used when there is no xml file found
        /// </summary>
        public HighscoreTracker()
            : this(new List<ScoreEntry>())
        {

        }
        /// <summary>
        /// Allocates the loaded scores during construction
        /// </summary>
        /// <param name="scoreEntries">The retrieved data from the xml file once serialized</param>
        public HighscoreTracker(List<ScoreEntry> scoreEntries)
        {
            this.m_bestScores = scoreEntries;
        }

        #endregion

        #region Getter Functions

        /// <returns>
        /// Outputs ScoreEntry objects from the BestScores list that match the specified grid size and puzzle image index number
        /// </returns>
        public List<ScoreEntry> GetRelevantEntries(int gridSize, int puzzleNumber)
        {
            var ScoreEntryTargets = from s in this.BestScores
                                    where s.GridSize == gridSize && s.PuzzleImageIndex == puzzleNumber
                                    select s;

            if (ScoreEntryTargets.Any())
                return ScoreEntryTargets.ToList();
            else
            {
                List<ScoreEntry> blankList = new List<ScoreEntry>();
                return blankList;
            }
        }
        public int GetBestMoves(int gridSize, int puzzleNumber)
        {
            List<ScoreEntry> EntriesToCompare = this.GetRelevantEntries(gridSize, puzzleNumber);
            int lowestMoveCount = 9999;
            foreach (ScoreEntry scoreEntry in EntriesToCompare)
            {
                if (scoreEntry.BestMoves < lowestMoveCount)
                    lowestMoveCount = scoreEntry.BestMoves;
            }
            return lowestMoveCount;
        }
        public int GetBestTime(int gridSize, int puzzleNumber)
        {
            List<ScoreEntry> EntriesToCompare = this.GetRelevantEntries(gridSize, puzzleNumber);
            int lowestTimeElapsed = 9999;
            foreach (ScoreEntry scoreEntry in EntriesToCompare)
            {
                if (scoreEntry.BestTime < lowestTimeElapsed)
                    lowestTimeElapsed = scoreEntry.BestTime;
            }
            return lowestTimeElapsed;

        }
        #endregion

        #region Class Methods

        /// <summary>
        /// Called upon puzzle completion to record the new score(s) IF they beat all previous scores
        /// </summary>
        public void UpdateScoreEntry(int gridSize, int puzzleNumber, int moveCount, int timeElapsed)
        {
            List<ScoreEntry> EntriesToCompare = this.GetRelevantEntries(gridSize, puzzleNumber);

            bool additionalEntryRequired = true;

            if (EntriesToCompare.Count > 0)
            {
                foreach (ScoreEntry scoreEntry in EntriesToCompare)
                {
                    // Beating the best time or best move count: a new entry IS required
                    if (scoreEntry.BestMoves <= moveCount && scoreEntry.BestTime <= timeElapsed)
                    {
                        additionalEntryRequired = false;
                    }
                }
            } 
            if (additionalEntryRequired)
            {
                this.m_bestScores.Add(new ScoreEntry()
                {
                    GridSize = gridSize,
                    PuzzleImageIndex = puzzleNumber,
                    BestMoves = moveCount,
                    BestTime = timeElapsed,

                });
            }
        }

        /* CODE CITATION:
         * Save and load methods for .xml file interactivity
         * Source Title: MonoGame Tutorial 015 - Store Highscores (XML)
         * Author: Oyyou (YouTube handle)
         * Date: May 2018 (accessed January 2022)
         * Availability: https://www.youtube.com/watch?v=JzEwVCgALuY&list=WL&index=60
         *******************************************************************************/
        // Start of cited code

        // Static - can be accessed without needing to be instantiated first for loading
        public static HighscoreTracker Load()
        {
            // If there isn't a file to load - create a new instance of filemanager
            if (!File.Exists(HighscoreTracker.m_scoresFileName))
            {
                return new HighscoreTracker();
            }

            // Otherwise we load the file and assign the content
            using (var reader = new StreamReader(new FileStream(HighscoreTracker.m_scoresFileName, FileMode.Open)))
            {
                var serializer = new XmlSerializer(typeof(List<ScoreEntry>));
                var scoreEntries = (List<ScoreEntry>)serializer.Deserialize(reader);
                return new HighscoreTracker(scoreEntries);
            }
        }
        public static void Save(HighscoreTracker highscoreTracker)
        {
            // overrides the file if it already exists
            using (var writer = new StreamWriter(new FileStream(HighscoreTracker.m_scoresFileName, FileMode.Create)))
            {
                var serializer = new XmlSerializer(typeof(List<ScoreEntry>));
                serializer.Serialize(writer, highscoreTracker.m_bestScores);
            }
        }
        // End of cited code

        /// <summary>
        /// Clears the BestScores list before overwriting the xml file
        /// </summary>
        public void ResetScores()
        {
            this.BestScores = new List<ScoreEntry>();
            Save(this);
        }
        #endregion
    }
}