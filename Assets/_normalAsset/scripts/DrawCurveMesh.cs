using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 注意：传入的list的key取值要在0-1中！！！
/// </summary>
namespace YZY_Tool
{
    public class DrawCurveMesh : MonoBehaviour
    {
        #region Parameters
        //每2个定点为一组，此值代表有多少组
        int count = 60;
        //每两组顶点的间隔距离，此值越小曲线越平滑
        const float pointdis = 0.2f;

        MeshFilter filter;
        Mesh mesh;
        MeshRenderer mr;
        public AnimationCurve curve;
        public CurveType curveType;
        public float maxHeight = 1;
        private List<Vector2> m_points;
        public Material mat;
        //MeshCollider mc;
        #endregion

        #region Properties
        //曲线

        public MeshFilter Filter
        {
            get
            {
                if (filter == null)
                {
                    filter = GetComponent<MeshFilter>();
                    if (filter == null)
                    {
                        Mr.material= mat;
                        filter =gameObject.AddComponent<MeshFilter>();
                    }

                }
                return filter;
            }

            set
            {
                filter = value;
            }
        }

        public MeshRenderer Mr
        {
            get
            {
                if (mr == null)
                {
                    mr = GetComponent<MeshRenderer>();
                    if (mr == null)
                    {
                        mr = gameObject.AddComponent<MeshRenderer>();
                    }
                }
                return mr;
            }

            set
            {
                mr = value;
            }
        }

        public Mesh Mesh
        {
            get
            {
                if(mesh==null)
                    mesh= Filter.mesh;
                return mesh;
            }

            set
            {
                mesh = value;
            }
        }
        #endregion

        #region Utility Methods
        public void SetCurve(List<Vector2> list)
        {
            //var _curve = new AnimationCurve();            
            switch (curveType)
            {
                case CurveType.Linear:
                    if (list!=null&&list.Count > 0)
                    {
                        curve = AnimationCurve.Linear(0, list[0].y, 1, list[list.Count - 1].y);
                        for (var i = 0; i < list.Count; i++)
                        {
                            curve.AddKey(list[i].x, list[i].y);
                        }
                    }
                    else
                    {
                        curve = AnimationCurve.Linear(0, 0, 1, 0);
                        for (var i = 0; i < 60; i++)
                        {
                            curve.AddKey(0, 0);
                        }
                    }
                    break;
                case CurveType.Ease:
                    if (list != null&&list.Count > 0)
                    {
                        curve = AnimationCurve.EaseInOut(0, list[0].y, 1, list[list.Count - 1].y);
                        for (var i = 0; i < list.Count; i++)
                        {
                            curve.AddKey(list[i].x, list[i].y);
                        }
                    }
                    else
                    {
                        curve = AnimationCurve.Linear(0, 0, 1, 0);
                        for (var i = 0; i <60; i++)
                        {
                            curve.AddKey(0, 0);
                        }
                    }
                    break;
            }
            DrawSquare(curve);
        }
        [ContextMenu("Draw")]
        void DrawSquare()
        {
            DrawSquare(curve);
        }


        #endregion

        #region Private Methods 
        
        void DrawSquare(AnimationCurve animCurve)
        {
            //创建mesh
            //Mesh mesh = gameObject.AddComponent<MeshFilter>().mesh;
            Mesh.Clear();

            //定义顶点列表
            List<Vector3> pointList = new List<Vector3>();
            //uv列表
            List<Vector2> uvList = new List<Vector2>();
            //第一列的前2个点直接初始化好
            pointList.Add(new Vector3(0, 0, 0));
            pointList.Add(new Vector3(0, animCurve.Evaluate(0) * maxHeight, 0));

            //设置前2个点的uv
            uvList.Add(new Vector2(0, 0));
            uvList.Add(new Vector2(0, 1));

            //三角形数组
            List<int> triangleList = new List<int>();
            //count = animCurve.length;
            for (int i = 0; i < count; i++)
            {
                //计算当前列位于什么位置
                float rate = (float)i / (float)(count);
                //计算当前的顶点
                pointList.Add(new Vector3((i + 1) * pointdis, 0, 0));

                //这里从曲线函数中取出当点的高度
                pointList.Add(new Vector3((i + 1) * pointdis, animCurve.Evaluate(rate)* maxHeight, 0));

                //uv直接使用rate即可
                uvList.Add(new Vector2(rate, 0));
                uvList.Add(new Vector2(rate, 1));

                //计算当前2个点与前面2个点组成的2个三角形
                int startindex = i * 2;
                triangleList.Add(startindex + 0);
                triangleList.Add(startindex + 1);
                triangleList.Add(startindex + 2);

                triangleList.Add(startindex + 3);
                triangleList.Add(startindex + 2);
                triangleList.Add(startindex + 1);
            }

            //把最终的顶点和三角形数组赋予mesh;
            Mesh.vertices = pointList.ToArray();
            Mesh.triangles = triangleList.ToArray();
            Mesh.uv = uvList.ToArray();
            Mesh.RecalculateNormals();
            
            //把mesh赋予MeshCollider
            Filter.sharedMesh = Mesh;
        }
      
        #endregion

        public enum CurveType
        {
            Linear,
            Ease,
        }
    }
}