using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;

namespace Pong {
    class State {

        // primary physics state
        public Vector2f position;
        public Vector2f momentum;
        public float orientation;
        public float angularMomentum;

        // secondary physics state
        public Vector2f velocity;
        public Vector2f force;
        public float angularVelocity;
        public float torque;

        // constant state
        public float mass;
        public float inverseMass;
        public float inertiaTensor;
        public float inverseInertiaTensor;

        #region Constructors

        public State() {
            position = new Vector2f(0,0);
            momentum = new Vector2f(0,0);
            orientation = 0;
            angularMomentum = 0;

            velocity = new Vector2f(0,0);
            force = new Vector2f(0,0);
            angularVelocity = 0;
            torque = 0;

            mass = 0;
            inverseMass = 0;
            inertiaTensor = 0;
            inverseInertiaTensor = 0;
        }

        public State(Vector2f position) {
            this.position = position;
            momentum = new Vector2f(0, 0);
            orientation = 0;
            angularMomentum = 0;

            velocity = new Vector2f(0, 0);
            force = new Vector2f(0, 0);
            angularVelocity = 0;
            torque = 0;

            mass = 0;
            inverseMass = 0;
            inertiaTensor = 0;
            inverseInertiaTensor = 0;
        }

        public State(float mass, float inertiaTensor) {
            position = new Vector2f(0, 0);
            momentum = new Vector2f(0, 0);
            orientation = 0;
            angularMomentum = 0;

            velocity = new Vector2f(0, 0);
            force = new Vector2f(0, 0);
            angularVelocity = 0;
            torque = 0;

            if (mass != 0 && inertiaTensor != 0) {
                this.mass = mass;
                inverseMass = 1.0f / mass;
                this.inertiaTensor = inertiaTensor;
                inverseInertiaTensor = 1.0f / inertiaTensor;
            } else {
                mass = 0;
                inverseMass = 0;
                inertiaTensor = 0;
                inverseInertiaTensor = 0;
            }
        }

        public State(Vector2f position, float mass, float inertiaTensor) {
            this.position = position;
            momentum = new Vector2f(0, 0);
            orientation = 0;
            angularMomentum = 0;

            velocity = new Vector2f(0, 0);
            force = new Vector2f(0, 0);
            angularVelocity = 0;
            torque = 0;

            if (mass != 0 && inertiaTensor != 0) {
                this.mass = mass;
                inverseMass = 1.0f / mass;
                this.inertiaTensor = inertiaTensor;
                inverseInertiaTensor = 1.0f / inertiaTensor;
            } else {
                mass = 0;
                inverseMass = 0;
                inertiaTensor = 0;
                inverseInertiaTensor = 0;
            }
        }

        #endregion

        private void Recalculate() {
            velocity = momentum * inverseMass;
            angularVelocity = angularMomentum * inverseInertiaTensor;
        }

        private State Evaluate(State state, float dt, State derivative){
            state.position += derivative.velocity * dt;
            state.momentum += derivative.force * dt;
            state.orientation += derivative.angularVelocity * dt;
            state.angularMomentum += derivative.torque * dt;
            return state;
        }

        public void Integrate(float dt) {
            State k1 = this;
            State k2 = Evaluate(this, dt * 0.5f, k1);
            State k3 = Evaluate(this, dt * 0.5f, k2);
            State k4 = Evaluate(this, dt, k3);

            float k = 1.0f / 6.0f;

            position += k * dt * (k1.velocity + 2.0f * (k2.velocity + k3.velocity) + k4.velocity);
            momentum += k * dt * (k1.force + 2.0f * (k2.force + k3.force) + k4.force);
            orientation += k * dt * (k1.angularVelocity + 2.0f * (k2.angularVelocity + k3.angularVelocity) + k4.angularVelocity);
            angularMomentum += k * dt * (k1.torque + 2.0f * (k2.torque + k3.torque) + k4.torque);
            Recalculate();
        }
    }
}
