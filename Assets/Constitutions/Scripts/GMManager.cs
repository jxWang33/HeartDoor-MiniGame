﻿using System;
using UnityEngine;

public class GMManager
{
    public const int START_HEART = 2;
    public const float JAM_ANGLE = 140;
    public static readonly System.Random rd = new System.Random(GetRandomSeed());//随机种子

    public const KeyCode UP_KEY = KeyCode.W;
    public const KeyCode DOWN_KEY = KeyCode.S;
    public const KeyCode LEFT_KEY = KeyCode.A;
    public const KeyCode RIGHT_KEY = KeyCode.D;
    public const KeyCode JUMP_KEY = KeyCode.J;
    public const KeyCode DASH_KEY = KeyCode.K;
    public const KeyCode DOOR_KEY = KeyCode.L;
    public const KeyCode FUNC_KEY = KeyCode.Space;

    public const string LEVEL_1 = "Level1-MysteriousCall";
    public const string LEVEL_2 = "Level2-GateCrasher";
    public const string LEVEL_3 = "Level3-BestFriend";
    public const string LEVEL_4 = "Level4-GoAhead";
    public const string LEVEL_5 = "Level5-HeartLock";

    public const string BadEnd_1 = "BadEnd1-Alone";
    public const string BadEnd_2 = "BadEnd2-RunAway";
    public const string NORMAL_END = "NormalEnd-Conservatism";
    public const string HAPPY_END = "HappyEnd-ForFuture";

    public static bool GAME_PAUSED = false;
    public static bool GAME_INITED = false;

    public static void GamePause() {
        GAME_PAUSED = true;
        Time.timeScale = 0;
    }
    public static void GameResume() {
        GAME_PAUSED = false;
        Time.timeScale = 1;
    }

    public static void Init() {
        QualitySettings.vSyncCount = 0;
        QualitySettings.pixelLightCount = 16;
        Application.targetFrameRate = 60;
        GAME_INITED = true;
    }

    private static int GetRandomSeed() {
        byte[] bytes = new byte[4];
        System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
        rng.GetBytes(bytes);
        return BitConverter.ToInt32(bytes, 0);
    }
}
