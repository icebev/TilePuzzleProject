using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmonkhetTilePuzzles
{
    /* AUDIOSTORE CLASS
     * Last modified by Joe Bevis 11/01/2022
     *****************************************/

    /// <summary>
    /// Stores the SoundEffects and Song to be accessible anywhere within the program
    /// Also used to keep track of the Mute setting
    /// </summary>
    public static class AudioStore
    {
        private static bool m_isMuted;

        public static SoundEffect m_tileSlideSFX;
        public static SoundEffect m_clickOnSFX;
        public static SoundEffect m_clickOffSFX;
        public static SoundEffect m_puzzleCompleteSFX;

        public static Song m_nileJourneyMusic;

        public static bool IsMuted
        {
            get { return AudioStore.m_isMuted; }
            set { AudioStore.m_isMuted = value; }
        }

        /// <param name="game">Takes the game instance to load the content to</param>
        public static void LoadAudio(TileGame game)
        {
            AudioStore.m_tileSlideSFX = game.Content.Load<SoundEffect>("audio/SFX/slide");
            AudioStore.m_clickOnSFX = game.Content.Load<SoundEffect>("audio/SFX/clickOn");
            AudioStore.m_clickOffSFX = game.Content.Load<SoundEffect>("audio/SFX/clickOff");
            AudioStore.m_puzzleCompleteSFX = game.Content.Load<SoundEffect>("audio/SFX/puzzleComplete");

            AudioStore.m_nileJourneyMusic = game.Content.Load<Song>("audio/music/nileJourneyMusicAmbience");
        }

    }
}
