using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmonkhetTilePuzzles
{
    public static class AudioStore
    {
        public static bool m_isMuted;

        public static SoundEffect m_tileSlideSFX;
        public static SoundEffect m_clickOnSFX;
        public static SoundEffect m_clickOffSFX;
        public static SoundEffect m_puzzleCompleteSFX;

        public static Song m_nileJourneyMusic;
    }
}
