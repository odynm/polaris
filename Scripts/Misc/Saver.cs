// Polaris - Project VII 2018
// 08/03/18 -> 02/07/18
// Compiled in Unity 2017.2.0f3
// Writed by Mathias Ody (M-Ody)
// mathiasluizody@hotmail.com
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saver 
{
    private string _myDocuments;

    private GamePrefs _gamePrefs;
    private GameStats _gameStats;

    private static Saver _instance;
    public static Saver Instance
    {
        get
        {
            if (_instance == null)
                _instance = new Saver();
            return _instance;
        }
    }

    public Saver ()
    {
        _myDocuments = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Polaris");

        if (!Directory.Exists(_myDocuments))
            Directory.CreateDirectory(_myDocuments);

        _gamePrefs = GetPrefsFromFile();
        _gameStats = GetStatsFromFile();
    }

    public void SavePrefs(GamePrefs prefs)
    {
        _gamePrefs = prefs;
        SavePrefsInFile();
    }

    public void SaveStats(GameStats stats)
    {
        _gameStats = stats;
        SaveStatsInFile();
    }

    public GamePrefs GetPrefs()
    {
        return _gamePrefs;
    }

    public GameStats GetStats()
    {
        return _gameStats;
    }

    public void DeleteProgress()
    {
        File.Delete(Path.Combine(_myDocuments, "stats.panetone"));
        _gameStats = new GameStats();
    }

    private void SavePrefsInFile()
    {
        string stream = "";
        stream += Encode(_gamePrefs.graphicsQuality);
        stream += Encode(_gamePrefs.brightness);
        stream += Encode(_gamePrefs.postProcessing);
        stream += Encode(_gamePrefs.volumeMaster);
        stream += Encode(_gamePrefs.volumeFX);
        stream += Encode(_gamePrefs.volumeMusic);
        stream += Encode(_gamePrefs.mouseSensitivity);
        stream += Encode(_gamePrefs.invertMouseClick);
        File.WriteAllText(Path.Combine(_myDocuments, "prefs.panetone"), stream);
    }

    private void SaveStatsInFile()
    {
        string stream = "";
        stream += Encode(_gameStats.level);
        stream += Encode(_gameStats.hasShotgun);
        stream += Encode(_gameStats.hasRocketLauncher);
        stream += Encode(_gameStats.health);
        stream += Encode(_gameStats.pistolAmmo);
        stream += Encode(_gameStats.shotgunAmmo);
        stream += Encode(_gameStats.rocketLauncherAmmo);
        File.WriteAllText(Path.Combine(_myDocuments, "stats.panetone"), stream);
    }

    private GamePrefs GetPrefsFromFile()
    {
        if (File.Exists(Path.Combine(_myDocuments,"prefs.panetone")))
        {
            return DecodePrefs();
        }
        else 
        {
            return new GamePrefs();
        }
    }

    private GameStats GetStatsFromFile()
    {
        if (File.Exists(Path.Combine(_myDocuments,"stats.panetone")))
        {
            return DecodeStats();
        }
        else 
        {
            return new GameStats();
        }
    }

    private GamePrefs DecodePrefs()
    {
        string[] stream = File.ReadAllLines(Path.Combine(_myDocuments, "prefs.panetone"));
        return new GamePrefs 
        {
            graphicsQuality = Decode(stream [0]),
            brightness = Decode(stream [1]) / 100f,
            postProcessing = Decode(stream[2]) == 1,
            volumeMaster = Decode(stream[3]) / 100f,
            volumeFX = Decode(stream[4]) / 100f,
            volumeMusic = Decode(stream[5]) / 100f,
            mouseSensitivity = Decode(stream[6]) / 100f,
            invertMouseClick = Decode(stream[7])  == 1
        };
    }

    private GameStats DecodeStats()
    {
        string[] stream = File.ReadAllLines(Path.Combine(_myDocuments, "stats.panetone"));
        return new GameStats 
        {
            level = Decode(stream [0]),
            hasShotgun = Decode(stream [1]) == 1,
            hasRocketLauncher = Decode(stream [2]) == 1,
            health = Decode(stream [3]),
            pistolAmmo = Decode(stream [4]),
            shotgunAmmo = Decode(stream [5]),
            rocketLauncherAmmo = Decode(stream [6]),
        };
    }

    private int Decode(string s)
    {
        char[] c = new char[s.Length];
        for (int i = 0; i < s.Length; i++)
        {
            c[i] = (char)(s[i] - (char)11);
        }
        return int.Parse(new string(c));
    }

    private string Encode(int n)
    {
        string s = n.ToString();
        char[] c = new char[s.Length];
        for (int i = 0; i < s.Length; i++)
        {
            c[i] = (char)(s[i] + (char)11);
        }
        return new string(c) + "\n";
    }

    private string Encode(float n)
    {
        return Encode ((int)(n * 100f));
    }

    private string Encode(bool b)
    {
        int n = b ? 1 : 0;
        return Encode(n);
    }
}

public class GamePrefs
{
    public int graphicsQuality = 2;
    public float brightness = 0.5f;
    public bool postProcessing = true;

    public float volumeMaster = 1.0f;
    public float volumeFX = 1.0f;
    public float volumeMusic = 1.0f;

    public float mouseSensitivity = 0.6f;
    public bool invertMouseClick = false;
}

public class GameStats
{
    public int level = 0;
    public bool hasShotgun = false;
    public bool hasRocketLauncher = false;
    public int health = 100;
    public int pistolAmmo = 20;
    public int shotgunAmmo = 0;
    public int rocketLauncherAmmo = 0;
}