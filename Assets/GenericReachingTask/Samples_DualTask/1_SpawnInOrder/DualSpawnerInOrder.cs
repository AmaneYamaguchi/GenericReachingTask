using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenericReachingTask.DualTask.SpawnInOrder
{
    public class DualSpawnerInOrder : MonoBehaviour
    {
        [System.Serializable]
        public class SpawnPoints
        {
            [SerializeField]
            private List<Transform> m_points;
            public List<Transform> Points => m_points;

            private int m_index;
            [SerializeField] private int m_increment = 1;
            [SerializeField] private bool m_randomizeInitialIndex = true;

            public void Initialize()
            {
                if (m_randomizeInitialIndex)
                {
                    m_index = Random.Range(0, m_points.Count);
                }
            }
            public Transform GetPoint() => m_points[m_index];
            public void Increment()
            {
                m_index = GetIncrementedValue(m_index, m_points.Count, m_increment);
            }
            public void Decrement()
            {
                m_index = GetIncrementedValue(m_index, m_points.Count, -m_increment);
            }
            /// <summary>
            /// ある値に特定の値を足しこんだ後、範囲内の数値に収める
            /// </summary>
            /// <param name="inc"></param>
            private int GetIncrementedValue(int input, int length, int inc = 1)
            {
                input += inc;

                // 範囲内に収める
                input = (int)Mathf.Repeat(input, length);

                return input;
            }
        }

        [SerializeField]
        private MultipleReachingTargetPoolManager m_poolManager;

        [SerializeField]
        private SpawnPoints m_spawnPointsFirst;
        [SerializeField]
        private SpawnPoints m_spawnPointsSecond;

        private PooledMultipleReachingTarget m_target1, m_target2;

        private void Spawn()
        {
            Vector3 pos1 = m_spawnPointsFirst.GetPoint().position;
            Vector3 pos2 = m_spawnPointsSecond.GetPoint().position;

            // オブジェクトを取り出して生成する
            if (m_poolManager.TryGet(out m_target1) && m_target1 != null
                && m_poolManager.TryGet(out m_target2) && m_target2 != null)
            {
                m_target1.transform.position = pos1;
                m_target2.transform.position = pos2;
            }
        }
        private void Start()
        {
            m_spawnPointsFirst.Initialize();
            m_spawnPointsSecond.Initialize();

            // spawn
            Spawn();
        }
        private void Update()
        {
            if (m_target1 == null || m_target2 == null) { return; }

            if (m_target1.IsReached && m_target2.IsReached)
            {
                m_target1.Deactivate();
                m_target2.Deactivate();

                m_spawnPointsFirst.Increment();
                m_spawnPointsSecond.Increment();

                Spawn();
            }
        }
    }
}
