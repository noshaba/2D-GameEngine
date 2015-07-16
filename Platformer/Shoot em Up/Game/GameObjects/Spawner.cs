using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Physics;

namespace Platformer
{
    class Spawner
    {
        private GameObject refObj;
        private float minDistance;
        public int minX, maxX;
        private List<GameObject> objects = new List<GameObject>();
        private List<Body> rigidBodies = new List<Body>();

        public Spawner(int minX, int maxX, GameObject refObj, float minDistance)
        {
            this.minX = minX;
            this.maxX = maxX;
            this.refObj = refObj;
            this.minDistance = minDistance;
        }

        public bool Near()
        {
            return refObj.rigidBody.COM.X + refObj.rigidBody.Radius > (minX - minDistance) && 
                   refObj.rigidBody.COM.X - refObj.rigidBody.Radius < (maxX + minDistance);
        }

        public bool Inside(GameObject obj)
        {
            return obj.rigidBody.COM.X + obj.rigidBody.Radius > minX &&
                   obj.rigidBody.COM.X - obj.rigidBody.Radius < maxX;
        }

        public bool Contains(GameObject obj)
        {
            return objects.Contains(obj);
        }

        public void Clear()
        {
            objects.Clear();
            rigidBodies.Clear();
        }

        public void Remove(GameObject obj)
        {
            objects.Remove(obj);
            rigidBodies.Remove(obj.rigidBody);
        }

        public void Add(GameObject obj)
        {
            objects.Add(obj);
            rigidBodies.Add(obj.rigidBody);
        }

        public void AddToGame()
        {
            if (Near())
                Game.AddRange(objects, rigidBodies);
        }

    }
}
