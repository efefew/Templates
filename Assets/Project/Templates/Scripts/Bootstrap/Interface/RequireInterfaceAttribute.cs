using System;

using UnityEngine;

public class RequireInterfaceAttribute : PropertyAttribute
{
    public Type InterfaceType { get; }

    public RequireInterfaceAttribute(Type interfaceType) => InterfaceType = interfaceType;
}