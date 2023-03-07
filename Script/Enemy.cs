using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "Enemy", menuName = "Create/new Enemy")]
public class Enemy// : ScriptableObject
{
    public int id;
    public Character c;
    public Char type;
    public bool isDead;
    public Stack<Vector2> path;
    public float power
    {
        get
        {
            return c.mass * c.scaler;
        }
    }
    public Enemy(int id)
    {
        this.id = id;
        c = new Character();
        path = new Stack<Vector2>();
        isDead = false;
    }
}
