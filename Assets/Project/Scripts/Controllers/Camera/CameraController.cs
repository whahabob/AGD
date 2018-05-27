using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    public Transform target;
    public float smoothTime = 0.3f;
	public float xOffset, yOffset, zOffset;
    private Vector3 velocity = Vector3.zero;

	public float maxShake;
	public float shakeIncrement;
	public float shake;
	private Vector3 offset;

	// Use this for initialization
	private void Start()
	{
		offset = Vector3.zero;
	}

    void Update()
    {
        if (!target)
            target = GameObject.FindGameObjectWithTag("Player").transform;

		Vector3 goalPos = target.position + new Vector3(xOffset, yOffset, zOffset); goalPos.y = transform.position.y;
		Vector3 newPos = Vector3.SmoothDamp(transform.position, goalPos, ref velocity, smoothTime);

		offset = new Vector3 (Random.Range (-shake, shake), Random.Range (-shake, shake), 0);
		transform.position = newPos + offset;

		shake = Mathf.Clamp (shake - shakeIncrement, 0, maxShake);
    }
}