using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WaterflowShortWaySearcher : MonoBehaviour
{
    [SerializeField] private Transform _spawnerTransform;
    [SerializeField] private Transform _homeTransform;
    [SerializeField] private RuleTile _roadRuleTile;
    [SerializeField] private Tilemap _map;
    private float SpawnerZPos => _spawnerTransform.position.z;
    private Vector3Int[] _localDirections = new Vector3Int[4]
    {
        Vector3Int.up,
        Vector3Int.right,
        Vector3Int.down,
        Vector3Int.left
    };
    private Vector3 _defaultWaypointOffset = new Vector3(0.12f, 0.12f, 0);
    private List<Node> _allRoadNodes;
    private Node _homeNode;
    private Node _spawnerNode;
    public List<Vector3> Waypoints;

    private void Awake() {
        Initialize();
        WaterFlow();
        InstantiateWaypoints();
    }

    private void Initialize()
    {
        _allRoadNodes = GenerateAllRoadNodes(_map);
        _spawnerNode = _allRoadNodes.Find(node =>
            node.CellPosition == _map.WorldToCell(_spawnerTransform.position));
        _homeNode = _allRoadNodes.Find(node =>
            node.CellPosition == _map.WorldToCell(_homeTransform.position));
    }

    private void WaterFlow()
    {
        Grow(_homeNode);

        while (_spawnerNode.Status == NodeStatus.DEFAULT)
        {
            if (GetNodeLayer(NodeStatus.READY_TO_GROW, out List<Node> layer))
            {
                foreach (Node node in layer)
                {
                    Grow(node);
                }
            }
            else
            {
                Debug.LogWarning("Home is not reachable");
                return;
            }
        }
    }

    private bool GetNeighborNode(Node node, Vector3Int direction, out Node neighbor)
    {
        neighbor = _allRoadNodes.Find(neighbor => neighbor.CellPosition == node.CellPosition + direction);
        return neighbor != null;
    }

    private void Grow(Node node)
    {
        node.Status = NodeStatus.GROWED;
        foreach (Vector3Int direction in _localDirections)
        {
            if (GetNeighborNode(node, direction, out Node neighbor))
            {
                if (neighbor.Status == NodeStatus.DEFAULT)
                {
                    neighbor.PreviousNode = node;
                    neighbor.StepsToHome = node.StepsToHome + 1;
                    neighbor.Status = NodeStatus.READY_TO_GROW;
                }
            }
        }
    }

    private void InstantiateWaypoints()
    {
        Node node = _spawnerNode;
        while (node.PreviousNode != null)
        {
            Waypoints.Add(node.WorldPosition + _defaultWaypointOffset);
            node = node.PreviousNode;
        }
        Waypoints.Add(node.WorldPosition + _defaultWaypointOffset);
    }

    private bool GetNodeLayer(NodeStatus status, out List<Node> layer)
    {
        layer = _allRoadNodes.FindAll(node => node.Status == status);
        return layer.Count != 0;
    }

    private List<Node> GenerateAllRoadNodes(Tilemap tilemap)
    {
        List<Node> nodes = new List<Node>();

        BoundsInt cellBounds = tilemap.cellBounds;
        for (int y = cellBounds.yMin; y < cellBounds.yMax; y++)
        {
            for (int x = cellBounds.xMin; x < cellBounds.xMax; x++)
            {
                Vector3Int cellPosition = new Vector3Int(x, y, cellBounds.zMin);
                if (_map.GetTile<RuleTile>(cellPosition) == _roadRuleTile)
                {
                    Node node = new Node();

                    node.CellPosition = cellPosition;
                    node.WorldPosition = tilemap.CellToWorld(cellPosition);
                    node.Status = NodeStatus.DEFAULT;
                    node.StepsToHome = 0;

                    nodes.Add(node);
                }

            }
        }

        return nodes;
    }

    internal class Node
    {
        [SerializeField]
        public Vector3Int CellPosition { get; set; }
        [SerializeField]
        public Vector3 WorldPosition { get; set; }
        [SerializeField]
        public NodeStatus Status { get; set; }
        [SerializeField]
        public int StepsToHome { get; set; }
        [SerializeField]
        public Node PreviousNode { get; set; }
    }

    internal enum NodeStatus
    {
        DEFAULT,
        READY_TO_GROW,
        GROWED
    }
}