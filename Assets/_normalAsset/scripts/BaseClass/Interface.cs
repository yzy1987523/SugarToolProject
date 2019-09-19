using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ISetValue {    
    void SetValue(MorphingValueType _type,ValueData _value);    
} 
public interface ISelectable
{
    void Select();
    void QuitSelect();
}

