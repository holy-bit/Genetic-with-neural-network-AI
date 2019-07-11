using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prey : Animal
{
    NeuralPerceptionComponent neuralPerceptionComponent;

    bool getCloser;
    float last_energy = 100;
    int last_population = 10;
    public float[] input = new float[9] { 0,0,0,0,0,0,0,0,0 };
    public float[] output_array = new float[4] {1,1,0,0};

    protected override void OnStart()
    {
        neuralPerceptionComponent = (NeuralPerceptionComponent) Perception_Component;
        base.OnStart();
        _renderer.GetPropertyBlock(_matPropertyBlock);
        
        Color finalColor = G_sex == Sex.MALE ? F_baseColor * 0.75f : F_baseColor * 1.25f;
        _matPropertyBlock.SetColor("_Color", finalColor);
        _renderer.SetPropertyBlock(_matPropertyBlock);

        transform.localScale = Vector3.one * G_sizeScale.value;

        Mesh_Agent_Component.speed = G_baseSpeed.value;
        Perception_Component.transform.localScale = Vector3.one * G_visionRadius.value;

        focused_element = null;
    }

    // Update is called once per frame
    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (Mesh_Agent_Component.isOnNavMesh)
        {
            UpdateMovementGoal();
            Mesh_Agent_Component.destination = Movement_Goal;
        }
    }

    protected override void UpdateDetection()
    {
        base.UpdateDetection();
        RequestFocusChange();
        
    }
    
    void UpdateMovementGoal()
    { 
        if (!focused_element)
        {
            AskNeuralNetworkForMovement();
            UpdateNoneFocused();
        }

        else
        {
            
            if (getCloser) Movement_Goal = focused_element.transform.position;
            else Movement_Goal = transform.position + (transform.position - focused_element.transform.position);
            if (focused_element.tag != "prey")
                AskNeuralNetworkForMovement();
        }
    }

    protected override void Eat(GameObject food)
    {
        base.Eat(food);

        energy += 20.0f;
        energy = Mathf.Clamp(energy, 0.0f, 100.0f);

        neuralPerceptionComponent.RemoveFromList(food);
        Destroy(food);
    }

    public override bool IsOldToReproduce()
    {
        bool canReproduce = lastTimeReproduced.get_Days(TimeController.Instance.world_time - lastTimeReproduced.world_moment) >= GlobalValues.PREY_DAYS_TO_ADULTHOOD;
        return canReproduce;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (focused_element)
        {
            switch (collision.gameObject.tag)
            {
                case "prey":
                    if (focused_element.tag == "prey")
                    {
                        SpawnOffspring(collision.gameObject.GetComponent<Prey>());
                        focused_element.GetComponent<Prey>().focused_element = null;
                        focused_element.GetComponentInChildren<PerceptionComponent>().object_detected = null;
                        focused_element = null;
                        Perception_Component.object_detected = null;
                    }
                break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "plant")
        {
            Eat(other.gameObject);
        }
    }

    void UpdateNoneFocused()
    {
        if (Mesh_Agent_Component.isOnNavMesh && Mesh_Agent_Component.remainingDistance <= 5.0f)
            Movement_Goal = transform.position + Quaternion.Euler(0, Random.Range(0, 360), 0) * transform.forward * 10.0f;
    }

    protected override void SpawnOffspring(Animal other)
    {
        int litter = Random.Range(1, GlobalValues.PREY_MAX_LITTER);

        for(int i = 0; i < litter; ++i)
        {
            GameObject animal = Instantiate(WorldController.Instance.prefab_Prey, transform.position + (other.transform.position - transform.position) * 0.5f, new Quaternion(0, 0, 0, 0), null);

            animal.GetComponent<Animal>().SetChromosomes(GeneticRecombination(other));
            animal.GetComponent<Animal>().Restart();

            if (currentGeneration > other.currentGeneration)
                animal.GetComponent<Animal>().currentGeneration = currentGeneration + 1;
            else
                animal.GetComponent<Animal>().currentGeneration = other.currentGeneration + 1;

            WorldController.Instance.AddAnimal(animal.GetComponent<Animal>());
        }

        Instantiate(love_particles, transform.position + (other.transform.position - transform.position) * 0.5f, love_particles.transform.rotation, null);

        lastTimeReproduced = TimeController.Instance.world_calendar;
        other.lastTimeReproduced = TimeController.Instance.world_calendar;
    }
    private void OnDestroy()
    {
        if(energy>0f)
        {
            setFear();
        }
    }

    public void setFear()
    {
        float[] output_P = { 0.1f, 0.1f, 0.9f, 0.1f };
        NeuralNetworkRabbits.instance.NetworkRetraining(input, output_P);
    }

    void AskNeuralNetworkForMovement()
    {
        // Distancia a planta: distanceToPlant / perception_component.detection_area.radius;
        // Distancia a presa: distanceToPrey / perception_component.detection_area.radius;
        // Distancia a depredador: distanceToPredator / perception_component.detection_area.radius;
        // Diferencia de tamaño: if(distanceToPredator != 0.0f) closestPredator.G_sizeScale > G_sizeScale ? 1.0f : 0.0f;
        // Energía: energy / 100.0f;
        // Es primavera: TimeController.Instance.world_calendar.season == 0 ? 1.0f : 0.0f;
        // Es verano:    TimeController.Instance.world_calendar.season == 1 ? 1.0f : 0.0f;
        // Es otoño:     TimeController.Instance.world_calendar.season == 2 ? 1.0f : 0.0f;
        // Es invierno:  TimeController.Instance.world_calendar.season == 3 ? 1.0f : 0.0f;


        if (last_energy < energy )
        {
            float[] output_P = { 0.6f ,0.2f, 0.5f, 0.9f };
            NeuralNetworkRabbits.instance.NetworkRetraining(input, output_P);
            
        }
        if(WorldController.Instance.total_preys.Count > last_population)
        {
            float[] output_P = { 0.9f, 0.1f, 0.5f, 0.9f };
            NeuralNetworkRabbits.instance.NetworkRetraining(input, output_P);
        }
        if (last_energy > 80f && energy <= 80f)
        {
            float[] output_P = { 0.1f, 0.1f, 0.5f, 0.9f };
            NeuralNetworkRabbits.instance.NetworkRetraining(input, output_P);
        }

        else if (last_energy > 60f && energy <= 60f)
        {
            float[] output_P = { 0.5f, 0.6f, 0.5f, 0.6f };
            NeuralNetworkRabbits.instance.NetworkRetraining(input, output_P);
        }

        else if (last_energy > 40f && energy <= 40f)
        {
            float[] output_P = { 0.6f, 0.7f, 0.5f, 0.3f };
            NeuralNetworkRabbits.instance.NetworkRetraining(input, output_P);
        }

        else if (last_energy > 20f && energy <= 20f)
        {
            float[] output_P = { 0.7f, 0.8f, 0.5f, 0.2f };
            NeuralNetworkRabbits.instance.NetworkRetraining(input, output_P);
        }

        else if (last_energy > 10f && energy <= 10f)
        {
            float[] output_P = { 0.9f, 0.9f, 0.5f, 0.1f };
            NeuralNetworkRabbits.instance.NetworkRetraining(input, output_P);
        }

        if (output_array[3] > output_array[1] && neuralPerceptionComponent.GetClosestPrey() == null)
        {
            float[] output_P = { 0.6f, 0.6f, 0.5f, 0.3f };
            NeuralNetworkRabbits.instance.NetworkRetraining(input, output_P);
        }
        if (input[2] < 0.8f && input[2]< input[1] && input[2] < input[3])
        {
            float[] output_P = { 0f, 0.2f, 0.9f, 0.1f };
            NeuralNetworkRabbits.instance.NetworkRetraining(input, output_P);
        }


        last_energy = energy;
        last_population = WorldController.Instance.total_preys.Count;

        float distance_plant = 0;
        float distance_predator = 0;
        float distance_Prey = 0;
        float scaleRelation = 0;

        if (neuralPerceptionComponent.GetClosestPlant()!=null)
        {
            distance_plant = (transform.position - neuralPerceptionComponent.GetClosestPlant().gameObject.transform.position).sqrMagnitude/ (GetComponentInChildren<Prey>().GetStats().visionRadius); ;
        }
          
        if (neuralPerceptionComponent.GetClosestPredator() != null)
        {
            distance_predator = (transform.position - neuralPerceptionComponent.GetClosestPredator().gameObject.transform.position).sqrMagnitude/ (GetComponentInChildren<Prey>().GetStats().visionRadius);
            scaleRelation = neuralPerceptionComponent.GetClosestPredator().GetStats().sizeScale > GetStats().sizeScale ? 1.0f : 0.0f;
        }
       

        if (neuralPerceptionComponent.GetClosestPrey() != null)
            distance_Prey = (transform.position - neuralPerceptionComponent.GetClosestPrey().gameObject.transform.position).sqrMagnitude/ (GetComponentInChildren<Prey>().GetStats().visionRadius);

        input = new float[]
        {
             1 - distance_plant, 1 - distance_predator, 1 - distance_Prey,
             scaleRelation,energy / 100.0f,
             TimeController.Instance.world_calendar.season == 0 ? 1.0f : 0.0f,
             TimeController.Instance.world_calendar.season == 1 ? 1.0f : 0.0f,
             TimeController.Instance.world_calendar.season == 2 ? 1.0f : 0.0f,
             TimeController.Instance.world_calendar.season == 3 ? 1.0f : 0.0f
        };
        
        NeuralNetwork.NetOutput output =  NeuralNetworkRabbits.instance.getAction(input);
        switch(output.id)
        {
            case 1:
                if (neuralPerceptionComponent.GetClosestPlant() != null) focused_element = neuralPerceptionComponent.GetClosestPlant().gameObject;
                else focused_element = null;
            break;

            case 2:
                if (neuralPerceptionComponent.GetClosestPredator() != null) focused_element = neuralPerceptionComponent.GetClosestPredator().gameObject;
                else focused_element = null;
            break;

            case 3:
                if (neuralPerceptionComponent.GetClosestPrey() != null)
                {
                    focused_element = neuralPerceptionComponent.GetClosestPrey().gameObject;
                    neuralPerceptionComponent.GetClosestPrey().gameObject.GetComponent<Prey>().focused_element = gameObject;
                }
                else focused_element = null; 
            break;

            default:
                focused_element = null;
                break;
        }
        getCloser = output.action;

        output_array = new float[]
        {
            NeuralNetwork.Instance.GetOutput(0),
            NeuralNetwork.Instance.GetOutput(1),
            NeuralNetwork.Instance.GetOutput(2),
            NeuralNetwork.Instance.GetOutput(3)
        };

        //print("action:" + NeuralNetwork.Instance.GetOutput(0).ToString() + "    Planta:" + NeuralNetwork.Instance.GetOutput(1).ToString() + "    Depredador:" + NeuralNetwork.Instance.GetOutput(2).ToString() + "    Presa:" + NeuralNetwork.Instance.GetOutput(3).ToString());

    }
}
