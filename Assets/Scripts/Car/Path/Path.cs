using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DPlay.AICar.Car
{
    /// <summary>
    ///     A configurable Path.
    ///     Add Child-GameObjects as path nodes. they are connected in the order as seen.
    ///     The last element is connected to the first one.
    /// </summary>
    public class Path : MonoBehaviour
    {
        /// <summary> Click in the editor to trigger OnValidate and update the displayed path </summary>
        public bool Update;

        /// <summary> The "radius" (width) of the path </summary>
        public float Radius = 0.3f;

        /// <summary> Internal list of node positions </summary>
        private List<Vector3> points;

        /// <summary> Internal list of path segments </summary>
        private List<PathSegment> segments;

        /// <summary> All positions of nodes/Points </summary>
        public IReadOnlyList<Vector3> Points => points;

        /// <summary> All path segments </summary>
        public IReadOnlyList<PathSegment> Segments => segments;

        /// <summary>
        ///     Returns the segment closest to the given position.
        /// </summary>
        /// <param name="position">The position to get the closest the segment to</param>
        /// <returns>The closest segment</returns>
        public PathSegment GetClosestSegment(Vector3 position)
        {
            int segmentIndex = 0;
            float shortestDist = Mathf.Infinity;

            for (int i = 0; i < Segments.Count; i++)
            {
                PathSegment segment = Segments[i];

                float dist = segment.GetDistanceTo(position);

                if (dist <= shortestDist)
                {
                    segmentIndex = i;
                    shortestDist = dist;
                }
            }

            return Segments[segmentIndex];
        }

        /// <summary>
        ///     Computes <seealso cref="points"/>.
        /// </summary>
        private void ComputePoints()
        {
            points = new List<Vector3>(transform.childCount);

            for (int i = 0; i < transform.childCount; i++)
            {
                points.Add(transform.GetChild(i).position);
            }
        }

        /// <summary>
        ///     Computes <seealso cref="segments"/>.
        /// </summary>
        private void ComputeSegments()
        {
            ComputePoints();

            segments = new List<PathSegment>(Points.Count);

            if (Points.Count == 0) return;

            for (int i = 0; i < Points.Count - 1; i++)
            {
                segments.Add(new PathSegment(Points[i], Points[i + 1]));
            }

            segments.Add(new PathSegment(Points[Points.Count - 1], Points[0]));
        }

        /// <summary>
        ///     Called by Unity to initialize the <see cref="Path"/>.
        /// </summary>
        private void Awake()
        {
            ComputeSegments();
        }

        /// <summary>
        ///     Called by Unity when this element is edited in the inspector.
        /// </summary>
        private void OnValidate()
        {
            ComputeSegments();
            Update = false;
        }

        /// <summary>
        ///     Called by Unity to Gizmos in the editor.
        /// </summary>
        private void OnDrawGizmos()
        {
            if (Segments == null) return;

            Gizmos.color = Color.red;

            foreach (PathSegment segment in Segments)
            {
                Gizmos.DrawLine(segment.Start, segment.End);
            }
        }
    }
}
