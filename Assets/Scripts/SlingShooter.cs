using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlingShooter : MonoBehaviour
{
    public CircleCollider2D Collider;
    public LineRenderer Trajectory;

    private Vector2 _startPos;
    private Bird _bird;

    [SerializeField]
    private float _radius = 0.75f;

    [SerializeField]
    private float _throwSpeed = 30f;

    // Start is called before the first frame update
    void Start()
    {
        _startPos = transform.position;
    }

    void OnMouseUp()
    {
        Collider.enabled = false;
        Vector2 velocity = _startPos - (Vector2)transform.position;
        float distance = Vector2.Distance(_startPos, transform.position);

        _bird.Shoot(velocity, distance, _throwSpeed);

        //Kembalikan ketapel ke posisi awal
        gameObject.transform.position = _startPos;
        Trajectory.enabled = false;
    }

    public void InitiateBird(Bird bird)
    {
        _bird = bird;
        _bird.MoveTo(gameObject.transform.position, gameObject);
        Collider.enabled = true;
    }

    void OnMouseDrag()
    {
        // Mengubah posisi mouse ke world position
        Vector2 p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        // Hitung supaya 'karet' ketapel berada dalam radius yang ditentukan
        Vector2 dir = p - _startPos;
        if (dir.sqrMagnitude > _radius)
            dir = dir.normalized * _radius;
        transform.position = _startPos + dir;

        float distance = Vector2.Distance(_startPos, transform.position);

        if (!Trajectory.enabled)
        {
            Trajectory.enabled = true;
        }

        DisplayTrajectory(distance);
    }

    void DisplayTrajectory(float distance)
    {
        if (_bird == null)
        {
            return;
        }

        Vector2 velocity = _startPos - (Vector2)transform.position;
        int segmentCount = 5;
        Vector2[] segments = new Vector2[segmentCount];

        // Posisi awal trajectoy merupakan posisi mouse dari player saat ini
        segments[0] = transform.position;

        // Velocity awal
        Vector2 segVelocity = velocity * _throwSpeed * distance;

        for (int i = 1; i < segmentCount; i++)
        {
            float elapsedTime = i * Time.fixedDeltaTime * 5;
            segments[i] = segments[0] + segVelocity * elapsedTime + 0.5f * Physics2D.gravity * Mathf.Pow(elapsedTime, 2);
        }

        Trajectory.positionCount = segmentCount;
        for (int i = 0; i < segmentCount; i++)
        {
            Trajectory.SetPosition(i, segments[i]);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
