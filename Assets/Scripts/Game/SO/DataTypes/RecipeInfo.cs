using Misc;
using System;

[Serializable]
public class RecipeInfo
{
    public KeyValuePair<UnitSO, int>[] Input;
    public KeyValuePair<UnitSO, int> Output;
}
