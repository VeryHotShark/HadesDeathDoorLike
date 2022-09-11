using NodeCanvas.Framework;
using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(Blackboard))]
public class PassPathToBlackboard : MonoBehaviour
{
    private Blackboard blackboard;

    public GameObject destination;

    void Awake()
    { blackboard = GetComponent<Blackboard>(); }

    void Start()
    {
        Debug.Log("Generating path");
        var path = ABPath.Construct(transform.position, destination.transform.position, SendPathToBlackboard);
        AstarPath.StartPath(path);
    }

    private void SendPathToBlackboard(Path path)
    {
        blackboard.SetVariableValue("myPath", path);
        Debug.Log("Generated path and passed to blackboard");
    }
}
