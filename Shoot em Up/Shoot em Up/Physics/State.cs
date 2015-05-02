using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
using SFML.System;

namespace Shoot_em_Up {
    class State {

        // primary physics state
        public Vector2f position;
        public float orientation;
        public Mat22f worldTransform;
        public Mat22f localTransform;

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
            worldTransform = Mat22f.Eye();
            localTransform = Mat22f.Eye();

            velocity = new Vector2f(0,0);
            angularVelocity = 0;

            mass = 0;
            inverseMass = 0;
            inertiaTensor = 0;
            inverseInertiaTensor = 0;
        }

        public State(Vector2f position, float rotation) {
            this.position = position;
            orientation = (float) (rotation * Math.PI / 180.0);
            worldTransform = Mat22f.RotationMatrix(orientation);
            localTransform = ~worldTransform;

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
            worldTransform = Mat22f.Eye();
            localTransform = Mat22f.Eye();

            velocity = new Vector2f(0, 0);
            angularVelocity = 0;

            this.mass = mass;
            inverseMass = mass != 0 ? 1.0f / mass : 0;
            this.inertiaTensor = inertiaTensor;
            inverseInertiaTensor = inertiaTensor != 0 ? 1.0f / inertiaTensor : 0;
        }

        public State(Vector2f position, float rotation, float mass, float inertiaTensor) {
            this.position = position;
            orientation = (float)(rotation * Math.PI / 180.0);
            worldTransform = Mat22f.RotationMatrix(orientation);
            localTransform = ~worldTransform;

            velocity = new Vector2f(0, 0);
            angularVelocity = 0;

            this.mass = mass;
            inverseMass = mass != 0 ? 1.0f / mass : 0;
            this.inertiaTensor = inertiaTensor;
            inverseInertiaTensor = inertiaTensor != 0 ? 1.0f / inertiaTensor : 0;
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
            worldTransform = Mat22f.RotationMatrix(orientation);
            localTransform = ~worldTransform;
        }

        public void Reset() {
            position = new Vector2f(0,0);
            orientation = 0;
            velocity = new Vector2f(0,0);
            angularVelocity = 0;
            worldTransform = Mat22f.Eye();
            localTransform = Mat22f.Eye();
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

        public static State operator -(State s1, State s2) {
            State output = new State(s1.mass, s1.inertiaTensor);
            output.position = s1.position - s2.position;
            output.orientation = s1.orientation - s2.orientation;
            return output;
        }

        public float DegOrientation {
            get {
                return (float)(orientation * 180.0 / Math.PI);
            }
        }
    }
}
