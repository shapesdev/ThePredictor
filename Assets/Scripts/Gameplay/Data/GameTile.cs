using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile : MonoBehaviour
{
    [SerializeField]
    private Transform arrow = default;

    private GameTileContent content;
    private GameTile north, east, south, west, nextOnPath;
    private int distance;

    static Quaternion
        northRotation = Quaternion.Euler(90f, 0f, 0f),
        eastRotation = Quaternion.Euler(90f, 90f, 0f),
        southRotation = Quaternion.Euler(90f, 180f, 0f),
        westRotation = Quaternion.Euler(90f, 270f, 0f);

    public bool HasPath => distance != int.MaxValue;
    public bool IsAlternative { get; set; }
    public GameTile NextTileOnPath => nextOnPath;
    public Vector3 ExitPoint { get; private set; }
    public Direction PathDirection { get; private set; }
    public GameTileContent Content {
        get => content;
        set {
            Debug.Assert(value != null, "Null assigned to content!");
            if (content != null) {
                content.Recycle();
            }
            content = value;
            content.transform.localPosition = transform.localPosition;
        }
    }

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

    public void HidePath() {
        arrow.gameObject.SetActive(false);
    }

    public void ClearPath() {
        distance = int.MaxValue;
        nextOnPath = null;
    }

    public void BecomeDestination() {
        distance = 0;
        nextOnPath = null;
        ExitPoint = transform.localPosition;
    }

    GameTile GrowPathTo(GameTile neighbor, Direction direction) {
        Debug.Assert(HasPath, "No path!");
        if (neighbor == null || neighbor.HasPath) {
            return null;
        }
        neighbor.distance = distance + 1;
        neighbor.nextOnPath = this;
        neighbor.ExitPoint =
            neighbor.transform.localPosition + direction.GetHalfVector();
        neighbor.PathDirection = direction;
        return neighbor.content.Type != GameTileContentType.Wall ? neighbor : null;
    }

    public GameTile GrowPathNorth() => GrowPathTo(north, Direction.South);

    public GameTile GrowPathEast() => GrowPathTo(east, Direction.West);

    public GameTile GrowPathSouth() => GrowPathTo(south, Direction.North);

    public GameTile GrowPathWest() => GrowPathTo(west, Direction.East);

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
