using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMovement : MonoBehaviour
{
    [SerializeField] Transform _targetTransform;
    public Transform TargetTransform
    {
        get => _targetTransform;
        set => _targetTransform = value;
    }
    [SerializeField] GameObject cardFront;
    [SerializeField] GameObject cardBack;

    private float _positionDamp = .2f;
    private float _rotationDamp = .2f;

    private Vector3 _smoothVelocity;
    private Vector4 _smoothRotationVelocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_targetTransform != null)
        {
            SmoothToTargetPositionRotation(_targetTransform);
        }

        TestVisibility();
    }

    private void SmoothToTargetPositionRotation(Transform target)
    {
        if (target.position != transform.position || target.eulerAngles != _targetTransform.eulerAngles)
        {
            SmoothToPointAndDirection(target.position, _positionDamp, target.rotation, _rotationDamp);
        }
    }

    private void SmoothToPointAndDirection(Vector3 point, float moveSmooth, Quaternion rotation, float rotSmooth)
    {
        transform.position = Vector3.SmoothDamp(transform.position, point, ref _smoothVelocity, moveSmooth);

        Quaternion newRotation;
        newRotation.x = Mathf.SmoothDamp(transform.rotation.x, rotation.x, ref _smoothRotationVelocity.x, rotSmooth);
        newRotation.y = Mathf.SmoothDamp(transform.rotation.y, rotation.y, ref _smoothRotationVelocity.y, rotSmooth);
        newRotation.z = Mathf.SmoothDamp(transform.rotation.z, rotation.z, ref _smoothRotationVelocity.z, rotSmooth);
        newRotation.w = Mathf.SmoothDamp(transform.rotation.w, rotation.w, ref _smoothRotationVelocity.w, rotSmooth);
        transform.rotation = newRotation;
    }

    private void TestVisibility()
    {
        float angle = Vector3.Angle(Camera.main.transform.forward, transform.forward);

        if (angle < 90)
        {
            FrontBecameVisible();
        }
        else
        {
            FrontBecameHidden();
        }
    }

    private void FrontBecameVisible()
    {
        cardFront.SetActive(true);
        cardBack.SetActive(false);
    }

    private void FrontBecameHidden()
    {
        cardFront.SetActive(false);
        cardBack.SetActive(true);
    }
}
