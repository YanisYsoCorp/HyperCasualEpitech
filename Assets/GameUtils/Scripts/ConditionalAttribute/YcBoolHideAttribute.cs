using UnityEngine;
using System;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
public class YcBoolHideAttribute : PropertyAttribute {

    public string ConditionalSourceField = "";
    public bool HideWhenEqualToValue;

    public YcBoolHideAttribute(string conditionalSourceField, bool hideWhenEqualToValue) {
        this.ConditionalSourceField = conditionalSourceField;
        this.HideWhenEqualToValue = hideWhenEqualToValue;
    }
}