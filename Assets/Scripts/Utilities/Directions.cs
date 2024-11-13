using UnityEngine;

namespace EmeWillem
{
    namespace Utilities
    {
        public static class Directions
        {
            public static DirectionType Translate(Vector3 movementDirection, Vector3 transformForward, Vector3 transformRight, float dotThreshold = 0.5f)
            {
                float forwardDot = Vector3.Dot(transformForward, movementDirection);
                float rightDot = Vector3.Dot(transformRight, movementDirection);

                if (forwardDot >= dotThreshold)
                {
                    return DirectionType.Forward;
                }
                else if (forwardDot <= -dotThreshold)
                {
                    return DirectionType.Backward;
                }
                else if (rightDot >= dotThreshold)
                {
                    return DirectionType.Right;
                }
                else if (rightDot <= -dotThreshold)
                {
                    return DirectionType.Left;
                }

                return DirectionType.None;
            }

            public static Vector3 Normalize(Vector3 direction)
            {
                direction.y = 0;
                direction.Normalize();

                return direction;
            }
        }
    }
}