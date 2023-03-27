using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Contracts", menuName = "Contracts - New")]
public class Contract : ScriptableObject
{
    public string contractName;
    public List<string> contractGains;
    public List<string> contractLosses;
    public List<ContractStack> stacksToAdd;
}

[Serializable]
public class ContractStack
{
    public Stack stack;
    public int stacksToAdd;
}
