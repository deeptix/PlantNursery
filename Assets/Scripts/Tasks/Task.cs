using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Task
{
    // Returns string describing the current task, i.e. "Water plant 5 times"
    public abstract string GetTaskName();

    // Returns string describing current progress, i.e. "3/5"
    public abstract string GetTaskProgressString(); 
    
    // Returns whether the task has been completed
    public abstract bool IsCompleted();
}
