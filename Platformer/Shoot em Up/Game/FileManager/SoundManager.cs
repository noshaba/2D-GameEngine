using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Audio;

namespace Platformer {
    public static class SoundManager {
        public static bool on = true;
        public static Sound scoreSound = new Sound(new SoundBuffer("../Content/score.ogg"));
        public static Sound hitSound = new Sound(new SoundBuffer("../Content/Hit.wav"));
        public static Sound ambient = new Sound(new SoundBuffer("../Content/Mellowtron.wav"));

        public static void Play(Sound s)
        {
            if (on)
            {
                s.Play();
            }
        }

        public static void stop() {
            scoreSound.Stop();
            hitSound.Stop();
            ambient.Stop();
        }
    }
}

