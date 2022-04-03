using UnityEngine;
public static class PhysicsUtils
{
    public static void DoMoveRigidBodyByKnockback(Rigidbody2D rb2d, Vector2 empuje, float knockbackForce, float deltaTime, string layerToIgnore = "Default")
    {
        // move the enemy to the knockback direction
        // determine if it would collide with anything
        // if not, move the enemy
        // if it would collide, move the enemy to the closest safe spot
        var direction = new Vector3(empuje.x, empuje.y, 0);
        var force = knockbackForce;
        var transform = rb2d.transform;
        // create a raycast at the enemy's position in the direction of the knockback based on the force and deltaTime
        // if the raycast hits the player, retry the raycast at the new position with a smaller force
        var hit = Physics2D.Raycast(transform.position, direction, force * deltaTime, 1 << LayerMask.NameToLayer("Default"));
        if (hit.collider != null)
        {
            // if the raycast hits the player, retry the raycast at the new position with a smaller force
            if (hit.collider.tag == "Player")
            {
                hit = Physics2D.Raycast(hit.collider.transform.position, direction, force * deltaTime / 2, 1 << LayerMask.NameToLayer("Default"));
            }
        }

        if (hit.collider == null)
        {

            rb2d.MovePosition(transform.position + direction * force * deltaTime);
            // rb2d.MovePosition(transform.position + new Vector3(empuje.x, empuje.y, 0) * knockbackForce * deltaTime);

        }
        else
        {


            var hitPoint = hit.point;
            var distance = Vector2.Distance(hitPoint, transform.position);
            var directionToMove = (hitPoint - (Vector2)transform.position).normalized;
            var distanceToMove = distance - 0.5f;
            rb2d.MovePosition((Vector2)transform.position + directionToMove * distanceToMove * deltaTime);
        }
    }
}