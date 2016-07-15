using UnityEngine;

public class DrunkMode : MonoBehaviour
{
    private static readonly float MAXIMUM_ANGULAR_VELOCITY_MAGNITUDE = 5.0f;
    private static readonly float ANGULAR_ACCELERATION_MAGNITUDE = 2.0f;
    private static readonly float RANGE_FOR_NEXT_PICK = 5.0f;
    private static readonly Vector3 RANGE_FOR_RANDOM_ANGLE = new Vector3(3.0f, 2.0f, 10.0f);

    private Vector3 _currentRotation = Vector3.zero;
    private Vector3 _targetRotation = Vector3.zero;
    private Vector3 _angularVelocity = Vector3.zero;
    private Quaternion _previousOrientation = Quaternion.identity;

    private void Start()
    {

    }

    private void Update()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            Quaternion currentOrientation = mainCamera.transform.localRotation;
            if (currentOrientation == _previousOrientation && _previousOrientation != Quaternion.identity)
            {
                return;
            }

            Animate(Time.deltaTime);
            mainCamera.transform.Rotate(_currentRotation);
            _previousOrientation = mainCamera.transform.localRotation;
        }
    }

    private void Animate(float elapsed)
    {
        Vector3 targetVector = _targetRotation - _currentRotation;
        targetVector.Normalize();
        targetVector *= ANGULAR_ACCELERATION_MAGNITUDE;

        Vector3 angularAccelerationFrame = targetVector * Time.deltaTime;
        _angularVelocity += angularAccelerationFrame;
        if (_angularVelocity.magnitude > MAXIMUM_ANGULAR_VELOCITY_MAGNITUDE)
        {
            _angularVelocity.Normalize();
            _angularVelocity *= MAXIMUM_ANGULAR_VELOCITY_MAGNITUDE;
        }

        Vector3 angularVelocityFrame = _angularVelocity * Time.deltaTime;
        _currentRotation += angularVelocityFrame;

        if ((_targetRotation - _currentRotation).magnitude <= RANGE_FOR_NEXT_PICK)
        {
            PickNextTargetRotation();
        }
    }

    private void PickNextTargetRotation()
    {
        _targetRotation.x = Random.Range(-RANGE_FOR_RANDOM_ANGLE.x, RANGE_FOR_RANDOM_ANGLE.x);
        _targetRotation.y = Random.Range(-RANGE_FOR_RANDOM_ANGLE.y, RANGE_FOR_RANDOM_ANGLE.y);
        _targetRotation.z = Random.Range(-RANGE_FOR_RANDOM_ANGLE.z, RANGE_FOR_RANDOM_ANGLE.z);
    }
}
