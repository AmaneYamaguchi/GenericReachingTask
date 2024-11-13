using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GenericReachingTask.DualTask.SpawnToRandomPlaces
{
    public class DualSpawnerSelectedPlaces : MonoBehaviour
    {
        [System.Serializable]
        public class SpawnPoint
        {
            public Transform Point => m_point;
            [SerializeField] private Transform m_point;

            /// <summary>
            /// このSpawnPointにオブジェクトが生成されているかどうか。
            /// 生成されているなら確率抽選からはじく。
            /// </summary>
            public bool HasObject { get; set; } = false;

            public SpawnPoint(Transform t)
            {
                m_point = t;
                HasObject = false;
            }
        }

        [SerializeField]
        private MultipleReachingTargetPoolManager m_poolManager;
        [SerializeField]
        private List<SpawnPoint> m_targetPointsFirst;
        [SerializeField]
        private List<SpawnPoint> m_targetPointsSecond;

        private PooledMultipleReachingTarget m_target1, m_target2;
        
        private void Spawn()
        {
            // まだオブジェクトが生成されていない場所をランダムに選ぶ
            var validPointsFirst = m_targetPointsFirst.Where(x => !x.HasObject).ToList();
            var indexFirst = Random.Range(0, validPointsFirst.Count());
            var validPointFirst = validPointsFirst[indexFirst];

            var validPointsSecond = m_targetPointsSecond.Where(x => !x.HasObject).ToList();
            var indexSecond = Random.Range(0, validPointsSecond.Count());
            var validPointSecond = validPointsSecond[indexSecond];

            // オブジェクトを取り出して生成する
            if (m_poolManager.TryGet(out m_target1) && m_target1 != null
                && m_poolManager.TryGet(out m_target2) && m_target2 != null)
            {
                m_target1.transform.position = validPointFirst.Point.position;
                validPointFirst.HasObject = true;

                m_target2.transform.position = validPointSecond.Point.position;
                validPointSecond.HasObject = true;

                m_target1.OnDeactivate.AddListener(() =>
                {
                    validPointFirst.HasObject = false;
                });
                m_target2.OnDeactivate.AddListener(() =>
                {
                    validPointSecond.HasObject = false;
                });
            }
        }
        private void Start()
        {
            Spawn();
        }
        private void Update()
        {
            if (m_target1 == null || m_target2 == null) { return; }

            if (m_target1.IsReached && m_target2.IsReached)
            {
                m_target1.Deactivate();
                m_target2.Deactivate();

                Spawn();
            }
        }
    }
}
