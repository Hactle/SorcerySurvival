using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public int Version = 1;

    public LevelProgressData Levels = new();
    public PlayerProgressData Player = new();
    public SettingsData Settings = new();
    public ShopData Shop = new();
}

[Serializable]
public class LevelProgressData
{
    public List<int> CompletedLevels = new();
}

[Serializable]
public class PlayerProgressData
{
    public int Experience;
    public int Coins;
}

[Serializable]
public class SettingsData
{

}

[Serializable]
public class ShopData
{

}