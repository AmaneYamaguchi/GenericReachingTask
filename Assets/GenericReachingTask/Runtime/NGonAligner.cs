using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenericReachingTask
{
    /// <summary>
    /// 正N角形を綺麗に作るクラス
    /// </summary>
#if UNITY_EDITOR
    [ExecuteAlways]
#endif
    public class NGonAligner : MonoBehaviour
    {
        /// <summary>
        /// 正N角形が接する円の半径
        /// </summary>
        [SerializeField]
        private float m_radius = 0.5f;

        private void Update()
        {
            // 頂点数
            int vertexCount = transform.childCount;
            float angle = 2f * Mathf.PI / vertexCount;

            for (int i = 0; i< vertexCount; i++)
            {
                Transform t = transform.GetChild(i);
                t.localPosition = m_radius * new Vector3
                {
                    x = Mathf.Sin(angle * i),
                    y = Mathf.Cos(angle * i),
                    z = 0f
                };
            }
        }
    }
}
