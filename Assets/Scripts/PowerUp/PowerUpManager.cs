using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public static class PowerUpManager
{
    private static int _nullDropChance = 8;
    private static Dictionary<Type, int> _dropPorportions = new Dictionary<Type, int>() 
    {
        {typeof(ShieldPowerUp), 4},
        {typeof(RepairPowerUp), 4},
        {typeof(MissilePowerUp), 2},
        {typeof(TripleMachineGunPowerUp), 2},
    };
    private static Dictionary<Type, Vector2> _dropRates = null;

    public static PowerUp GetRandomPowerup()
    {
        if(_dropRates == null)
            CalculateDropRates();

        int totalChance = _dropPorportions.Values.Sum() + _nullDropChance;

        int chance = UnityEngine.Random.Range(0, totalChance);
        System.Diagnostics.Debug.WriteLine(chance);

        if(chance < _nullDropChance)
            return null;

        foreach (var item in _dropRates)
        {
            Type type = item.Key;

            if(chance < item.Value.y && chance >= item.Value.x)
                return (PowerUp) Activator.CreateInstance(type);
                // return (PowerUp) item.Key.GetConstructors()[0].Invoke(null);
        }

        return null;
    }
    private static void CalculateDropRates()
    {
        int accumulator = _nullDropChance;
        _dropRates = new Dictionary<Type, Vector2>();

        foreach (var item in _dropPorportions)
        {
            Vector2 values = new Vector2(accumulator, accumulator + item.Value);

            _dropRates.Add(item.Key, values);
            accumulator += item.Value;

            System.Diagnostics.Debug.WriteLine(item.Key + ": " + values.x + "-" + (values.y - 1));
        }
    }
}
