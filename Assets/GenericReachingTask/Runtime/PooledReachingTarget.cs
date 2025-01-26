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

        /// <summary>
        /// オブジェクトが非アクティブになったときに呼ばれるイベント
        /// </summary>
        public UnityEvent OnDeactivate => m_onDeactivate;
        private UnityEvent m_onDeactivate = new();

        /// <summary>
        /// オブジェクトが生成されてから自動で消えるまでの時間
        /// </summary>
        [SerializeField]
        private float m_lifeTime = float.MaxValue;
        /// <summary>
        /// オブジェクトが生成されてから自動で消えるまでの時間。
        /// デフォルトでは無限（float.MaxValue）。Initialize()の直後に設定すること。
        /// </summary>
        public float LifeTime
        {
            get => m_lifeTime;
            set => m_lifeTime = value;
        }
        private float m_currentLifeTime = 0f;
        /// <summary>
        /// オブジェクトが寿命を過ぎているかどうか
        /// </summary>
        public bool IsOutOfLife => m_lifeTime <= m_currentLifeTime;

        public void Initialize()
        {
            m_onDeactivate.RemoveAllListeners();
            m_currentLifeTime = 0f;
            m_lifeTime = float.MaxValue;
        }
        public void Deactivate()
        {
            m_onDeactivate.Invoke();
            m_pool.Release(this);
        }

        /// <summary>
        /// Updateはゲームオブジェクトが有効な場合にのみ呼ばれるため、
        /// Deactivate()後から次のInitialize()までは呼ばれない。
        /// </summary>
        void Update()
        {
            m_currentLifeTime += Time.deltaTime;
            if (IsOutOfLife)
            {
                Deactivate();
            }
        }
        void OnTriggerEnter(Collider other)
        {
            Deactivate();
        }
    }
}
