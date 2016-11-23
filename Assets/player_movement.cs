using UnityEngine;
using System.Collections;

public class player_movement : MonoBehaviour {

    public GameObject camera_go;
    public float walk_speed = 1f;
    public float strafe_speed = 1f;
    public float pitch_speed = 1f;
    public float yaw_speed = 1f;

	// Update is called once per frame
	void Update () {
        float dt = Time.deltaTime;

        Vector3 yaw = new Vector3(0, Input.GetAxis("Mouse X") * yaw_speed * dt, 0);
        Vector3 pitch = new Vector3(Input.GetAxis("Mouse Y") * pitch_speed * dt, 0, 0);

        transform.Rotate(yaw);
        camera_go.transform.Rotate(pitch);

        Vector3 move = new Vector3(Input.GetAxis("Horizontal") * strafe_speed * dt, 0, Input.GetAxis("Vertical") * walk_speed * dt);

        Quaternion quat = transform.localRotation;
        Vector3 rotated_move = quat * move;

        CharacterController cc = GetComponent<CharacterController>();
        cc.Move(rotated_move);

	}
}
