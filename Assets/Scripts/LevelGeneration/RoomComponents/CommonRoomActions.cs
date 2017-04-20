using UnityEngine;

public class CommonRoomActions
{
    public static readonly RoomClearAction[] COMMON_CLEAR_ACTIONS =
    {
        ScriptableObject.CreateInstance<OpenDoors>(),
        ScriptableObject.CreateInstance<ActivateNextRoom>(),
        ScriptableObject.CreateInstance<GenerateRoomOnEnd>()
    };
    public static readonly RoomSpawnAction[] COMMON_SPAWN_ACTIONS = { };
    public static readonly RoomStartAction[] COMMON_START_ACTIONS = { };
}