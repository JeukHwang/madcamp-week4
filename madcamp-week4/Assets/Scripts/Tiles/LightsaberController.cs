using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsaberController : MonoBehaviour
{
    public bool red;
    public bool green;
    public bool blue;
    public bool on = true;

    LightsaberWeapon weapon;

    // Start is called before the first frame update
    void Start()
    {
        weapon = gameObject.GetComponentInChildren<LightsaberWeapon>();
        updateProperty();
    }

    void updateProperty()
    {
        Debug.Log("prop!");
        if (on)
        {
            weapon.WeaponOn();
        }
        else
        {
            weapon.WeaponOff();
        }
        //weapon.bladeColor__ = new Color(red ? 1.0f : 0f, green ? 1.0f : 0f, blue ? 1.0f : 0f);
        //weapon.red = red;
        //weapon.green = green;
        //weapon.blue = blue;
        //weapon.InitializeBladeColor();
    }

    // Update is called once per frame
    void Update()
    {
        updateProperty();
    }
}
