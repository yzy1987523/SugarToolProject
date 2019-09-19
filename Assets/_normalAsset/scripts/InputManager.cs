using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// </summary>
public class InputManager : MonoBehaviour
{
    #region Parameters
    public LayerMask layer;
    Timer timer;
    float pressTime = 0.2f;
    float x;
    float y;
    private Quaternion tempRot;
    public float rotSpeed=10;
    public float yMinLimit=-89;
    public float yMaxLimit=89;
    private float distance;
    public float scrollSpeed=1;
    public float zoomMin=-10;
    public float zoomMax=10;
    #endregion
    #region Properties
    public Timer Timer
    {
        get
        {
            if (timer == null)
            {
                timer = new Timer(pressTime);
            }
            return timer;
        }

        set
        {
            timer = value;
        }
    }
    #endregion
    #region Private Methods
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RayCheck(Input.mousePosition);
            Timer.InitTime();
        }
        else if (Input.GetMouseButton(0))
        {
            //需要按住一点时间才会反馈，这样避免操作的突然感和误操作
            if (Timer.TimeIsEnd())
            {
                CheckRot();
            }
        }
        CheckScroll();
    }
    
    void RayCheck(Vector3 _pos)
    {
        //在点击处发射射线，返回检测到的对象
        Ray ray = Camera.main.ScreenPointToRay(_pos);
        RaycastHit hitInfo;
        //Debug.DrawLine(ray.origin, ray.direction*3+ ray.origin);
        if (Physics.Raycast(ray, out hitInfo, 1000, layer))
        {
            //划出射线，只有在scene视图中才能看到

            GameObject gameObj = hitInfo.collider.gameObject;
            //Debug.Log(gameObj.name);
            var _com = gameObj.GetComponent<ISelectable>();
            if (_com != null)
            {
                _com.Select();
            }
        }
    }
    void CheckRot()
    {
        y -= Input.GetAxis("Mouse Y") * rotSpeed;
        x += Input.GetAxis("Mouse X") * rotSpeed;
        y = y.ClampAngle(yMinLimit, yMaxLimit);
        tempRot = Quaternion.Euler(y, x, 0);
        CamCtrl._instance.Rot(tempRot);
    }
    void CheckScroll()
    {
        if (Mathf.Abs( Input.GetAxis("Mouse ScrollWheel"))>0.05f)
        {
            distance = Mathf.Clamp(distance-Input.GetAxis("Mouse ScrollWheel") * scrollSpeed, zoomMin , zoomMax);
            CamCtrl._instance.Scroll(distance);
        }
    }

   
   
    #endregion
    #region Utility Methods
    #endregion







}
