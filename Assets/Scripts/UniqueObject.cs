using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueObject : MonoBehaviour
{
    private static UniqueObject _inst;
    
    private void Start () 
    {
        if (_inst == null) 
        {
            _inst = this;

            DontDestroyOnLoad(gameObject);
        } 
        else if(_inst != this)
        {
            Destroy(gameObject);
        }
    }
}
