using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerController : MonoBehaviour
{
    [SerializeField] protected float _Speed;
    [SerializeField] protected AudioClip _OnCollisionSound;
    [SerializeField] protected GameObject _OnCollisionEffectPrefab;

    protected List<Transform> _points;

    private void Start()
    {
        StartCoroutine(Coroutine_Movement());
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        StopAllCoroutines();
    //        transform.position = Vector3.zero;
    //        StartCoroutine(Coroutine_Movement());
    //    }
    //}

    protected abstract IEnumerator Coroutine_Movement();

    protected void OnFinish()
    {
        StopAllCoroutines();
        AudioController.Instance.PlayOneShot(_OnCollisionSound, 1);
        Instantiate(_OnCollisionEffectPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public void AssignPoints(List<Transform> points)
    {
        _points = points;
    }
}
