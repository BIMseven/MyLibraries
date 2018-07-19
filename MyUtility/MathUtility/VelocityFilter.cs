﻿using UnityEngine;
using System.Collections;

namespace MyUtility
{
    // Keeps track of smoothed velocity (position must be updated regularly).  Uses a 
    // simple low pass filter.
    public class VelocityFilter
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

        public Vector3 RawVelocity { get; private set; }
        public Vector3 SmoothedVelocity { get; private set; }
        public Vector3 Acceleration { get; private set; }

        // 0 puts more weight towards smoothed value, 1 puts more weight/trust into 
        // the raw value
        [Range( 0, 1.0f )]
        public float SmoothingWeight;

        public Vector3 PreviousPosition;

        // Helper for Update, compared against TRANSFORM_UPDATES_PER_VELOCITY_UPDATES
        private uint timesPositionEqualedPreviousPosition;

        // TODO If we want to save older smoothed velocities, use queue:
        //https://msdn.microsoft.com/en-us/library/7977ey2c(v=vs.110).aspx

//---------------------------------------------------------------------CONSTRUCTORS:

        public VelocityFilter()
        {
            SmoothingWeight = DEFAULT_SMOOTHING_WEIGHT;
        }

        public VelocityFilter( float smoothingWeight )
        {
            SmoothingWeight = smoothingWeight;
		}

		public VelocityFilter( Vector3 restingPosition, float smoothingWeight )
		{
			PreviousPosition = restingPosition;
			SmoothingWeight = smoothingWeight;
		}

//--------------------------------------------------------------------------METHODS:

        /// <summary>
        /// Sets velocity to zero
        /// </summary>
        /// <param name="restingPosition">The place the object is stopped</param>
        public void Clear( Vector3 restingPosition )
        {
            timesPositionEqualedPreviousPosition = 0;
            RawVelocity = Vector3.zero;
            SmoothedVelocity = Vector3.zero;
            PreviousPosition = restingPosition;
        }

        /// <summary>
        /// Updates the current velocity based on position reading
        /// </summary>    
        public void Update( Vector3 position, float deltaT )
        {
            if( isBadValue( position ) )
            {
                return;
            }
            Vector3 velocityLastFrame = SmoothedVelocity;

            // Get a raw reading of velocity
            RawVelocity = ( position - PreviousPosition ) / deltaT;
            

            // Set the current velocity to be a combination of raw reading and the 
            // smoothed reading from previous frame.
            SmoothedVelocity = Vector3.Lerp( velocityLastFrame,
                                             RawVelocity,
                                             SmoothingWeight );

            //Vector3 accelerationLastFrame = Acceleration;
            //Vector3 accelerationThisFrame = SmoothedVelocity - velocityLastFrame;
            //Acceleration = Vector3.Lerp( Acceleration, 
            //                             accelerationThisFrame, 
            //                             SmoothingWeight );
            Acceleration = SmoothedVelocity - velocityLastFrame;

            // We will need this for the next update
            PreviousPosition = position;
        }

//--------------------------------------------------------------------------HELPERS:

        private bool isBadValue(Vector3 position)
        {
            // Sometimes user feeds the same position.  We need to count how many times
            // position was equal to previousPosition because we might actually be 
            // stopped, but we don't want a bum value to affect our speed if it isn't
            if (position == PreviousPosition &&
                ++timesPositionEqualedPreviousPosition > TRANSFORM_UPDATES_PER_VELOCITY_UPDATES)
            {
                Debug.Log("Bad value!");
                return true;
            }
            timesPositionEqualedPreviousPosition = 0;
            return false;
        }

//--------------------------------------------------------------GETTERS AND SETTERS:
    }
}