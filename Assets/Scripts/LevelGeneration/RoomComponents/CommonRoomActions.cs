using UnityEngine;

/*
 * Collection of common ScriptableObjects that should be used by Rooms when a room is cleared,
 * player enters a room, or the room is placed into the world by the level generator
 */
[CreateAssetMenu(fileName = "CommonRoomActions", menuName = "ScriptableObjects/Rooms/CommonRoomActions")]
public class CommonRoomActions : ScriptableObject
{
    public RoomClearAction[] clearActions;
    public RoomSpawnAction[] spawnActions;
    public RoomStartAction[] playerEnterActions;
}