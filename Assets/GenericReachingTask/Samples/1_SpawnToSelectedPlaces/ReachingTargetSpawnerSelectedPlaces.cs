
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GenericReachingTask.SpawnToSelectedPlaces
{
    public class ReachingTargetSpawnerSelectedPlaces : MonoBehaviour
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
        private ReachingTargetPoolManager m_poolManager;
        [SerializeField]
        private List<SpawnPoint> m_targetPoints;
        private int m_spawnCount;

        private void Spawn()
        {
            // まだオブジェクトが生成されていない場所をランダムに選ぶ
            var validPoints = m_targetPoints.Where(x => !x.HasObject).ToList();
            var index = Random.Range(0, validPoints.Count());
            var validPoint = validPoints[index];

            // オブジェクトを取り出して生成する
            if (m_poolManager.TryGet(out var obj) && obj != null)
            {
                obj.transform.position = validPoint.Point.position;
                m_spawnCount++;
                validPoint.HasObject = true;
                obj.OnDeactivate.AddListener(() =>
                {
                    m_spawnCount--;
                    validPoint.HasObject = false;
                });
            }
            else
            {
                Debug.LogError($"ReachingTargetSpawnSelectedPlaces.Spawn(): Could not get pooled object!");
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Spawn();
            }
        }

        // IMGUI
        private int m_windowId = 10001;
        private Rect m_windowRect = new Rect(0, 0, 200, 100);
        [SerializeField]
        private bool m_drawGUI = true;
        private void OnGUI()
        {
            if (!m_drawGUI) { return; }

            m_windowRect = GUI.Window(m_windowId, m_windowRect, (id) =>
            {
                GUILayout.Label($"Spawn Count: {m_spawnCount}");
                if (GUILayout.Button("Spawn (Space)"))
                {
                    Spawn();
                }
                GUI.DragWindow();
            }, name);
        }
    }
}
