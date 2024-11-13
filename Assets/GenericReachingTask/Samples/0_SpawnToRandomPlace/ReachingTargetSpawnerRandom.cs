using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenericReachingTask.SpawnToRandomPlace
{
    /// <summary>
    /// <see cref="Transform.position"/>�𒆐S�Ƃ��闧���̂̒��Ƀ��[�`���O�^�[�Q�b�g�𐶐�����B
    /// </summary>
    public class ReachingTargetSpawnerRandom : MonoBehaviour
    {
        [SerializeField]
        private ReachingTargetPoolManager m_poolManager;
        /// <summary>
        /// ���[�`���O�^�[�Q�b�g�𐶐�����͈́B
        /// �����̂̕ӂ̒����̔����im�j�B
        /// </summary>
        [SerializeField]
        [Tooltip("���[�`���O�^�[�Q�b�g�𐶐�����͈́B\n�����̂̕ӂ̒����̔����im�j�B")]
        private float m_spawnRange = 1f;
        private int m_spawnCount = 0;

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
