using System.Collections.Generic;
using UnityEngine;

public class ColliderIgnoreList : MonoBehaviour
{
    private readonly List<Collider2D> ignoredColliders = new List<Collider2D>();

    public void Add(IEnumerable<Collider2D> colliders)
    {
        ignoredColliders.AddRange(colliders);
    }

    public bool Contains(Collider2D other)
    {
        return ignoredColliders.Contains(other);
    }
}