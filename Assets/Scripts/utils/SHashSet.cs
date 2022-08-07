using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Serialization;
using Unity.VisualScripting;

[Serializable]
public class SHashSet<T> : HashSet<T>
{
    public SHashSet() {
        
    }

    public SHashSet(SerializationInfo info, StreamingContext context)
    {
        T[] thisArray = info.GetValue("dict", typeof(T[])) as T[];
        this.AddRange(thisArray);
        
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue("dict", this.ToArray(), typeof(T[]));
    }

}