using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    List<Weapon> weapons = new List<Weapon>();

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Alpha1))
        {
            ClearWeapon();
            weapons[0].gameObject.SetActive(true);
        }

        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            ClearWeapon();
            weapons[1].gameObject.SetActive(true);
        }

    }


    private void ClearWeapon()
    {
        foreach (var item in weapons)
        {
            item.gameObject.SetActive(false);
        }
    }
}
