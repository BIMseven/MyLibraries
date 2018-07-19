using UnityEngine;
using System.Collections;

namespace MyUtility
{
    public class VelocityTracker : MonoBehaviour
    {
//------------------------------------------------------------------------CONSTANTS:

        private const string LOG_TAG = "VelocityTracker";
        public bool VERBOSE = false;

        // 0 is conservative, 1 is confident in position

        private const float DEFAULT_SMOOTHING_WEIGHT = 0.2f;

        // In frames
        private const int MAX_FRAMES_BETWEEN_UPDATES = 4;

//---------------------------------------------------------------------------FIELDS:

        [Range( 0, 1f )]
        public float SmoothingWeight = DEFAULT_SMOOTHING_WEIGHT;

        [Range(0, 1f)]
        public float AngularSmoothingWeight = DEFAULT_SMOOTHING_WEIGHT;

        int rotFrames = 1;
        int posFrames = 1;

        public Vector3 RawVelocity
        {
            get
            {
                return velocitySmoother.RawVelocity;
            }
        }

        public Vector3 SmoothedVelocity
        {
            get
            {
                return velocitySmoother.SmoothedVelocity;
            }
        }

        public Vector3 Acceleration
        {
            get
            {
                return velocitySmoother.Acceleration;
            }
        }

        public Vector3 RawAngularVelocity
        {
            get
            {
                return angularVelocitySmoother.RawAngularVelocity;
            }
        }

        public Vector3 AngularVelocity
        {
            get
            {
                return angularVelocitySmoother.AngularVelocity;    
            }    
        }

        public Vector3 AngularAcceleration
        {
            get
            {
                return angularVelocitySmoother.AngularAcceleration;
            }

        }

        private AngularVelocityFilter angularVelocitySmoother;
        private VelocityFilter velocitySmoother;

        public float timeOfLastPositionUpdate, timeOfLastRotationUpdate;

        public bool TrackerUpdated;

//---------------------------------------------------------------------MONO METHODS:

        void Awake()
        {
            velocitySmoother = new VelocityFilter( SmoothingWeight );
			velocitySmoother.Clear( transform.position );
            angularVelocitySmoother = new AngularVelocityFilter(AngularSmoothingWeight);
            angularVelocitySmoother.Clear(transform.rotation);
        }
        
        void FixedUpdate()
        {
            if( transform.position != velocitySmoother.PreviousPosition ||
                posFrames >= MAX_FRAMES_BETWEEN_UPDATES )
            {
                velocitySmoother.Update( transform.position, posFrames * Time.fixedDeltaTime );
                posFrames = 1;
            }
            else posFrames++;

            if( transform.rotation != angularVelocitySmoother.RotLastFrame ||
                rotFrames >= MAX_FRAMES_BETWEEN_UPDATES )
            {
                angularVelocitySmoother.Update( transform.rotation, rotFrames * Time.fixedDeltaTime );
                rotFrames = 1;
            }
            else rotFrames++;
        }


//--------------------------------------------------------------------------METHODS:

        public void Clear()
		{
			velocitySmoother.Clear( transform.position );
		}

        public void Clear( Vector3 restingPosition )
        {
            velocitySmoother.Clear( restingPosition );
        }

        public void ClearAngularVelocitySmoother()
        {
            angularVelocitySmoother.Clear(transform.rotation);
        }

        public void ClearAngularVelocitySmoother(Quaternion restingPosition)
        {
            angularVelocitySmoother.Clear(restingPosition);
        }

//--------------------------------------------------------------------------HELPERS:
    }
}
    