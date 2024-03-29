/** 
 * Generic Event Types used to declare events that can happen across the game
 *
 * HOW TO USE:
 * Add to enum the names of different events
 **/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventTypes
{
    DayPassed,
    WateredPlant,
    AgedPlant,
    DraggingPlant,
    ZoomedIn,
    ZoomedOut,
    UpdatedPlant,       // Broadcasted when a plant's information is updated
    FinishedLevel,      // Broadcasted when level is complete to remove dropzone listeners
}
