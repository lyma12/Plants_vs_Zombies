using UnityEngine;

class CameraFollower : Singleton<CameraFollower>{
    private GameObject target;
    [SerializeField]
    [Range(0,1)] private float smooth;
    [SerializeField]
    private Vector3 offset;
    public GameObject Player{
        set{ target = value;}
    }

    private void FixedUpdate() {
        if(target != null){
            Vector3 p = new Vector3(target.transform.position.x, target.transform.position.y , target.transform.position.z) + offset;
            transform.position = Vector3.Lerp(transform.position, p, smooth);
        }
    }
}