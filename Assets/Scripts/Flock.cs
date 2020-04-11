using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public BoidsSetting generalSettings;
    public GameObject attractor;
    public List<Obstacle> repellers;
    public List<Boids> flock;
    
    private void Start()
    {
        flock = new List<Boids>();
        GetListofBoids();
        for (int i = 0; i < flock.Count; i++)
        {
            flock[i].UpdateTarget(attractor);
            flock[i].UpdateSettings(generalSettings);
        }
    }

    private void Update()
    {
        MouseClickChangeTarget();
        for (int i = 0; i < flock.Count; i++)
        {
            flock[i].run(flock, repellers);
        }
    }



    public bool MouseClickChangeTarget()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Plane plane = new Plane(Vector3.up, 0);

            float distance;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out distance))
            {
                attractor.transform.position = ray.GetPoint(distance);
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    private void GetListofBoids()
    {
        Boids[] temp = gameObject.GetComponentsInChildren<Boids>();

        if (temp == null)
        {
            Debug.Log("Boid list empty.");
            return;
        }

        foreach (Boids b in temp)
        {
            if (b != null)
                flock.Add(b);
        }
    }

    public void AddBoid(Boids newBoid)
    {
        flock.Add(newBoid);
    }


}
