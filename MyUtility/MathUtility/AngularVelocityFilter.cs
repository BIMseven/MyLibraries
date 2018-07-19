using UnityEngine;
using System.Collections;

namespace MyUtility
{
    // Keeps track of smoothed velocity (position must be updated regularly).  Uses a 
    // simple low pass filter.

    //Filter cannot stay updated when called during update, angularvelocityfilter tracker will have to
    //keep track of whether or not the position and deltaTime values are updated and consistent with 
    //the object's values. tldr: make a tracker and make sure it keeps the filter updated in a helper method.

    public class AngularVelocityFilter
    {
        //------------------------------------------------------------------------CONSTANTS:

        private const string LOG_TAG = "VelocityTracker";
        public bool VERBOSE = false;

        private const float EPSILON = 0.001f;

        private const float DEFAULT_SMOOTHING_WEIGHT = 0.5f;

        // We were getting calls to Update more frequently than the game was updating
        // transform.  To keep it from happening again, we ignore updates with same
        // position unless it keeps happining (in which case the object is probably
        // stopped)
        private const int TRANSFORM_UPDATES_PER_VELOCITY_UPDATES = 2;

        //---------------------------------------------------------------------------FIELDS:


        public Vector3 RawAngularVelocity;
        public Vector3 AngularVelocity;
        public Vector3 AngularAcceleration;

        // 0 puts more weight towards smoothed value, 1 puts more weight/trust into 
        // the raw value
        [Range( 0, 1.0f )]
        public float SmoothingWeight;

        public Quaternion RotLastFrame;

        // Helper for Update, compared against TRANSFORM_UPDATES_PER_VELOCITY_UPDATES
        private uint timesPositionEqualedPreviousPosition;

        // TODO If we want to save older smoothed velocities, use queue:
        //https://msdn.microsoft.com/en-us/library/7977ey2c(v=vs.110).aspx

        //---------------------------------------------------------------------CONSTRUCTORS:

        public AngularVelocityFilter()
        {
            SmoothingWeight = DEFAULT_SMOOTHING_WEIGHT;
        }

        public AngularVelocityFilter( float smoothingWeight )
        {
            SmoothingWeight = smoothingWeight;
        }

        public AngularVelocityFilter( Quaternion restingPosition, float smoothingWeight )
        {
            RotLastFrame = restingPosition;
            SmoothingWeight = smoothingWeight;
        }

//--------------------------------------------------------------------------METHODS:

        /// <summary>
        /// Sets velocity to zero
        /// </summary>
        /// <param name="restingPosition">The place the object is stopped</param>
        public void Clear( Quaternion restingPosition )
        {
            timesPositionEqualedPreviousPosition = 0;
            RawAngularVelocity = Vector3.zero;
            AngularVelocity = Vector3.zero;
            AngularAcceleration = Vector3.zero;
            RotLastFrame = restingPosition;
        }
               
        /// <summary>
        /// Updates the current velocity based on position reading
        /// </summary>    
        public void Update( Quaternion rotThisFrame, float deltaT )
        {
            //Debug.Log("AngVelFilt Updated, deltaT:" + deltaT);
            Vector3 velocityLastFrame = AngularVelocity;

            var rotSinceLastFrame = rotThisFrame * RotLastFrame.Inverse();

            float magnitude = 0f;
            Vector3 axis = Vector3.zero;
            rotSinceLastFrame.ToAngleAxis( out magnitude, out axis );


            RawAngularVelocity = ( axis * magnitude ) / deltaT;
            //magnitude *= Mathf.Deg2Rad;

            // Set the current velocity to be a combination of raw reading and the 
            // smoothed reading from previous frame.
            AngularVelocity = Vector3.Lerp( velocityLastFrame,
                                            RawAngularVelocity,
                                            SmoothingWeight );
            
            AngularAcceleration = AngularVelocity - velocityLastFrame;

            // We will need this for the next update
            RotLastFrame = rotThisFrame;
        }

//--------------------------------------------------------------------------HELPERS:
//--------------------------------------------------------------GETTERS AND SETTERS:
    }
}