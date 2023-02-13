using EzGraphics;
using Microsoft.Xna.Framework;

namespace PrimitivesTests
{
    public class Vision
    {
        private float visionAngle;
        private float range;
        private float rangeSqrd;
        private float rotation;
        private Vector2 center;

        private Vector2 edge1;
        private Vector2 edge2;

        /// <summary>
        /// Field of view in degrees
        /// </summary>
        public float FieldOfView
        {
            get { return MathHelper.ToDegrees(visionAngle*2); }
            set
            {
                visionAngle = MathHelper.ToRadians(value / 2);
                RecomputeEdges();
            }
        }
        /// <summary>
        /// Used to set the range
        /// </summary>
        public float Range
        {
            get { return range; }
            set
            {
                range = value;
                rangeSqrd = range * range;
            }
        }
        /// <summary>
        /// Set the rotation in radians
        /// </summary>
        public float Rotation
        {
            get { return rotation; }
            set
            {
                rotation = MathHelper.WrapAngle(value);
                RecomputeEdges();
            }
        }

        public Vector2 Center
        {
            get { return center; }
            set
            {
                center = value;
                //dirty = true;
            }
        }
        /// <summary>
        /// Create a new vision with max range and fieldOfView in degrees
        /// </summary>
        /// <param name="range"></param>
        /// <param name="fieldOfView">In degrees</param>
        public Vision(float range, float fieldOfView)
        {
            Range = range;
            FieldOfView = fieldOfView;
            Center = Vector2.Zero;
        }

        private void RecomputeEdges()
        {
            edge1 = Vector2Utils.GetUnitVector(rotation + visionAngle);
            edge2 = Vector2Utils.GetUnitVector(rotation - visionAngle);
        }
        /// <summary>
        /// Checks if the supplied point is in the vision
        /// </summary>
        /// <param name="point">the point to check (in world space)</param>
        /// <returns>true if in vision, else false</returns>
        public bool InVision(Vector2 point)
        {
            Vector2 relPos = point - center;
            float lengthSqrd = relPos.LengthSquared();
            if (lengthSqrd > rangeSqrd) return false;
            if (visionAngle > MathHelper.PiOver2)
            {
                return InWideView(relPos, edge1, edge2);
            }
            else
            {
                return InNarrowView(relPos, edge1, edge2);
            }
        }

        /// <summary>
        /// ***In progress*** *Note that vision over 180 degrees result in a concave polygon unsuited for a hull
        /// </summary>
        /// <param name="resolution"></param>
        /// <returns>null</returns>
        public Vector2[] ToPolygon(int resolution)
        {
            return null;
        }


        /// <summary>
        /// Draws outline of the vision area on screen
        /// </summary>
        /// <param name="shapeBatch"></param>
        /// <param name="color"></param>
        /// <param name="lineWidth">thickness of the lines, default is 1</param>
        /// <param name="resolution">resolution of the circle arc, default 48, use higher for smother  when range is big</param>
        public void Draw(ShapeBatch shapeBatch, Color color, float lineWidth = 1, int resolution = 48)
        {
            shapeBatch.DrawArc(center, range, rotation - visionAngle, visionAngle + visionAngle, color, lineWidth, resolution);
            shapeBatch.DrawLine(center, center + edge1 * range, color, lineWidth);
            shapeBatch.DrawLine(center, center + edge2 * range, color, lineWidth);
        }

        /*
         * viewDir = v, center to point relPos = u, edge vectors(?) = e1,e2
         * 
         * If u.v the point is behind the viewer.
         * else if u.u = ||u|| sqrd > r sqrd the point is beyond viewing distance.
         * else if sign(e1 x u) = sign( u x e2 ) then the point is within the sector.
         * else it is in the bounding semicircle but outside the sector.
         */
        public static bool InNarrowView(Vector2 relPos, Vector2 edge1, Vector2 edge2)
        {
            //if (Vector2.Dot(viewDir, relPos) <= 0.0f) return false;
            //if (relPos.LengthSquared() > radiusSqrd) return false;
            if (Vector2Utils.PseudoCross(edge1, relPos) <= 0 && Vector2Utils.PseudoCross(relPos, edge2) <= 0) return false;
            return true;
        }
        //some changes if field of view is greater than PI
        public static bool InWideView(Vector2 relPos, Vector2 edge1, Vector2 edge2)
        {
            if (Vector2Utils.PseudoCross(edge1, relPos) > 0 || Vector2Utils.PseudoCross(relPos, edge2) > 0) return true;
            return false;
        }
    }
}
