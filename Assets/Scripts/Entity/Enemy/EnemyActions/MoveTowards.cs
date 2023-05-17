using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[Serializable]
public class MoveTowards: iEnemyAction
{
    //Attributes
    [SerializeField]
    private float _speedModifier;
    [SerializeField]
    private float _accelerationModifier;
    [SerializeField]
    private float _steeringSpeed;
    [SerializeField]
    private float _maximumAngle;
    [SerializeField]
    private float _minimumDistance;
    private Entity _targetObject;
    private Vector2 _targetPosition;
    private Vector2 _desiredDirection = Vector2.zero;

    private bool _stopOnEnd = true;
    private bool _decelerate = false;
    private float _decelerationRadius = 2f;

    //Constructors
    public MoveTowards(Vector2 targetPosition, bool decelerate = true, float decelerationRadius = 2, bool stopOnEnd = true): 
        this(targetPosition, 1, decelerate: decelerate, decelerationRadius: decelerationRadius) { }
    public MoveTowards(Entity target, bool decelerate = false, float decelerationRadius = -1, bool stopOnEnd = false): 
        this(target, 1, decelerate: decelerate, decelerationRadius: decelerationRadius) { }
    public MoveTowards(Vector2 targetPosition,
        float speedModifier, float accelerationModifier = 1, float trackingSpeed = 1f, 
        float maximumAngle = 360, float minimumDistance = 0.25f,
        bool decelerate = true, float decelerationRadius = 2f, bool stopOnEnd = true
    ) : this(speedModifier, accelerationModifier, trackingSpeed, maximumAngle, minimumDistance, decelerate, decelerationRadius, stopOnEnd)
    {
        _targetPosition = targetPosition;
        _targetObject = null;
    }
    public MoveTowards(Entity targetObject,
        float speedModifier, float accelerationModifier = 1, float trackingSpeed = 1f, 
        float maximumAngle = 360, float minimumDistance = 0.25f,
        bool decelerate = false, float decelerationRadius = -1, bool stopOnEnd = false
    ): this(speedModifier, accelerationModifier, trackingSpeed, maximumAngle, minimumDistance, decelerate, decelerationRadius, stopOnEnd)
    {
        _targetObject = targetObject;
        _targetPosition = _targetObject.Position;
    }
    public MoveTowards(
        float speedModifier, float accelerationModifier, float trackingSpeed, 
        float maximumAngle, float minimumDistance, bool decelerate, float decelerationRadius, bool stopOnEnd
    ) {
        _speedModifier = speedModifier;
        _accelerationModifier = accelerationModifier;
        _steeringSpeed = trackingSpeed / 10;
        _maximumAngle = maximumAngle;
        _minimumDistance = minimumDistance;
        _decelerate = decelerate;
        _decelerationRadius = decelerationRadius;
        _stopOnEnd = stopOnEnd;
    }

    //Methods
    private Vector2 GetSteeringVector(float speed, Vector2 currentPosition, Vector2 currentVelocity)
    {
        float steeringMultiplier = _steeringSpeed;
        //Calculates velocity in a straight line towards target
        Vector2 desiredVelocity = (_targetPosition - currentPosition).normalized * speed * _speedModifier;

        //Calculates angle between velocity calculated above and current, actual velocity.
        //If this value is greater than the desired maximum angle, velocity is unaltered.
        float angle = Vector2.Angle(currentVelocity.normalized, desiredVelocity.normalized);
        if(angle >= _maximumAngle)
            return currentVelocity.normalized;

        //Calculates the hypotenuse vector between the current and desired velocities, multiplied by the turning speed.
        //Returns this normalized value.
        Vector2 steeringVector = (desiredVelocity - currentVelocity) * steeringMultiplier;

        if(_decelerate) {
            float decelerationMultiplier = 1;
            float distance = Vector2.Distance(currentPosition, _targetPosition);
            if(distance <= _decelerationRadius) {
                decelerationMultiplier = Mathf.Clamp(distance - _minimumDistance, 0, distance) / _decelerationRadius;
                steeringVector *= decelerationMultiplier;
            }
            Debug.Log(distance + " vector: " + _targetPosition + " Multiplier: " + decelerationMultiplier);
        }

        return (currentVelocity + steeringVector).normalized;
    }

    #region Interface Implementation
    //Behaviour ends if distance is less than the desired distance.
    //Desired direction is calculated on Update, applied on FixedUpdate.
    public bool CheckCondition(iEnemy target) => Vector2.Distance(target.Position, _targetPosition) <= _minimumDistance; 
    public void FixedUpdate(iEnemy target) 
    {
        target.Move(_desiredDirection, target.DesiredSpeed * _speedModifier, target.CurrentAcceleration * _accelerationModifier);
    }
    public void Update(iEnemy target) 
    {
        if(_targetObject != null)
            this._targetPosition = _targetObject.Position;

        _desiredDirection = GetSteeringVector(target.DesiredSpeed, target.Position, target.CurrentVelocity);
    }
    public void OnStart(iEnemy target) { return; }
    public void OnFinish(iEnemy target) { 
        if(_stopOnEnd)
            target.Stop(); 
    }
    #endregion
}