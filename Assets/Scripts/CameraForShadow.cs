using UnityEngine;
using System.Collections;
namespace ShadowMapCaculate
{
    /// <summary>
    /// 创建depth相机
    /// by lijia
    /// </summary>
    public class CameraForShadow : MonoBehaviour
    {
        public static CameraForShadow shadowCamera;

        RenderTexture _rt;
        /// <summary>
        /// 光照的角度
        /// </summary>
     
        public Shader shader;
        void Awake()
        {
                shadowCamera=this;
        }

        void Start()
        {
           
           
           GetComponent<Camera>().SetReplacementShader(shader, "RenderType");
        }


        /// <summary>
        /// OnPostRender is called after a camera finishes rendering the scene.
        /// </summary>
     

    }
}