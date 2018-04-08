using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class RaycastController : MonoBehaviour
{
    public LayerMask collisionMask;

    public const float skinWidth = .015f;
    private const float dstBetweenRays = .25f;

    [HideInInspector]
    public int horizontalRayCount;
    [HideInInspector]
    public int verticalRayCount;

    [HideInInspector]
    public float horizontalRaySpacing;
    [HideInInspector]
    public float verticalRaySpacing;

    [HideInInspector]
    public Collider2D coll;
    [HideInInspector]
    public RaycastOrigins raycastOrigins;

    public virtual void Awake()
    {
        var player = GetComponent<Player>();
        if (player != null)
        {
            var collider2Ds = player.GetComponents<Collider2D>();
            coll = collider2Ds.FirstOrDefault(x => x.enabled);
        }
        else
        {
            coll = GetComponent<BoxCollider2D>();
        }
    }

    public virtual void Start()
    {
        CalculateRaySpacing();
    }

    public void UpdateRaycastOrigins()
    {
        var bounds = coll.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    public void CalculateRaySpacing()
    {
        var bounds = coll.bounds;
        bounds.Expand(skinWidth * -2);

        var boundsWidth = bounds.size.x;
        var boundsHeight = bounds.size.y;


        horizontalRayCount = Mathf.RoundToInt(boundsHeight / dstBetweenRays);
        verticalRayCount = Mathf.RoundToInt(boundsWidth / dstBetweenRays);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    public struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }
}
