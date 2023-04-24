using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerPrediction : PlayerController
{
    protected override IEnumerator Coroutine_Movement()
    {
        for (int i = 0; i < _points.Count; i++)
        {
            float delta = _Speed * Time.deltaTime;
            Vector3 directionVector = (_points[i].position - transform.position);
            transform.forward = directionVector;
            float distance = (_points[i].position - transform.position).magnitude;
            RaycastResult collisionHit = RaycastForCollision(transform.position, directionVector, distance, delta);

            while (delta < distance && !collisionHit.Success)
            {
                Vector3 newPos = Vector3.MoveTowards(transform.position, _points[i].position, delta);
                directionVector = _points[i].position - newPos;
                distance = directionVector.magnitude;
                transform.forward = directionVector;
                transform.position = newPos;

                yield return null;

                delta = _Speed * Time.deltaTime;
                collisionHit = RaycastForCollision(newPos, directionVector, distance, delta);
            }
            if (collisionHit.Success)
            {
                transform.position = collisionHit.HitPoint;
                OnFinish();
                break;
            }

            transform.position = _points[i].position;
        }
        OnFinish();
    }

    struct RaycastResult
    {
        public bool Success;
        public Vector3 HitPoint;
    }
    private RaycastResult RaycastForCollision(Vector3 source, Vector3 direction, float maxDistance, float delta)
    {
        Ray ray = new Ray(source, direction);
        Vector3 point0 = source - Vector3.up * transform.localScale.y;
        Vector3 point1 = source + Vector3.up * transform.localScale.y;

        Collider[] overlapColliders = Physics.OverlapCapsule(point0, point1, transform.localScale.x / 2f);
        for (int i = 0; i < overlapColliders.Length; i++)
        {
            if (overlapColliders[i].gameObject != gameObject)
            {
                return new RaycastResult() { Success = true, HitPoint = transform.position };
            }
        }
        RaycastHit[] hits = Physics.CapsuleCastAll(point0,point1, transform.localScale.x / 2f,direction.normalized, maxDistance);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.gameObject != gameObject)
            {
                Vector3 hitDirectionVector = (hits[i].point - transform.position);
                if (hits[i].collider.gameObject != gameObject && hitDirectionVector.magnitude < delta)
                {
                    if (hits[i].distance == 0)
                    {
                        hits[i].point = hits[i].collider.transform.position;
                    }
                    return new RaycastResult() { Success = true, HitPoint = hits[i].point };
                }
            }
        }

        return new RaycastResult() { Success = false };
    }
}
