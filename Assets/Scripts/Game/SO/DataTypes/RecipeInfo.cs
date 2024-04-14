using System;
using System.Collections.Generic;

[Serializable]
public class RecipeInfo
{
    public KeyValuePair<UnitSO, int>[] Input;
    public KeyValuePair<UnitSO, int> Output;
}
