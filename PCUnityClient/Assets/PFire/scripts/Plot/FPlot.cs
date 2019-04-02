using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FPlot {
    protected bool isEnd;
    public abstract void PlotStart();
    public abstract void PlotEnd();
    public abstract void PlotUpdate();

}
