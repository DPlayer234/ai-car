using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DPlay.AICar.Car
{
    /// <summary>
    ///     Represents a single path segment with a start and end. A line.
    /// </summary>
    public struct PathSegment
    {
        /// <summary> The start position of the segment </summary>
        public Vector3 Start;

        /// <summary> The end position of the segment </summary>
        public Vector3 End;

        /// <summary>
        ///     Initializes a new instance of the <see cref="PathSegment"/> struct.
        /// </summary>
        /// <param name="start">The start position of the segment</param>
        /// <param name="end">The end position of the segment</param>
        public PathSegment(Vector3 start, Vector3 end)
        {
            Start = start;
            End = end;
        }

        /// <summary>
        ///     The normalized vector pointing from the <seealso cref="Start"/> to the <seealso cref="End"/>.
        /// </summary>
        public Vector3 Direction => (End - Start).normalized;

        /// <summary>
        ///     The length of the segment.
        /// </summary>
        public float Length => Vector3.Distance(Start, End);

        /// <summary>
        ///     Projects the <paramref name="vector"/> onto the line defined by the segment.
        /// </summary>
        /// <param name="vector">The vector to project onto this</param>
        /// <returns>The new projected vector</returns>
        public Vector3 ProjectOnto(Vector3 vector)
        {
            Vector3 direction = Direction;
            return direction * Vector3.Dot(vector, direction);
        }

        /// <summary>
        ///     Gets the closest point to <paramref name="point"/> on the line defined by the segment.
        /// </summary>
        /// <param name="point">The reference point</param>
        /// <returns>The closest point on the line</returns>
        public Vector3 GetNormalPointTo(Vector3 point)
        {
            Vector3 offset = ProjectOnto(point - Start);
            return Start + offset;
        }

        /// <summary>
        ///     Returns the distance of a point to the segment.
        ///     This considers that points may be beyond the start or end of the segment.
        /// </summary>
        /// <param name="point">The point to get the distance to</param>
        /// <returns>The distance to the point</returns>
        public float GetDistanceTo(Vector3 point)
        {
            float length = Length;
            Vector3 nPoint = GetNormalPointTo(point);

            return Vector3.Distance(nPoint, Start) < length && Vector3.Distance(nPoint, End) < length
                ? Vector3.Distance(point, nPoint)
                : Mathf.Min(Vector3.Distance(point, Start), Vector3.Distance(point, End));
        }

        /// <summary>
        ///     Returns the distance of a point to the segment, as if the segment was infinitely extended.
        /// </summary>
        /// <param name="point">The point to get the distance to</param>
        /// <returns>The distance to the point</returns>
        public float GetDistanceToInfinite(Vector3 point)
        {
            Vector3 nPoint = GetNormalPointTo(point);
            return Vector3.Distance(point, nPoint);
        }
    }
}
