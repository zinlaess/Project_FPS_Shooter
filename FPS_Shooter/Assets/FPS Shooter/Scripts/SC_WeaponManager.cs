using UnityEngine;

public class SC_WeaponManager : MonoBehaviour
{
    public Camera playerCamera;
    public SC_Weapon primaryWeapon;
    public SC_Weapon secondaryWeapon;
    public Vector3 pos;

    [HideInInspector]
    public SC_Weapon selectedWeapon;

    void Start()
    {
        primaryWeapon.ActivateWeapon(true);
        secondaryWeapon.ActivateWeapon(false);
        selectedWeapon = primaryWeapon;
        primaryWeapon.manager = this;
        secondaryWeapon.manager = this;
    }

    void Update()
    {
        //Select secondary weapon when pressing 1
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            primaryWeapon.ActivateWeapon(false);
            secondaryWeapon.ActivateWeapon(true);
            selectedWeapon = secondaryWeapon;
        }

        //Select primary weapon when pressing 2
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            primaryWeapon.ActivateWeapon(true);
            secondaryWeapon.ActivateWeapon(false);
            selectedWeapon = primaryWeapon;
        }
    }
    // stopgap measure : fix with animations
    public void WeaponPosStabilizationDefault()
    {
        pos = this.transform.position;
        pos = new Vector3(pos.x, pos.y + 0.1f,pos.z);
        this.transform.position = pos;
    }
    public void WeaponPosStabilizationMov()
    {
        pos = this.transform.position;
        pos = new Vector3(pos.x, pos.y - 0.1f, pos.z);
        this.transform.position = pos;
    }
    public void WeaponPosStabilizationJump()
    {
        pos = this.transform.position;
        pos = new Vector3(pos.x, pos.y - 0.3f, pos.z);
        this.transform.position = pos;
    }
    public void WeaponPosStabilizationisGrounded()
    {
        pos = this.transform.position;
        pos = new Vector3(pos.x, pos.y + 0.3f, pos.z);
        this.transform.position = pos;
    }
}