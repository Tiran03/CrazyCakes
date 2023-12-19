using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{

    [SerializeField] private List<WayPoint> m_waypoints = new List<WayPoint>();
    [SerializeField] public bool activatesDefeat = false; // Nueva propiedad
    [SerializeField] private bool visited = false;

    public List<WayPoint> GetWaypoints()
    {
        return m_waypoints;
    }

    public void AddWaypoint(WayPoint waypoint)
    {
        if (!m_waypoints.Contains(waypoint))
        {
            m_waypoints.Add(waypoint);
        }
    }

    public bool Contains(WayPoint p_waypoint)
    {
        return m_waypoints.Contains(p_waypoint);
    }

    public bool IsVisited()
    {
        return visited;
    }

    public void MarkVisited()
    {
        visited = true;
    }

    public List<WayPoint> GetOneWaypoints()
    {
        List<WayPoint> oneWaypoints = new List<WayPoint>();
        foreach (WayPoint waypoint in m_waypoints)
        {
            if (!waypoint.Contains(this))
            {
                oneWaypoints.Add(waypoint);
            }
        }
        return oneWaypoints;
    }

    public List<WayPoint> GetBackwardWaypoints()
    {
        List<WayPoint> backwardWaypoints = new List<WayPoint>();
        foreach (WayPoint waypoint in m_waypoints)
        {
            if (waypoint.Contains(this))
            {
                backwardWaypoints.Add(waypoint);
            }
        }
        return backwardWaypoints;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);

        foreach (var l_waypoint in m_waypoints)
        {
            Gizmos.color = Color.blue;
            if (l_waypoint.Contains(this))
            {
                Gizmos.color = Color.green;
            }

            Gizmos.DrawLine(transform.position, l_waypoint.transform.position);
        }
    }
}
