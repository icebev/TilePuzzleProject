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
    public class HighscoreTracker
    {
        private static string m_scoresFileName = "bestScores.xml"; // since we don't give a path it will be saved in the bin folder

        private List<ScoreEntry> m_bestScores;
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
        //public List<Score> Scores { get; private set; }

        public HighscoreTracker()
            : this(new List<ScoreEntry>())
        {

        }
        public HighscoreTracker(List<ScoreEntry> scoreEntries)
        {
            this.m_bestScores = scoreEntries;
        }

        public void UpdateScoreEntry(int gridSize, int puzzleNumber, int moveCount, int timeElapsed)
        {
            List<ScoreEntry> EntriesToCompare = this.GetRelevantEntries(gridSize, puzzleNumber);

            bool additionalEntryRequired = true;

            if (EntriesToCompare.Count > 0)
            {
                foreach (ScoreEntry scoreEntry in EntriesToCompare)
                {
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

        public List<ScoreEntry> GetRelevantEntries(int gridSize, int puzzleNumber)
        {
            var ScoreEntryTargets = from s in this.m_bestScores
                                    where s.GridSize == gridSize && s.PuzzleImageIndex == puzzleNumber
                                    select s;

            return ScoreEntryTargets.ToList();
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

        // static - can be accessed without needing to be instantiated first
        public static HighscoreTracker Load()
        {
            // if there isn't a file to load - create a new instance of filemanager
            if (!File.Exists(HighscoreTracker.m_scoresFileName))
            {
                return new HighscoreTracker();
            }

            // otherwise we load the file and assign the content

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
    }
}