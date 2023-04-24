using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class PlayerControllerRigidbody : PlayerController
{
    private Rigidbody _rb;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    protected override IEnumerator Coroutine_Movement()
    {
        for (int i = 0; i < _points.Count; i++)
        {
            float delta = _Speed * Time.deltaTime;
            Vector3 directionVector = (_points[i].position - _rb.position);
            transform.forward = directionVector;
            float distance = (_points[i].position - _rb.position).magnitude;
            while (delta < distance)
            {
                Vector3 newPos = Vector3.MoveTowards(_rb.position, _points[i].position, delta);
                directionVector = _points[i].position - newPos;
                distance = directionVector.magnitude;
                transform.forward = directionVector;
                _rb.MovePosition(newPos);
                
                yield return null;

                delta = _Speed * Time.deltaTime;
            }
            _rb.position=_points[i].position;
        }
        OnFinish();
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnFinish();
    }
}
