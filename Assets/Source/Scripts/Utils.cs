using System;
using System.Linq;
using UnityEngine;

namespace AdventureWorld.Prueba
{
    public class Utils
    {
        // create a child gameobject with a trigger collider with the radius of the gameobject
        // this will be used to detect if the player is within the radius of the gameobject
        // it should have a script attached to it that will call the OnTriggerEnter2D method
        public static GameObject CreateTrigger(GameObject parent, float radius, Vector2 offset, Type[] components,
            String name = "Trigger")
        {
            if (components == null)
            {
                components = new Type[] { };
            }

            components = new[] {typeof(CircleCollider2D)}.Concat(components).ToArray();
            GameObject trigger = new GameObject("Trigger", components)
            {
                transform =
                {
                    parent = parent.transform,
                    localPosition = Vector3.zero,
                }
            };
            var collider = trigger.GetComponent<CircleCollider2D>();
            collider.isTrigger = true;
            collider.radius = radius;
            collider.offset = offset;
            return trigger;
        }
    }
}