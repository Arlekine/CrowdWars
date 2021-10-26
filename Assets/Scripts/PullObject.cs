using System;
using System.Collections;
using UnityEngine;

public class PullObject : MonoBehaviour
{
    public Action<PullObject> onComplete;

    public void Init(float lifeTime)
    {
        StartCoroutine(LifeRoutine(lifeTime));
    }

    private IEnumerator LifeRoutine(float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        onComplete?.Invoke(this);
    }
}