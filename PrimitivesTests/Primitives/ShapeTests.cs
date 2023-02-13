using Microsoft.Xna.Framework;
using SharpDX.Direct3D9;
using System;

namespace PrimitivesTests.Primitives
{
    public static class ShapeTests
    {
        
        /// <summary>
        /// Can so far only do Circle vs Any, Hull vs Hull and AABB vs AABB, not OBB vs OBB/Hull/AABB or AABB vs Hull. Capsule not implemented at all..
        /// </summary>
        /// <param name="s1">First shape</param>
        /// <param name="s2">Second shape</param>
        /// <returns>true if the shapes overlap, otherwise false</returns>
        public static bool Overlap(Shape s1, Shape s2)
        {
            return (s1.Type, s2.Type)
            switch
            {
                (ShapeType.Circle, ShapeType.Circle) => CircleOverlap((Circle)s1, (Circle)s2),
                (ShapeType.Circle, ShapeType.OBB) => CircleOBBOverlap((Circle)s1, (OBB)s2),
                (ShapeType.OBB, ShapeType.Circle) => CircleOBBOverlap((Circle)s2, (OBB)s1),
                (ShapeType.Circle, ShapeType.AABB) => CircleAABBOverlap((Circle)s1, (AABB)s2),
                (ShapeType.AABB, ShapeType.Circle) => CircleAABBOverlap((Circle)s2, (AABB)s1),
                (ShapeType.AABB, ShapeType.AABB) => AABBAABBOverlap((AABB)s1, (AABB)s2),
                (ShapeType.Hull, ShapeType.Hull) => HullHullOverlapSAT((Hull)s1, (Hull)s2),
                (ShapeType.Circle, ShapeType.Hull) => CircleHullOverlapSAT((Circle)s1, (Hull)s2),
                (ShapeType.Hull, ShapeType.Circle) => CircleHullOverlapSAT((Circle)s2, (Hull)s1),
                _ => false,
            };
        }

        /// <summary>
        /// Only does Circle and Hull overlapps using Separatig Axis Test, no other shapes will be tested
        /// </summary>
        /// <param name="s1">First Circle or Hull</param>
        /// <param name="s2">Second Circle or Hull</param>
        /// <returns>true if the shapes overlap, otherwise false. Shapes other than Hull/Circle will return false</returns>
        public static bool OverlapSAT(Shape s1, Shape s2)
        {
            return (s1.Type, s2.Type)
            switch
            {
                (ShapeType.Circle, ShapeType.Circle) => CircleOverlap((Circle)s1, (Circle)s2),
                (ShapeType.Hull, ShapeType.Hull) => HullHullOverlapSAT((Hull)s1, (Hull)s2),
                (ShapeType.Circle, ShapeType.Hull) => CircleHullOverlapSAT((Circle)s1, (Hull)s2),
                (ShapeType.Hull, ShapeType.Circle) => CircleHullOverlapSAT((Circle)s2, (Hull)s1),
                _ => false,
            };
        }

        public static bool AABBAABBOverlap(AABB a1, AABB a2)
        {
            Vector2 dist = a2.center - a1.center;
            if (Math.Abs(dist.X) > a1.extents.X + a2.extents.X) return false;
            if (Math.Abs(dist.Y) > a1.extents.Y + a2.extents.Y) return false;
            return true;
        }

        public static bool CircleOBBOverlap(Circle circle, OBB obb)
        {
            Vector2 closestPoint = obb.ClosestPoint(circle.center);
            float dist = Vector2.DistanceSquared(closestPoint, circle.center);
            return dist <= circle.radius * circle.radius;
        }

        public static bool CircleOverlap(Circle circle1, Circle circle2)
        {
            float dist = Vector2.DistanceSquared(circle1.center, circle2.center);
            float r = circle1.radius + circle2.radius;
            return dist <= r * r;
        }

        public static bool CircleAABBOverlap(Circle circle, AABB aabb)
        {
            Vector2 closestPoint = aabb.ClosestPoint(circle.center);
            float dist = Vector2.DistanceSquared(closestPoint, circle.center);
            return dist <= circle.radius * circle.radius;
        }
        /// <summary>
        /// Method for testing Circle and Hull for overlapp/intersection ***CURRENTLY BUGGED*** -> circle fully inside hull is not detected!
        /// </summary>
        /// <param name="circle">Circle to test</param>
        /// <param name="hull">Hull to test against</param>
        /// <returns>true if the circle and hull are overlapping, otherwise false</returns>
        public static bool CircleHullOverlap(Circle circle, Hull hull)
        {
            float sqRadius = circle.radius * circle.radius;
            Vector2[] points = hull.GetWorldCoords();
            Vector2 prev = points[^1];
            float min = float.MaxValue;
            for (int i = 0; i < points.Length; i++)
            {
                Vector2 curr = points[i];
                float sqDist = PrimitivesTest.SqDistPointSegment(circle.center, prev, curr);
                if(sqDist <= sqRadius)
                {
                    if (min > sqDist)
                    {
                        min = sqDist;
                        //Like with hull vs hull saving best contact and face normal is good for resolving collisions
                    }
                }
                prev = curr;
            }
            if (min < sqRadius)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Method for testing Circle and Hull for overlapp using Separating Axis Test
        /// </summary>
        /// <param name="circle">Circle to test</param>
        /// <param name="hull">Hull to test against</param>
        /// <returns>true if the circle and hull are overlapping, otherwise false</returns>
        public static bool CircleHullOverlapSAT(Circle circle, Hull hull)
        {
            // first check axis from Circle point of view
            Vector2 axis = circle.center - hull.Center;
            float min = ProjectCircle(circle.center, circle.radius, axis); //negate the radius as the support points are calculated backwards
            hull.SupportPoint(axis, out float max);
            //DebugDraw.DrawCircle(hull.SupportPoint(axis, out _), 2, Color.Blue);
            float distance = min - max; //signed distance, negative means overlapping on this axis
            if (distance > 0) return false;



            // then check axis from Hull/Polygon faces
            Vector2[] points = hull.GetWorldCoords();
            Vector2 prev = points[^1];
            for (int i = 0; i < points.Length; i++)
            {
                Vector2 curr = points[i];
                Vector2 faceNormal = GetFaceNormal(prev, curr); //not normalized just the direction 
                //DebugDraw.DrawLine((prev+curr)/2, (prev + curr) / 2 + Vector2.Normalize(faceNormal) * 10);

                max = ProjectCircle(circle.center, circle.radius, faceNormal); //like get support point but for circle (and not the actual point)

                min = Vector2.Dot(faceNormal, curr); //the distance along the axis of current face
                distance = min - max; //signed distance, positive means overlapping on this axis *Opposite Hull vs Hull because outward facing normals!

                if (distance < 0) return false; // if negative we can exit early because we found a separating axis! -> no collision

                prev = curr;
            }
            return true;
        }

        private static float ProjectCircle(Vector2 center, float radius, Vector2 axis)
        {
            Vector2 direction = Vector2.Normalize(axis);
            //DebugDraw.DrawCircle(center - direction * radius, 3, Color.Red);
            return Vector2.Dot(axis, center - direction * radius);
        }

        /// <summary>
        /// Checks two Hulls against eachother using Separating Axis Test
        /// </summary>
        /// <param name="hull1"></param>
        /// <param name="hull2"></param>
        /// <returns>True if the hulls are overlapping, otherwise false</returns>
        public static bool HullHullOverlapSAT(Hull hull1, Hull hull2)
        {
            //check bounding boxes first
            //..code..

            //chech hull1 vs hull2 then hull2 vs hull1
            if (FaceQuery(hull1, hull2) > 0.0f)
            {
                return false;
            }
            if (FaceQuery(hull2, hull1) > 0.0f)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Queries all faces of hull1 and checks if hull2 is overlapping on that face normal axis 
        /// </summary>
        /// <param name="hull1">Hull with tha faces</param>
        /// <param name="hull2">Hull to check against</param>
        /// <returns></returns>
        private static float FaceQuery(Hull hull1, Hull hull2)
        {
            Vector2[] points = hull1.GetWorldCoords();
            Vector2 prev = points[^1]; // handy to get the last vertex as previos before the loot starts
            float bestDistance = float.MinValue;
            for (int i = 0; i < points.Length; i++)
            {
                Vector2 curr = points[i];
                //Two points next to eachother on a polygon makes up a face (prev and curr here)
                //The face normal is rotated 90 deg to the vector created between these two points
                //and rotating vector 90deg in 2d is ez: (x,y) => (-y,x) (or (y,-x) for other direction..)
                Vector2 faceNormal = new(-(curr.Y - prev.Y), curr.X - prev.X); // I say "normal" but aint normalized, not needed because I only need the direction
                //the normal is calculated inwards because we are intressted in the support point towards the face of the polygon

                //DebugDraw.DrawLine((prev + curr) / 2, (prev + curr) / 2 + Vector2.Normalize(faceNormal) * 10);
                prev = curr;

                hull2.SupportPoint(faceNormal, out float max); // I dont need the support point only the max value in the direction of the normal
                float min = Vector2.Dot(faceNormal, curr); //the distance along the axis of current face
                float distance = min - max; //signed distance, negative means overlapping on this axis

                if (distance > 0) return distance; // if positive we can exit early because we found a separating axis!

                if (distance > bestDistance)
                {
                    bestDistance = distance; //to find shortest overlap distance
                    //in more advanced functions the axis (faceNormal)  and best face (curr+prev points)
                    //is also intresting to save and return for collision resolving purposes, not just detecting overlap as here
                }

            }
            return bestDistance;
        }


        /// <summary>
        /// Gets the not normalized face normal of the face made up by point a and b 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>A Vector2 in the direction of the face normal</returns>
        private static Vector2 GetFaceNormal(Vector2 a, Vector2 b)
        {
            Vector2 n = new();
            GetFaceNormal(a, b, ref n);
            return n;
        }
        private static void GetFaceNormal(Vector2 a, Vector2 b, ref Vector2 n)
        {
            n.X = b.Y - a.Y;
            n.Y = -(b.X - a.X);
        }


    }
}
