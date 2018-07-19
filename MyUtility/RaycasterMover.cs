using UnityEngine;
using System.Collections;
using MyUtility;

public class RaycasterMover : MonoBehaviour
{
    //------------------------------------------------------------------------CONSTANTS:

    private const string LOG_TAG = "RaycasterMover";
    public bool VERBOSE = false;

    //---------------------------------------------------------------------------FIELDS:
    private Camera camer;
    public string desiredTag;
    private Ray ray;
    private RaycastHit hit;
    private GameObject hitObject = null;
    private Quaternion desiredRotation;
    private Vector3 xLock;
    private Vector3 yLock;
    private Vector3 zLock;
    private float xSnapThreshold;
    private float ySnapThreshold;
    private float zSnapThreshold;
    private bool lockX = false;
    private bool lockY = false;
    private bool lockZ = false;
    private bool enableRaycasterMover = true;
    

    public RaycasterMover(Camera cam)
    {
        camer = cam;
    }
    public RaycasterMover(Camera cam,Quaternion rot)
    {
        camer = cam;
        desiredRotation = rot;
    }
    public RaycasterMover(Camera cam, string tag, Quaternion rot)
    {
        camer = cam;
        desiredRotation = rot;
        desiredTag = tag;
    }

    //---------------------------------------------------------------------MONO METHODS:

    void Update()
    {
        //if we are disabled, return so our mouse button input is not triggered 
        //if (enableRaycasterMover == false)
        //return;
        //Get Object
        vLog("Balls!");
        if(Input.GetMouseButtonDown (0))
        {
            ray = camer.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast (ray,out hit))
            {
                if(desiredTag == null)
                {
                    hitObject = hit.collider.gameObject;
                }
                else
                {
                    if (hit.collider.tag == desiredTag)
                        hitObject = hit.collider.gameObject;
                }
                vLog("hit a " + hitObject.tag.ToString());

            }
        }
        //Hold Object
        if (hitObject)
        {
            if (Input.GetMouseButton(0))
            {
                vLog("moving object!");
                ray = camer.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (desiredRotation != null)
                        hitObject.transform.rotation = desiredRotation;

                    float x = hit.point.x;
                    float y = hit.point.y;
                    float z = hit.point.z;
                    if(lockX)
                    {
                        if(Vector3.Distance(new Vector3(hitObject.transform.position.x,0,0),xLock) >= xSnapThreshold)
                        {
                            x = xSnapThreshold;
                        }
                    }
                    if(lockY)
                    {
                        if (Vector3.Distance(new Vector3(0, hitObject.transform.position.y, 0), yLock) >= ySnapThreshold)
                        {
                            y = ySnapThreshold;
                        }
                    }
                    if(lockZ)
                    {
                        if (Vector3.Distance(new Vector3(0, 0,hitObject.transform.position.z), zLock) >= zSnapThreshold)
                        {
                            z = zSnapThreshold;
                        }
                    }
                    hitObject.transform.position = new Vector3(x,y,z); // maybe change in a sec
                }
            }
        }
        //Drop Object
        if(Input.GetMouseButtonDown(0))
        {
            hitObject = null;
        }
    }

    //--------------------------------------------------------------------------METHODS:
 
     // snapToX = (squared distance(object.tranform.position.x,xLock) < xSnapThreshold)
    /**
     * If the user wishes to preserve an axis value of an object they can also use this stuff to lock it
     * */

     ///<summary>
     ///Locks the movement along the x-axis given an axis value to 
     ///lock to and a threshold within which the object begins to snap.
     ///Also used to preserve the x axis value of an object, given the 
     ///transform's x.
     ///</summary>
    public void SetXLock(float val,float snapThreshold)
    {
        vLog("X axis locked, lock value = " + val + "snapThreshold = "+ snapThreshold);
        xSnapThreshold = snapThreshold;
        lockX = true;
        xLock = new Vector3(val,0,0);
    }

    /// <summary>
    ////// Locks the movement along the y-axis given an axis value to 
    ///lock to and a threshold within which the object begins to snap.
    ///Also used to preserve the x axis value of an object, given the 
    ///transform's y.
    /// </summary>
    /// <param name="val"></param>
    /// <param name="snapThreshold"></param>
    public void SetYLock(float val, float snapThreshold)
    {
        vLog("Y axis locked, lock value = " + val + "snapThreshold = " + snapThreshold);
        ySnapThreshold = snapThreshold;
        lockY = true;
        yLock = new Vector3(0, val, 0);
    }
    /// <summary>
    /// Locks the movement along the z-axis given an axis value to 
    ///lock to and a threshold within which the object begins to snap.
    ///Also used to preserve the x axis value of an object, given the 
    ///transform's z.
    /// </summary>
    /// <param name="val"></param>
    /// <param name="snapThreshold"></param>
    public void SetZLock(float val, float snapThreshold)
    {
        vLog("Z axis locked, lock value = " + val + "snapThreshold = " + snapThreshold);
        zSnapThreshold = snapThreshold;
        lockZ = true;
        zLock = new Vector3(0,0,val);
    }

    /// <summary>
    /// Unlocks a given axis
    /// </summary>
    /// <param name="axis"></param>
    public void UnlockAxis(string axis)
    {
        if (axis.ToLower() == "x")
            lockX = false;
        else if (axis.ToLower() == "y")
            lockY = false;
        else if (axis.ToLower() == "z")
            lockZ = false;
        else
            vLog("Unable to unlock axis given input: "+axis);
    }
    ///<summary>
    /// Enables the RaycasterMover
    ///</summary>
    public void Enable()
    {
        vLog("enabled");
        enableRaycasterMover = true;
    }
    ///<summary>
    /// Disables the RaycasterMover
    ///</summary>
    public void Disable()
    {
        vLog("disabled");
        enableRaycasterMover = false;
    }
    //--------------------------------------------------------------------------HELPERS:

    private void vLog(string message)
    {
        if (VERBOSE) LOG_TAG.TPrint(message);
    }
}