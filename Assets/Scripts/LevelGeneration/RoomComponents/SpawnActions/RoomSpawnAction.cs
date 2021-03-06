﻿using UnityEngine;

/*
 * Any action that needs to be triggered once level generation is complete. The Room may not be activated yet.
 */
public abstract class RoomSpawnAction : ScriptableObject
{
    /*
     * Any action that needs to be triggered once level generation is complete. The Room may not be activated yet.
     */
    public abstract void OnRoomGenerationDone(Room room);

    /*
     * Any action that needs to be triggered once a room as been activated. At the time of writting this, that means
     * the room adjacent to the called Room has had all the enemies killed.
     */
    public abstract void OnRoomActivated(Room room);
}