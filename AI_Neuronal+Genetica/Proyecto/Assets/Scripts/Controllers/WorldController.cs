using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    [SerializeField]
    int population;

    [SerializeField]
    int population_plants;

    [SerializeField]
    int population_predator;

    [SerializeField]
    public GameObject prefab_Prey;

    [SerializeField]
    public GameObject prefab_Plant;

    [SerializeField]
    public GameObject prefab_Predator;

    [SerializeField]
    GameObject[] plants;

    List<Animal> total_animals;
    public List<Prey> total_preys;
    List<Predator> total_predators;

    [SerializeField]
    Terrain terrain;

    public int totalDeceases { get; private set; }

    Vector3 terrainSize;

    private static WorldController instance = null;
    private void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Game Instance Singleton
    public static WorldController Instance
    {
        get
        {
            return instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        total_animals = new List<Animal>();
        total_preys = new List<Prey>();
        total_predators = new List<Predator>();


        switch (TimeController.Instance.world_calendar.season)
        {
            case 0:
                population_plants = 200;
                break;
            case 1:
                population_plants = 160;
                break;
            case 2:
                population_plants = 130;
                break;
            case 3:
                population_plants = 90;
                break;
        }

        plants = new GameObject[population_plants];

        terrainSize = terrain.terrainData.size;

        for (int i = 0; i < population_plants; i++)
        {
            plants[i] = SpawnPlant(prefab_Plant);
        }

        SpawnPreys(population);
        SpawnPredators(population_predator);
      
    }

    // Update is called once per frame
    void Update()
    {
        switch (TimeController.Instance.world_calendar.season)
        {
            case 0:
                population_plants = 200;
                break;
            case 1:
                population_plants = 160;
                break;
            case 2:
                population_plants = 130;
                break;
            case 3:
                population_plants = 90;
                break;
        }

        for (int i = 0; i < population_plants; i++)
        {
            if (plants[i] == null)
                plants[i] = SpawnPlant(prefab_Plant);
        }

        if (total_preys.Count <= 10) SpawnPreys((int)(population * 0.5f));
        if (total_predators.Count <= 1) SpawnPredators((int)(population_predator * 0.5f));
    }
   

    public void SpawnPreys(int times)
    {
        for (int i = 0; i < times; i++)
        {
            GameObject animal = SpawnAnimal(prefab_Prey);

            animal.GetComponent<Animal>().RandomizeStats();
            animal.GetComponent<Animal>().Restart();

            total_animals.Add(animal.GetComponent<Animal>());
            total_preys.Add(animal.GetComponent<Prey>());

        }
    }

    public void SpawnPredators(int times)
    {
        for (int i = 0; i < times; i++)
        {
            GameObject animal = SpawnAnimal(prefab_Predator);

            animal.GetComponent<Animal>().RandomizeStats();
            animal.GetComponent<Animal>().Restart();

            total_animals.Add(animal.GetComponent<Animal>());
            total_predators.Add(animal.GetComponent<Predator>());
        }
    }

    GameObject SpawnAnimal(GameObject prefab_animal)
    {
        return Instantiate(prefab_animal, new Vector3(Random.Range(2, terrainSize.x), 0.1f, Random.Range(2, terrainSize.z)), new Quaternion(0,0,0,0),null);
    }

    GameObject SpawnPlant(GameObject prefab_plant)
    {
        return Instantiate(prefab_plant,new Vector3(Random.Range(2, terrainSize.x), 0.1f, Random.Range(2, terrainSize.z)), new Quaternion(0, 0, 0, 0), null);
    }

    GameObject SpawnPredator(GameObject prefab_plant)
    {
        return Instantiate(prefab_Predator, new Vector3(Random.Range(2, terrainSize.x), 0.1f, Random.Range(2, terrainSize.z)), new Quaternion(0, 0, 0, 0), null);
    }

    public List<Animal> get_animals()
    {
        return total_animals;
    }

    public void DestroyAnimal(Animal animal)
    {
        total_animals.Remove(animal);

        if(animal.gameObject.tag == "prey")
        {
            total_preys.Remove((Prey) animal);
        }
        else if(animal.gameObject.tag == "predator")
        {
            total_predators.Remove((Predator)animal);
        }

        Instantiate(animal.death_particles, animal.transform.position, animal.death_particles.transform.rotation, null);

        ++totalDeceases;
        Destroy(animal.gameObject);
    }

    public void AddAnimal(Animal animal)
    {
        total_animals.Add(animal);

        if (animal.gameObject.tag == "prey")
        {
            total_preys.Add((Prey)animal);
        }
        else if (animal.gameObject.tag == "predator")
        {
            total_predators.Add((Predator)animal);
        }

    }
}
