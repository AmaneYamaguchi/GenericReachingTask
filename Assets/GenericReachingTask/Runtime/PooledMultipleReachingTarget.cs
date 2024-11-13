using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

namespace GenericReachingTask
{
    public class PooledMultipleReachingTarget : MonoBehaviour, IPooledObject<PooledMultipleReachingTarget>
    {
        private IObjectPool<PooledMultipleReachingTarget> m_pool;
        public IObjectPool<PooledMultipleReachingTarget> ObjectPool
        {
            set => m_pool = value;
        }

        /// <summary>
        /// このオブジェクトにリーチしている<see cref="Rigidbody"/>
        /// </summary>
        private HashSet<Collider> m_reachingObjects = new(64);
        /// <summary>
        /// リーチされているかどうか
        /// </summary>
        public bool IsReached => m_reachingObjects.Count > 0;

        public UnityEvent OnDeactivate => m_onDeactivate;
        private UnityEvent m_onDeactivate = new();

        public void Initialize()
        {
            m_reachingObjects.Clear();
            m_onDeactivate.RemoveAllListeners();
        }
        public void Deactivate()
        {
            m_onDeactivate.Invoke();
            m_pool.Release(this);
        }

        void OnTriggerEnter(Collider other)
        {
            m_reachingObjects.Add(other);
        }
        void OnTriggerExit(Collider other)
        {
            m_reachingObjects.Remove(other);
        }
    }
}
