using UnityEngine;

namespace Characters
{
    public class VFXTransforms : MonoBehaviour
    {
       [SerializeField] private Transform up;
       [SerializeField] private Transform center;
       [SerializeField] private Transform down;
       public Transform Up => up;
       public Transform Center => center;
       public Transform Down => down;
    }  
}

