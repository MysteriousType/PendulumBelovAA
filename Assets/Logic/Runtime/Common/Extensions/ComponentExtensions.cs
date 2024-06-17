namespace Assets.Logic.Runtime.Common.Extensions
{
    using UnityEngine;

    public static class ComponentExtensions
    {
        public static Transform FindChildByName(this Component component, string childName)
        {
            return component.transform.FindChildByName(childName);
        }

        public static T FindComponentInChild<T>(this Component component, string childName) where T : Component
        {
            Transform child = component.transform.FindChildByName(childName);
            return child.GetComponent<T>();
        }
    }
}