using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile : MonoBehaviour
{
    [SerializeField]
    Transform arrow = default;

    GameTile north, east, south, west, nextOnPath;
    int distance;

    static Quaternion
        northRotation = Quaternion.Euler(90f, 0f, 0f),
        eastRotation = Quaternion.Euler(90f, 90f, 0f),
        southRotation = Quaternion.Euler(90f, 180f, 0f),
        westRotation = Quaternion.Euler(90f, 270f, 0f);

    public bool HasPath => distance != int.MaxValue;
    public bool IsAlternative { get; set; }

    public void ShowPath() {
        if(distance == 0) {
            arrow.gameObject.SetActive(false);
            return;
        }
        arrow.gameObject.SetActive(true);
        arrow.localRotation =
            nextOnPath == north ? northRotation :
            nextOnPath == east ? eastRotation :
            nextOnPath == south ? southRotation :
            westRotation;
    }

    public void ClearPath() {
        distance = int.MaxValue;
        nextOnPath = null;
    }

    public void BecomeDestination() {
        distance = 0;
        nextOnPath = null;
    }

    GameTile GrowPathTo(GameTile neighbor) {
        Debug.Assert(HasPath, "No path!");
        if (neighbor == null || neighbor.HasPath) {
            return null;
        }
        neighbor.distance = distance + 1;
        neighbor.nextOnPath = this;
        return neighbor;
    }

    public GameTile GrowPathNorth() => GrowPathTo(north);

    public GameTile GrowPathEast() => GrowPathTo(east);

    public GameTile GrowPathSouth() => GrowPathTo(south);

    public GameTile GrowPathWest() => GrowPathTo(west);

    public static void MakeEastWestNeighbors(GameTile east, GameTile west) {
        Debug.Assert(
    east.west == null && west.east == null, "Redefined neighbors");
        east.west = west;
        west.east = east;
    }

    public static void MakeNorthSouthNeighbors(GameTile north, GameTile south) {
        Debug.Assert(
            south.north == null && north.south == null, "Redefined neighbors");
        north.south = south;
        south.north = north;
    }
}
