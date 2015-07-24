using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Audio;

namespace Platformer {
    public static class SoundManager {
        public static bool on = true;
        public static Sound collectCoin = new Sound(new SoundBuffer("../Content/sounds/CollectCoin.wav"));
        public static Sound shatter = new Sound(new SoundBuffer("../Content/sounds/Shatter.wav"));
        public static Sound rebound = new Sound(new SoundBuffer("../Content/sounds/Rebound.wav"));
        public static Sound ambient = new Sound(new SoundBuffer("../Content/sounds/Phantom_from_Space.wav"));

        public static void Play(Sound s)
        {
            if (on)
            {
                s.Play();
            }
        }

        public static void Stop() {
            collectCoin.Stop();
            shatter.Stop();
            rebound.Stop();
            ambient.Stop();
        }
    }
}

