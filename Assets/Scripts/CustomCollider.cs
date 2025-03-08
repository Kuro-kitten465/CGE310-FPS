using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class CustomColliderData
{
    public Transform ColliderTransform;
    public float DetectionRadius = 1f;
    public LayerMask DetectionLayer;
    public int ArraySize = 20;
    public UnityEvent<Collider> OnEnter;
    public UnityEvent<Collider> OnStay;
    public UnityEvent<Collider> OnExit;
}

public class CustomCollider
{
    private CustomColliderData m_Data;
    private Collider[] m_DetectedObjects;
    private HashSet<Collider> m_PreviousObjects = new();

    public CustomCollider(CustomColliderData data)
    {
        m_Data = data;
        m_DetectedObjects = new Collider[m_Data.ArraySize];
    }

    public void DetectObjects()
    {
        int detectedCount = Physics.OverlapSphereNonAlloc(m_Data.ColliderTransform.position, m_Data.DetectionRadius, m_DetectedObjects, m_Data.DetectionLayer);

        HashSet<Collider> currentObjects = new HashSet<Collider>();

        for (int i = 0; i < detectedCount; i++)
        {
            Collider obj = m_DetectedObjects[i];
            currentObjects.Add(obj);

            if (!m_PreviousObjects.Contains(obj)) // New object detected -> OnEnter
            {
                m_Data.OnEnter?.Invoke(obj);
            }

            m_Data.OnStay?.Invoke(obj); // Object is still inside -> OnStay
        }

        // Detect objects that left the sphere -> OnExit
        foreach (Collider obj in m_PreviousObjects)
        {
            if (!currentObjects.Contains(obj))
            {
                m_Data.OnExit?.Invoke(obj);
            }
        }

        m_PreviousObjects = currentObjects;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(m_Data.ColliderTransform.position, m_Data.DetectionRadius);
    }
}