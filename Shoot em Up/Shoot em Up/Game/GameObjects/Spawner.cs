﻿using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoot_em_Up
{
    class Spawner
    {
        private List<GameObject> objects;
        private Vector2f pos;

        public Spawner(Vector2f position)
        {
            this.pos = position;
            this.objects = new List<GameObject>();
        }

        public void Add(GameObject obj)
        {
            this.objects.Add(obj);
        }

        public bool NearPlayer(float x, float w)
        {
            return x + w > this.pos.X;
        }

        public void Spawn()
        {
            foreach (GameObject obj in this.objects)
            {
                Game.Add(obj);
            }
        }
    }
}
