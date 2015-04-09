﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;

namespace Pong {
    class State {

        // primary physics state
        public Vector2f position;
        public float orientation;

        // secondary physics state
        public Vector2f velocity;
        public float angularVelocity;

        // constant state
        public float mass;
        public float inverseMass;
        public float inertiaTensor;
        public float inverseInertiaTensor;

        #region Constructors

        public State() {
            position = new Vector2f(0,0);
            orientation = 0;

            velocity = new Vector2f(0,0);
            angularVelocity = 0;

            mass = 0;
            inverseMass = 0;
            inertiaTensor = 0;
            inverseInertiaTensor = 0;
        }

        public State(Vector2f position) {
            this.position = position;
            orientation = 0;

            velocity = new Vector2f(0, 0);
            angularVelocity = 0;

            mass = 0;
            inverseMass = 0;
            inertiaTensor = 0;
            inverseInertiaTensor = 0;
        }

        public State(float mass, float inertiaTensor) {
            position = new Vector2f(0, 0);
            orientation = 0;

            velocity = new Vector2f(0, 0);
            angularVelocity = 0;

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
            orientation = 0;

            velocity = new Vector2f(0, 0);
            angularVelocity = 0;

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

        private State Evaluate(State state, float dt, State derivative){
            state.position += derivative.velocity * dt;
            state.orientation += derivative.angularVelocity * dt;
            return state;
        }

        public void Integrate(float dt) {
            State k1 = this;
            State k2 = Evaluate(this, dt * 0.5f, k1);
            State k3 = Evaluate(this, dt * 0.5f, k2);
            State k4 = Evaluate(this, dt, k3);

            float k = 1.0f / 6.0f;

            position += k * dt * (k1.velocity + 2.0f * (k2.velocity + k3.velocity) + k4.velocity);
            orientation += k * dt * (k1.angularVelocity + 2.0f * (k2.angularVelocity + k3.angularVelocity) + k4.angularVelocity);
        }

        // to interpolate between two states

        public static State operator *(State state, float s) {
            State output = new State(state.mass, state.inertiaTensor);
            output.position = state.position * s;
            output.orientation = state.orientation * s;
            return output;
        }

        public static State operator *(float s, State state) {
            return state * s;
        }

        public static State operator +(State s1, State s2) {
            State output = new State(s1.mass, s1.inertiaTensor);
            output.position = s1.position + s2.position;
            output.orientation = s1.orientation + s2.orientation;
            return output;
        }

        public float DegOrientation {
            get {
                return (float)(orientation * 180.0f / Math.PI);
            }
        }
    }
}