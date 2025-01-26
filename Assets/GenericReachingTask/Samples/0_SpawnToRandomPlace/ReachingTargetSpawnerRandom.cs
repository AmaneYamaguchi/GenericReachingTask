using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenericReachingTask.SpawnToRandomPlace
{
    /// <summary>
    /// <see cref="Transform.position"/>を中心とする立方体の中にリーチングターゲットを生成する。
    /// </summary>
    public class ReachingTargetSpawnerRandom : MonoBehaviour
    {
        [SerializeField]
        private ReachingTargetPoolManager m_poolManager;
        /// <summary>
        /// リーチングターゲットを生成する範囲。
        /// 立方体の辺の長さの半分（m）。
        /// </summary>
        [SerializeField]
        [Tooltip("リーチングターゲットを生成する範囲。\n立方体の辺の長さの半分（m）。")]
        private float m_spawnRange = 1f;
        private int m_spawnCount = 0;
        /// <summary>
        /// リーチングターゲットが自動で消えるまでの時間（秒）。
        /// </summary>
        [SerializeField]
        [Tooltip("リーチングターゲットが自動で消えるまでの時間（秒）。\n無限にしたい場合はコメントアウトしてください。")]
        private float m_lifeTime = 10f;

        private void Spawn()
        {
            Vector3 localPos = new Vector3
            {
                x = Random.Range(-m_spawnRange, m_spawnRange),
                y = Random.Range(-m_spawnRange, m_spawnRange),
                z = Random.Range(-m_spawnRange, m_spawnRange)
            };
            Vector3 globalPos = transform.position + localPos;

            // get and spawn pooled object
            if (m_poolManager.TryGet(out var obj) && obj != null)
            {
                obj.LifeTime = m_lifeTime;
                obj.transform.position = globalPos;
                m_spawnCount++;
                obj.OnDeactivate.AddListener(() =>
                {
                    m_spawnCount--;
                });
            }
            else
            {
                Debug.LogError($"ReachingTargetSpawnRandom.Spawn(): Could not get pooled object!");
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
        private int m_windowId = 10000;
        private Rect m_windowRect = new Rect(0, 0, 200, 100);
        [SerializeField] private bool m_drawGUI = true;
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
