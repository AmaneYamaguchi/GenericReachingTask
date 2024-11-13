using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

namespace GenericReachingTask
{
    public class PooledReachingTarget : MonoBehaviour, IPooledObject<PooledReachingTarget>
    {
        private IObjectPool<PooledReachingTarget> m_pool;
        public IObjectPool<PooledReachingTarget> ObjectPool
        {
            set => m_pool = value;
        }

        public UnityEvent OnDeactivate => m_onDeactivate;
        private UnityEvent m_onDeactivate = new();

        public void Initialize()
        {
            m_onDeactivate.RemoveAllListeners();
        }
        public void Deactivate()
        {
            m_onDeactivate.Invoke();
            m_pool.Release(this);
        }

        void OnTriggerEnter(Collider other)
        {
            Deactivate();
        }
    }
}
