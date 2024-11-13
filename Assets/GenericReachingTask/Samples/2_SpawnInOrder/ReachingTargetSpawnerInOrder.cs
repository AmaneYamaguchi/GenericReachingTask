using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenericReachingTask.SpawnInOrder
{
    public class ReachingTargetSpawnerInOrder : MonoBehaviour
    {
        [SerializeField]
        private ReachingTargetPoolManager m_poolMangaer;
        [SerializeField]
        private List<Transform> m_spawnPoints;
        private int m_index = 0;
        [SerializeField]
        private int m_increment = 1;

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
        private void Spawn()
        {
            Vector3 pos = m_spawnPoints[m_index].position;

            if (m_poolMangaer.TryGet(out var obj) &&  obj != null)
            {
                obj.transform.position = pos;
                obj.OnDeactivate.AddListener(() =>
                {
                    Debug.Log($"Reached {m_index}th Object");
                    m_index = GetIncrementedValue(m_index, m_spawnPoints.Count, m_increment);
                    Spawn();
                });
            }
            else
            {
                Debug.LogError("ReachingTargetSpawnerInOrder.Spawn(): Could not get pooled object!");
            }
        }
        private void Start()
        {
            m_index = 0;
            Spawn();
        }
    }
}
