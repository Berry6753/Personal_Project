using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;

public class Character
{ 
    public int DataId {  get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string PrefabPath {  get; set; }

    public List<string> AttackClassNameList { get; set; }
}

public class Attack
{ 
    public string AttackClassName {  get; set; }
    public string AttackName { get; set; }
    public string AttackDescription { get; set;}
    public int BaseDamage {  get; set; }
    public int UpScaleDamage {  get; set; }
}