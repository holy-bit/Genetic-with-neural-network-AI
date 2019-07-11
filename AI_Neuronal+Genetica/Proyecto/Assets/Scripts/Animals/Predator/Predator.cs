using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Predator : Animal
{


    protected override void OnStart()
    {
        base.OnStart();
        _renderer.GetPropertyBlock(_matPropertyBlock);

        Color finalColor = G_sex == Sex.MALE ? F_baseColor * 0.75f : F_baseColor * 1.25f;
        _matPropertyBlock.SetColor("_Color", finalColor);
        _renderer.SetPropertyBlock(_matPropertyBlock);

        transform.localScale = Vector3.one * G_sizeScale.value;

        Mesh_Agent_Component.speed = G_baseSpeed.value;
        Perception_Component.transform.localScale = Vector3.one * G_visionRadius.value;

    }
    
    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (Mesh_Agent_Component.isOnNavMesh)
        {
            if (focused_element)
                Mesh_Agent_Component.destination = focused_element.transform.position;
            else
                Mesh_Agent_Component.destination = Movement_Goal;
        }

    }

    protected override void UpdateDetection()
    {
        base.UpdateDetection();
        RequestFocusChange();
    }
    

    protected override void Eat(GameObject food)
    {
        base.Eat(food);

        energy += 50.0f;
        energy = Mathf.Clamp(energy, 0.0f, 100.0f);

        WorldController.Instance.DestroyAnimal(food.GetComponent<Animal>());

        Collider[] preysInsideCollider = Physics.OverlapSphere(transform.position, transform.localScale.x * G_visionRadius.value, LayerMask.GetMask("Preys"));

        if (preysInsideCollider.Length >= 0)
        {
            float distanceToPrey = transform.localScale.x * G_visionRadius.value;
            foreach (Collider prey in preysInsideCollider)
            {
                if ((prey.gameObject.transform.position - transform.position).magnitude > transform.localScale.x && distanceToPrey >= (prey.gameObject.transform.position - transform.position).magnitude)
                {
                    distanceToPrey = (prey.gameObject.transform.position - transform.position).magnitude;
                    focused_element = null;
                    RequestFocusChange(prey.gameObject);
                }
            }
        }
    }
    public override bool IsOldToReproduce()
    {
        bool canReproduce = lastTimeReproduced.get_Days(TimeController.Instance.world_time - lastTimeReproduced.world_moment) >= GlobalValues.PREDATOR_DAYS_TO_ADULTHOOD;

        return canReproduce;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (focused_element)
        {
            switch (collision.gameObject.tag)
            {
                case "prey":

                    if (IsStronger(collision.gameObject.GetComponent<Animal>()) || Random.Range(0, 10) <= 0)
                    {
                        Eat(collision.gameObject);
                    }

                    else
                    {
                        collision.gameObject.GetComponent<Prey>().setFear();
                        _rigidbody.AddForce((transform.position - collision.transform.position).normalized * 10.0f, ForceMode.Impulse);
                        collision.gameObject.GetComponent<Animal>()._rigidbody.AddForce((collision.transform.position - transform.position).normalized * 10.0f, ForceMode.Impulse);

                        collision.gameObject.GetComponent<Animal>().focused_element = null;

                        Collider[] preysInsideCollider = Physics.OverlapSphere(transform.position, transform.localScale.x * G_visionRadius.value, LayerMask.GetMask("Preys"));

                        if (preysInsideCollider.Length >= 0)
                        {
                            float distanceToPrey = transform.localScale.x * G_visionRadius.value;
                            foreach (Collider prey in preysInsideCollider)
                            {
                                if ((prey.gameObject.transform.position - transform.position).magnitude > transform.localScale.x && distanceToPrey >= (prey.gameObject.transform.position - transform.position).magnitude)
                                {
                                    if(prey.gameObject != collision.gameObject)
                                    {
                                        distanceToPrey = (prey.gameObject.transform.position - transform.position).magnitude;
                                        //focused_element = null;
                                        RequestFocusChange(prey.gameObject);
                                    }
                                }

                                else
                                    focused_element = null;

                            }
                        }
                    }


                    break;

                case "predator":
                    if (focused_element.tag == "predator")
                    {
                        SpawnOffspring(collision.gameObject.GetComponent<Predator>());
                        focused_element.GetComponent<Predator>().focused_element = null;
                        focused_element.GetComponentInChildren<PerceptionComponent>().object_detected = null;
                        focused_element = null;
                        Perception_Component.object_detected = null;
                    }
                    break;

                default:
                    break;
            }

        }
    }

    protected override void RequestFocusChange(GameObject other_focused_element = null)
    {
        GameObject new_focused_element;

        if (!other_focused_element)
            new_focused_element = Perception_Component.object_detected;
        else new_focused_element = other_focused_element;

        if (new_focused_element != focused_element)
        {
            if (!focused_element)
            {
                try
                {

                    switch (new_focused_element.tag)
                    {
                        case "prey":
                            if (DetectionCheck(new_focused_element.GetComponent<Prey>()))
                            {
                                focused_element = new_focused_element;
                                OnFoodFocused(new_focused_element);
                            }
                            break;

                        case "predator":

                            if (GetSex() == Sex.MALE && new_focused_element.GetComponent<Predator>().GetSex() == Sex.FEMALE &&
                                IsOldToReproduce() && new_focused_element.GetComponent<Predator>().IsOldToReproduce() &&
                                (TimeController.Instance.world_calendar.season == GlobalValues.PREDATOR_RUT_SEASON ? true : (Random.Range(0, 100) <= GlobalValues.PREDATOR_PROBABILITY_TO_HAVE_CHILDREN)))
                            {
                                focused_element = new_focused_element;
                                focused_element.GetComponent<Predator>().focused_element = gameObject;
                                OnPredatorFocused(new_focused_element);
                                focused_element.GetComponent<Predator>().OnPredatorFocused(gameObject);
                            }


                            break;
                    }
                }
                catch (MissingReferenceException e) { }
            }

            else
            {
                if (new_focused_element)
                {
                    switch (focused_element.tag)
                    {
                        case "prey":
                            switch (new_focused_element.tag)
                            {
                                case "prey":
                                    if (DetectionCheck(new_focused_element.GetComponent<Prey>()))
                                    {
                                        focused_element = new_focused_element;
                                        OnFoodFocused(new_focused_element);
                                    }
                                    break;

                                case "predator":

                                    if (GetSex() == Sex.MALE && new_focused_element.GetComponent<Predator>().GetSex() == Sex.FEMALE &&
                                        IsOldToReproduce() && new_focused_element.GetComponent<Predator>().IsOldToReproduce() &&
                                        (TimeController.Instance.world_calendar.season == GlobalValues.PREDATOR_RUT_SEASON ? true : (Random.Range(0, 100) <= GlobalValues.PREDATOR_PROBABILITY_TO_HAVE_CHILDREN)))
                                    {
                                        focused_element = new_focused_element;
                                        focused_element.GetComponent<Predator>().focused_element = gameObject;
                                        OnPredatorFocused(new_focused_element);
                                        focused_element.GetComponent<Predator>().OnPredatorFocused(gameObject);
                                    }


                                    break;
                            }
                            break;
                    }
                }
            }
        }

        if (!focused_element) OnNoneFocused();

    }

    void OnFoodFocused(GameObject food)
    {
    }

    void OnPredatorFocused(GameObject predator)
    {

    }

    public void OnPreyFocused(GameObject prey)
    {
    }

    void OnNoneFocused()
    {
        if (Mesh_Agent_Component.isOnNavMesh && Mesh_Agent_Component.remainingDistance <= 1.0f)
            Movement_Goal = transform.position + Quaternion.Euler(0, Random.Range(0, 360), 0) * transform.forward * 50.0f;
    }

    protected override void SpawnOffspring(Animal other)
    {
        int litter = Random.Range(1, GlobalValues.PREDATOR_MAX_LITTER);

        for (int i = 0; i < litter; ++i)
        {
            GameObject animal = Instantiate(WorldController.Instance.prefab_Predator, transform.position + (other.transform.position - transform.position) * 0.5f, new Quaternion(0, 0, 0, 0), null);

            animal.GetComponent<Animal>().SetChromosomes(GeneticRecombination(other));
            animal.GetComponent<Animal>().Restart();

            if (currentGeneration > other.currentGeneration)
                animal.GetComponent<Animal>().currentGeneration = currentGeneration + 1;
            else
                animal.GetComponent<Animal>().currentGeneration = other.currentGeneration + 1;

            WorldController.Instance.AddAnimal(animal.GetComponent<Animal>());
        }

        lastTimeReproduced = TimeController.Instance.world_calendar;
        other.lastTimeReproduced = TimeController.Instance.world_calendar;
    }
}
