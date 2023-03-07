using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public Character c;
    int exp;
    public bool isDead;
    public float power
    {
        get
        {
            return c.mass * c.scaler;
        }
    }
    public int maxMass
    {
        get
        {
            return 10 + c.massLv * 5;
        }
    }
    public int expLimit
    {
        get
        {
            return c.massLv * c.scaler * (5 * (1 + c.massLv));
        }
    }

    public bool isFull()
    {
        if (c.mass >= maxMass)
        {
            return true;
        }
        else return false;
    }

    public void AddExp(int exPoint)
    {
        exp += exPoint;
        if (exp >= expLimit)
        {
            exp -= expLimit;
            c.massLv++;
            GameManager.Instance.setLv(c.massLv);
        }
        GameManager.Instance.setXp(exp, expLimit);
    }

    public Player()
    {
        c = new Character();
        isDead = false;
        exp = 0;
        c.massLv = 1;
    }
}
