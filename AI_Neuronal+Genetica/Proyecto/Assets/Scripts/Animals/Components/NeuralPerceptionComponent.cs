using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralPerceptionComponent : PerceptionComponent
{

    public List<Prey> preys_detected;
    public List<Predator> predators_detected;
    public List<Plant> plants_detected;

    private void Awake()
    {
        preys_detected = new List<Prey>();
        predators_detected = new List<Predator>();
        plants_detected = new List<Plant>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        switch(other.tag)
        {
            case "plant":
                plants_detected.Add(other.GetComponent<Plant>());
            break;

            case "prey":
                Prey other_prey = other.GetComponent<Prey>();
                Prey this_prey =  transform.parent.GetComponent<Prey>();
                if (this_prey.GetSex() != other_prey.GetSex() && other.gameObject != transform.parent.gameObject && other_prey.IsOldToReproduce() && this_prey.IsOldToReproduce())
                preys_detected.Add(other_prey);
            break;

            case "predator":
                predators_detected.Add(other.GetComponent<Predator>());
            break;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {


    }

    private void OnTriggerExit(Collider other)
    {
        RemoveFromList(other.gameObject);
    }

    public void RemoveFromList(GameObject other)
    {
        switch (other.tag)
        {
            case "plant":
                plants_detected.Remove(other.GetComponent<Plant>());
                break;

            case "prey":
                preys_detected.Remove(other.GetComponent<Prey>());
                break;

            case "predator":
                predators_detected.Remove(other.GetComponent<Predator>());
                break;
        }
    }

    public Prey GetClosestPrey()
    {
        Prey closestPrey = null;
        float distance = GetComponentInParent<Prey>().GetStats().visionRadius;

        foreach (Prey prey in preys_detected)
        {
            if (!prey) { preys_detected.Remove(prey); break; }

            float preyDistance = (transform.position - prey.transform.position).sqrMagnitude;
            if (distance >= preyDistance)
            {
                closestPrey = prey;
                distance = preyDistance;
            }
        }

        return closestPrey;
    }

    public Plant GetClosestPlant()
    {
        Plant closestPlant = null;
        float distance = GetComponentInParent<Prey>().GetStats().visionRadius;

        foreach (Plant plant in plants_detected)
        {
            if(!plant) { plants_detected.Remove(plant); break; }

            float plantDistance = (transform.position - plant.transform.position).sqrMagnitude;
            if (distance >= plantDistance)
            {
                closestPlant = plant;
                distance = plantDistance;
            }
        }

        return closestPlant;
    }

    public Predator GetClosestPredator()
    {
        Predator closestPredator = null;
        float distance = GetComponentInParent<Prey>().GetStats().visionRadius;

        foreach (Predator predator in predators_detected)
        {
            if (!predator) { predators_detected.Remove(predator); break; }

            float predatorDistance = (transform.position - predator.transform.position).sqrMagnitude;
            if (distance >= predatorDistance)
            {
                closestPredator = predator;
                distance = predatorDistance;
            }
        }

        return closestPredator;
    }
}
