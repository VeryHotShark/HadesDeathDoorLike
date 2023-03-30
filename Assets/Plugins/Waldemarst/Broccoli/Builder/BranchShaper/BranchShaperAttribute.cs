using System;

/// <summary>
/// Attribute to apply to Broccoli Branch Shaper implementations.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class BranchShaperAttribute : Attribute
{
    public readonly int id;
    public readonly int order;
    public readonly string name;
    public readonly bool enabled;

    public BranchShaperAttribute (int id, string name, int order = 0, bool enabled = true) {
        this.id = id;
        this.order = order;
        this.name = name;
        this.enabled = enabled;
    }
}