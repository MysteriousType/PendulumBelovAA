namespace Assets.Logic.Runtime.Common.Extensions
{
    using UnityEngine;

    public static class TransformExtensions
    {
        public static Transform FindChildByName(this Transform transform, string childName)
        {
            foreach (Transform childTransform in transform)
            {
                if (childTransform.name == childName)
                {
                    return childTransform;
                }

                Transform foundChildTransform = childTransform.FindChildByName(childName);

                if (foundChildTransform != null)
                {
                    return foundChildTransform;
                }
            }

            return null;
        }

        public static T FindComponentInChild<T>(this Transform transform, string childName) where T : Component
        {
            Transform child = transform.FindChildByName(childName);
            return child.GetComponent<T>();
        }

        public static T FindComponent<T>(this Transform transform, string objectName) where T : Component
        {
            if (transform.name == objectName)
            {
                return transform.GetComponent<T>();
            }

            Transform child = transform.FindChildByName(objectName);
            return child.GetComponent<T>();
        }
    }
}