using System.Collections.Generic;

public static class ObjectTracker
{
    public readonly static HashSet<string> DestroyedObjectIDs = new();
}
