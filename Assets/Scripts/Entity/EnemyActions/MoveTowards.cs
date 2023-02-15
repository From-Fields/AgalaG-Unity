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

    //Constructors
    public MoveTowards(float speedModifier, float accelerationModifier, float trackingSpeed, float maximumAngle, float minimumDistance, Vector2 targetPosition)
    {
        _speedModifier = speedModifier;
        _accelerationModifier = accelerationModifier;
        _steeringSpeed = trackingSpeed;
        _maximumAngle = maximumAngle;
        _minimumDistance = minimumDistance;
        _targetPosition = targetPosition;
        _targetObject = null;
    }
    public MoveTowards(float speedModifier, float accelerationModifier, float trackingSpeed, float maximumAngle, float minimumDistance, Entity targetObject)
    {
        _speedModifier = speedModifier;
        _accelerationModifier = accelerationModifier;
        _steeringSpeed = trackingSpeed;
        _maximumAngle = maximumAngle;
        _minimumDistance = minimumDistance;
        _targetObject = targetObject;
        _targetPosition = _targetObject.Position;
    }

    //Methods
    private Vector2 GetSteeringVector(float speed, Vector2 currentPosition, Vector2 currentVelocity)
    {
        //Calculates velocity in a straight line towards target
        Vector2 desiredVelocity = (_targetPosition - currentPosition).normalized * speed * _speedModifier;

        //Calculates angle between velocity calculated above and current, actual velocity.
        //If this value is greater than the desired maximum angle, velocity is unaltered.
        float angle = Vector2.Angle(currentVelocity.normalized, desiredVelocity.normalized);
        if(angle >= _maximumAngle)
            return currentVelocity.normalized;

        //Calculates the hypotenuse vector between the current and desired velocities, multiplied by the turning speed.
        //Returns this normalized value.
        Vector2 steeringVector = (desiredVelocity - currentVelocity) * _steeringSpeed * Time.fixedDeltaTime;

        return (currentVelocity + steeringVector).normalized;
    }

    #region Interface Implementation
    //Behaviour ends if distance is less than the desired distance.
    //Desired direction is calculated on Update, applied on FixedUpdate.
    public bool CheckCondition(Enemy target) => Vector2.Distance(target.Position, _targetPosition) <= _minimumDistance; 
    public void FixedUpdate(Enemy target) 
    {
        target.Move(_desiredDirection, target.DesiredSpeed * _speedModifier, target._currentAcceleration * _accelerationModifier);
    }
    public void Update(Enemy target) 
    {
        if(_targetObject != null)
            this._targetPosition = _targetObject.Position;

        _desiredDirection = GetSteeringVector(target.DesiredSpeed, target.Position, target.CurrentVelocity);
    }
    public void OnStart(Enemy target) { return; }
    public void OnFinish(Enemy target) { target.Move(Vector2.zero, target.DesiredSpeed, target._currentAcceleration * 1000); }
    #endregion
}