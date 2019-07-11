using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sex { MALE = 0, FEMALE = 1 };
public enum Animal_State { DEAD = -1, RESTING = 0, SEARCHING_FOR_FOOD = 1, GOING_AFTER_FOOD = 2, FLEEING = 3 };

public struct Animal_Stats
{
    public float strength { get; }
    public float baseSpeed { get; }
    public float sizeScale { get; }
    public float visionRadius { get; }
    public float camouflage { get; }
    public Sex sex { get; }

    public Animal_Stats(float _strength, float _baseSpeed, float _sizeScale, float _visionRadius, float _camouflage, Sex _sex)
    {
        strength = _strength;
        baseSpeed = _baseSpeed;
        sizeScale = _sizeScale;
        visionRadius = _visionRadius;
        camouflage = _camouflage;
        sex = _sex;
    }
}

static class GlobalValues
{
    public static float MIN_STRENGTH = 0.0f;
    public static float MAX_STRENGTH = Mathf.Infinity;
    public static float STRENGTH_CONSUMPTION_FACTOR = 0.015f;

    public static float MIN_BASE_SPEED = 0.0f;
    public static float MAX_BASE_SPEED = Mathf.Infinity;
    public static float SPEED_CONSUMPTION_FACTOR = 0.01f;

    public static float MIN_SIZE_SCALE = 1.0f;
    public static float MAX_SIZE_SCALE = 3.0f;
    public static float SIZE_SCALE_CONSUMPTION_FACTOR = 0.3f;

    public static float MIN_VISION_RADIUS = 1.0f;
    public static float MAX_VISION_RADIUS = Mathf.Infinity;
    public static float VISION_RADIUS_CONSUMPTION_FACTOR = 0.05f;

    public static float MIN_CAMOUFLAGE = 0.0f;
    public static float MAX_CAMOUFLAGE = 100.0f;
    public static float CAMOUFLAGE_CONSUMPTION_FACTOR = 0.0075f;

    public static int PREY_DAYS_TO_ADULTHOOD = 5;
    public static int PREY_RUT_SEASON = 0;
    public static float PREY_PROBABILITY_TO_HAVE_CHILDREN = 20;
    public static int PREY_MAX_LITTER = 7;

    public static int PREDATOR_DAYS_TO_ADULTHOOD = 30;
    public static int PREDATOR_RUT_SEASON = 0;
    public static float PREDATOR_PROBABILITY_TO_HAVE_CHILDREN = 20;
    public static int PREDATOR_MAX_LITTER = 3;


}
