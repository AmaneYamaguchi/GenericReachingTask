using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

namespace GenericReachingTask
{
    public interface IPooledObject<T> where T : class
    {
        IObjectPool<T> ObjectPool { set; }
        void Initialize();
        void Deactivate();
        UnityEvent OnDeactivate { get; }
    }
}
