using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boids : MonoBehaviour
{
    private BoidsSetting settings;
    private GameObject target;

    private Vector3 velocity = Vector3.zero;
    private Vector3 desiredVelocity;
    private Vector3 steer;
    private Vector3 ahead;
    private Vector3 ahead2;
    
    private void Update()
    {
        Debug.DrawRay(transform.position, velocity.normalized * 2, Color.green);
        Debug.DrawRay(transform.position, desiredVelocity.normalized * 2, Color.magenta);
    }
    
    public void UpdateSettings(BoidsSetting newSettings)
    {
        settings = newSettings;
    }

    public void UpdateTarget(GameObject newTarget)
    {
        target = newTarget;
    }

    public void run(List<Boids> boid, List<Obstacle> obstacleList)
    {
        steer += seek(target.transform.position, false);
        steer += align(boid);
        steer += cohesion(boid);
        steer -= separate(boid);
        Debug.Log(steer);
        //steer += collisionAvoidance(obstacleList);
        
        steer = Vector3.ClampMagnitude(steer, settings.mForce);
        steer /= settings.mass;              // acceleration = Force/mass
        
        velocity = Vector3.ClampMagnitude(velocity + steer, settings.mVelocity);

        transform.position += velocity * Time.deltaTime; // new pos = current pos + added distance (v*t = d)
        this.transform.rotation = Quaternion.LookRotation(velocity);
    }
    
    private Vector3 seek(Vector3 target, bool slowdown)
    {
        Vector3 steer; //steering vector
        Vector3 desired = target - this.transform.position;
        float d = desired.magnitude;

        if (d > 0)
        {
            desired.Normalize();
            if ((slowdown) && (d < 100.0f)) // either by distance
            {
                desired *= (settings.mVelocity * d / 100.0f);
            }
            else // or by speed
            {
                desired *= settings.mVelocity;
            }
            // steering = desired minus velocity
            //target = desired - velocity;
            //steer = target;
            steer = desired - velocity;

            //steer = Vector3.ClampMagnitude(steer, settings.mForce);
        }
        else
        {
            steer = new Vector3(0, 0);
        }
        return steer;
    }

    private bool avoidCircle(Obstacle obstacle)
    {
        return (Vector3.Distance(obstacle.center, ahead) <= obstacle.radius || Vector3.Distance(obstacle.center, ahead2) <= obstacle.radius);
    }

    private Vector3 collisionAvoidance(List<Obstacle> obstacleList)
    {
        ahead = transform.position + Vector3.Normalize(velocity) * settings.mAhead;
        ahead2 = transform.position + Vector3.Normalize(velocity) * settings.mAhead * 0.5f;

        Obstacle mostThreatening = FindMostThreateningObstacle(obstacleList);
        Vector3 avoidance = new Vector3(0, 0, 0);

        if (mostThreatening != null)
        {
            avoidance.x = ahead.x - mostThreatening.center.x;
            avoidance.y = ahead.y - mostThreatening.center.y;

            avoidance = Vector3.Normalize(avoidance) * settings.mAvoidForce;
        }
        else
        {
            avoidance = avoidance * 0;
        }

        return avoidance;
    }
    
    private Obstacle FindMostThreateningObstacle(List<Obstacle> obstacles)
    {
        Obstacle mostThreatening = null;

        for (int i = 0; i < obstacles.Count; i++)
        {
            if (avoidCircle(obstacles[i]) && 
                (mostThreatening == null || 
                Vector3.Distance(transform.position, obstacles[i].center) < Vector3.Distance(transform.position, mostThreatening.center)))
                {
                    mostThreatening = obstacles[i];
                }
        }
        return mostThreatening;
    }

    // Boid Rules
    // Alignment
    private Vector3 align(List<Boids> boids)
    {
        Vector3 sum = new Vector3(0, 0, 0);
        int count = 0;

        for (int i = 0; i < boids.Count; i++)
        {
            Boids other = boids[i];
            float d = Vector3.Distance(this.transform.position, other.transform.position);
            if ((d > 0) && (d < settings.DetectionRadius))
            {
                sum += other.transform.position; //adds location
                count++;
            }
        }
        if (count > 0)
        {
            sum /= (float)count;
            sum = Vector3.ClampMagnitude(sum, settings.mForce);
        }
        return sum;
    }
    
    // Separation
    private Vector3 separate(List<Boids> boids)
    {
        Vector3 sum = new Vector3(0, 0, 0);
        int count = 0;

        // checks every boid in the system if they're too close
        for (int i = 0; i < boids.Count; i++)
        {
            Boids other = boids[i];
            float d = Vector3.Distance(this.transform.position, other.transform.position);
            if ((d > 0) && (d < settings.SeparationRadius))
            {
                Vector3 diff = this.transform.position - other.transform.position;
                diff.Normalize();
                diff /= d;      // weight by distance
                sum += diff;
                count++;
            }
        }
        if (count > 0) // average
        {
            sum /= (float)count;
        }
        return sum * settings.SeparationForce;
    }
    
    // Cohesion
    private Vector3 cohesion(List<Boids> boids)
    {
        Vector3 sum = new Vector3(0, 0, 0);
        int count = 0;

        // checks every boid in the system if they're too close
        for (int i = 0; i < boids.Count; i++)
        {
            Boids other = boids[i];
            float d = Vector3.Distance(this.transform.position, other.transform.position);
            if ((d > 0) && (d < settings.DetectionRadius))
            {
                sum += other.transform.position;
                count++;
            }
        }
        if (count > 0) // average
        {
            sum /= (float)count;
            return seek(sum, false);
        }
        return sum;
    }
}