using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

namespace GenericReachingTask
{
    [System.Obsolete("Use PooledReachingTarget instead.")]
    public class PooledReachingTargetMortal : MonoBehaviour, IPooledObject<PooledReachingTargetMortal>
    {
        private IObjectPool<PooledReachingTargetMortal> m_pool;
        public IObjectPool<PooledReachingTargetMortal> ObjectPool
        {
            set => m_pool = value;
        }

        public UnityEvent OnDeactivate => m_onDeactivate;
        private UnityEvent m_onDeactivate = new();

        /// <summary>
        /// オブジェクトが生成されてから自動で消えるまでの時間
        /// </summary>
        [SerializeField]
        private float m_lifeTime = 3f;
        private float m_currentLifeTime = 0f;
        /// <summary>
        /// オブジェクトが寿命を過ぎているかどうか
        /// </summary>
        public bool IsOutOfLife => m_lifeTime <= m_currentLifeTime;

        public void Initialize()
        {
            m_onDeactivate.RemoveAllListeners();
            m_currentLifeTime = 0f;
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
