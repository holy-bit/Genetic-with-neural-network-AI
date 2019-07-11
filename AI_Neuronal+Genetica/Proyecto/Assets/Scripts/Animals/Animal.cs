using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Animal : MonoBehaviour
{
    // public enum ANIMAL_STATE

    // GENES
    protected Gene G_strength;
    protected Gene G_baseSpeed;
    protected Gene G_sizeScale;
    protected Gene G_visionRadius;
    protected Gene G_camouflage;
    protected Sex G_sex;

    // CHROMOSOMES
    Chromosome C_size;
    Chromosome C_movility;
    Chromosome C_perception;
    Chromosome C_stealth;

    // FENOTYPE

    [SerializeField] protected Color F_baseColor;


    // DEBUG
    public int currentGeneration { get; set; }

    // ANIMAL COMPONENTS
    public Rigidbody _rigidbody;
    protected NavMeshAgent Mesh_Agent_Component;
    protected PerceptionComponent Perception_Component;
    protected Vector3 Movement_Goal;
    protected Animal_State current_state;
    protected MaterialPropertyBlock _matPropertyBlock;
    protected Renderer _renderer;
    protected Animal_Stats stats;
    [SerializeField] protected GameObject selected_indicator;
    [SerializeField] protected GameObject best_indicator;
    TimeController.Calendar birthday;

    [SerializeField] public GameObject death_particles;
    [SerializeField] protected GameObject love_particles;

    protected float totalGeneticEnergyConsumption;

    GameObject target;
    protected float energy;
    float energy_timer;

    public GameObject focused_element;

    // NATURAL VARIABLES
    public TimeController.Calendar lastTimeReproduced { get; set; }

    /*
    public Animal()
    {
        //Mesh_Agent_Component = GetComponent<NavMeshAgent>();
    }
    */

    private void Awake()
    {
        G_strength = new Gene(10.0f, GlobalValues.MIN_STRENGTH, GlobalValues.MAX_STRENGTH, GlobalValues.STRENGTH_CONSUMPTION_FACTOR);
        G_baseSpeed = new Gene(10.0f, GlobalValues.MIN_BASE_SPEED, GlobalValues.MAX_BASE_SPEED, GlobalValues.SPEED_CONSUMPTION_FACTOR);
        G_sizeScale = new Gene(1.0f, GlobalValues.MIN_SIZE_SCALE, GlobalValues.MAX_SIZE_SCALE, GlobalValues.SIZE_SCALE_CONSUMPTION_FACTOR);
        G_visionRadius = new Gene(10.0f, G_sizeScale.value, GlobalValues.MAX_VISION_RADIUS, GlobalValues.VISION_RADIUS_CONSUMPTION_FACTOR);
        G_camouflage = new Gene(10.0f, GlobalValues.MIN_CAMOUFLAGE, GlobalValues.MAX_CAMOUFLAGE, GlobalValues.CAMOUFLAGE_CONSUMPTION_FACTOR);
        G_sex = Sex.FEMALE;

        C_size = new Chromosome(new List<Gene>() { G_sizeScale, G_strength });
        C_movility = new Chromosome(new List<Gene>() { G_baseSpeed });
        C_perception = new Chromosome(new List<Gene>() { G_visionRadius });
        C_stealth = new Chromosome(new List<Gene>() { G_camouflage });

        //food = new List<GameObject>();

        Mesh_Agent_Component = GetComponent<NavMeshAgent>();
        Perception_Component = GetComponentInChildren<PerceptionComponent>();
        _matPropertyBlock = new MaterialPropertyBlock();
        _renderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();

        energy = 100;
        current_state = Animal_State.RESTING;

        birthday = TimeController.Instance.world_calendar;

    }

    // Start is called before the first frame update
    void Start()
    {

        lastTimeReproduced = TimeController.Instance.world_calendar;

        OnStart();
    }

    // Update is called once per frame
    void Update()
    {
        OnUpdate();
        UpdateDetection();


    }

    protected virtual void OnStart()
    {
        stats = new Animal_Stats(G_strength.value, G_baseSpeed.value, G_sizeScale.value, G_visionRadius.value, G_camouflage.value, G_sex);

        totalGeneticEnergyConsumption = G_baseSpeed.value * G_baseSpeed.energyConsumptionFactor +
                                        G_sizeScale.value * G_sizeScale.energyConsumptionFactor +
                                        G_visionRadius.value * G_visionRadius.energyConsumptionFactor +
                                        G_camouflage.value * G_camouflage.energyConsumptionFactor;

        C_size = new Chromosome(new List<Gene>() { G_sizeScale, G_strength });
        C_movility = new Chromosome(new List<Gene>() { G_baseSpeed });
        C_perception = new Chromosome(new List<Gene>() { G_visionRadius });
        C_stealth = new Chromosome(new List<Gene>() { G_camouflage });
    }

    protected virtual void OnUpdate()
    {
        energy_timer += Time.deltaTime;
        if (energy_timer > 1)
        {
            DecreaseEnergy(totalGeneticEnergyConsumption * energy_timer);
            energy_timer = 0;
            if (energy <= 0) WorldController.Instance.DestroyAnimal(this);
        }

    }

    protected virtual void UpdateDetection() { }
    public virtual bool IsOldToReproduce() { return false; }

    protected virtual void Eat(GameObject food)
    {
    }

    public void RandomizeStats()
    {
        G_baseSpeed = new Gene(Random.Range(1.0f, 10.0f), GlobalValues.MIN_BASE_SPEED, GlobalValues.MAX_BASE_SPEED, GlobalValues.SPEED_CONSUMPTION_FACTOR);
        G_sizeScale = new Gene(Random.Range(0.5f, 3.0f), GlobalValues.MIN_SIZE_SCALE, GlobalValues.MAX_SIZE_SCALE, GlobalValues.SIZE_SCALE_CONSUMPTION_FACTOR);
        G_strength = new Gene(G_sizeScale.value * 25, GlobalValues.MIN_STRENGTH, GlobalValues.MAX_STRENGTH, GlobalValues.STRENGTH_CONSUMPTION_FACTOR);
        G_visionRadius = new Gene(Random.Range(1.0f, 20.0f), G_sizeScale.value, GlobalValues.MAX_VISION_RADIUS, GlobalValues.VISION_RADIUS_CONSUMPTION_FACTOR);
        G_camouflage = new Gene(Random.Range(0.0f, 100.0f), GlobalValues.MIN_CAMOUFLAGE, GlobalValues.MAX_CAMOUFLAGE, GlobalValues.CAMOUFLAGE_CONSUMPTION_FACTOR);
        G_sex = Random.Range(0, 10) <= 4 ? Sex.MALE : Sex.FEMALE;

        C_size = new Chromosome(new List<Gene>() { G_sizeScale, G_strength });
        C_movility = new Chromosome(new List<Gene>() { G_baseSpeed });
        C_perception = new Chromosome(new List<Gene>() { G_visionRadius });
        C_stealth = new Chromosome(new List<Gene>() { G_camouflage });
    }

    public void DecreaseEnergy(float energyDecrease)
    {
        energy -= energyDecrease;
    }

    public int GetDaysOld()
    {
        return TimeController.Instance.world_calendar.get_Days(TimeController.Instance.world_calendar - birthday);
    }

    protected virtual void RequestFocusChange(GameObject other_focused_element = null)
    {
    }

    protected virtual void SpawnOffspring(Animal other)
    {

    }

    public virtual List<Chromosome> GeneticRecombination(Animal other)
    {
        List<Chromosome> otherChromosomeList = other.GetChromosomes();
        List<Chromosome> newChromosomeList = new List<Chromosome>();

        newChromosomeList.Add(new Chromosome(C_size.Recombination(otherChromosomeList[0].genes, 0.3f)));
        newChromosomeList.Add(new Chromosome(C_movility.Recombination(otherChromosomeList[1].genes, 1.0f)));
        newChromosomeList.Add(new Chromosome(C_perception.Recombination(otherChromosomeList[2].genes, 2.0f)));
        newChromosomeList.Add(new Chromosome(C_stealth.Recombination(otherChromosomeList[3].genes, 10.0f)));


        return newChromosomeList;

    }

    public void SetChromosomes(List<Chromosome> newChromosomes, bool randomizeSex = true)
    {
        G_sizeScale = newChromosomes[0].genes[0];
        G_strength = new Gene(G_sizeScale.value * 25.0f, GlobalValues.MIN_STRENGTH, GlobalValues.MAX_STRENGTH, GlobalValues.STRENGTH_CONSUMPTION_FACTOR);
        G_baseSpeed = newChromosomes[1].genes[0];
        G_visionRadius = newChromosomes[2].genes[0];
        G_camouflage = newChromosomes[3].genes[0];

        if (randomizeSex) G_sex = Random.Range(0, 10) <= 4 ? Sex.MALE : Sex.FEMALE;
    }

    public void Restart() { OnStart(); }

    public Sex GetSex() { return G_sex; }
    public List<Chromosome> GetChromosomes() {
        return new List<Chromosome>() { C_size, C_movility, C_perception, C_stealth };
    }
    public Animal_Stats GetStats() { return stats; }
    public float GetAnimalCurrentEnergy() { return energy; }
    public float GetAnimalCurrentEnergyConsumption() { return totalGeneticEnergyConsumption; }
    public void ToggleIndicator() { selected_indicator.SetActive(!selected_indicator.activeSelf); }
    public void ToggleBestIndicator() { best_indicator.SetActive(!best_indicator.activeSelf); }

    protected bool IsStronger(Animal other)
    {
        return (stats.strength > other.stats.strength);
    }

    protected bool DetectionCheck (Animal other)
    {
        return (Random.Range(0.0f, 100.0f) > other.stats.camouflage);
    }
    
}
