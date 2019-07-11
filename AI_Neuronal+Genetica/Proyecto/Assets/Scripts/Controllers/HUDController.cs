using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HUDController : MonoBehaviour
{

    [SerializeField]
    GameObject canvas;

    [SerializeField]
    GameObject geneticUI;

    [SerializeField]
    GameObject neuralUI;

    [SerializeField]
    Text Date;

    [SerializeField]
    Text Season;

    [SerializeField]
    Text MaxGen;

    [SerializeField]
    Text TotalBorn;

    [SerializeField]
    Text TotalDead;

    [SerializeField]
    Text AlphaStats;

    [SerializeField]
    Text SelectedStats;

    [SerializeField]
    Text timeStep;

    [SerializeField]
    Slider timeSlider;

    [SerializeField]
    AnimalPicker AnimalPicker;

    [SerializeField]
    BestAnimalPicker BestAnimalPicker;

    [SerializeField]
    Button SpawnPredators;

    [SerializeField]
    Button SpawnPreys;

    [SerializeField]
    InputField numberOfPredatorsToSpawn;

    [SerializeField]
    InputField numberOfPreysToSpawn;

    [SerializeField]
    Button ui_NeuralButton;

    [SerializeField]
    Button ui_GeneticButton;

    string date;
    string hour;
    string season;

    float time_multiplier;
    bool  time_stopped;


    string a_species;
    float a_strength;
    float a_speed;
    float a_size;
    float a_vision;
    float a_camo;
    int a_age;
    int a_currentGeneration;
    float a_energy;
    float a_energy_consumption;
    Sex a_gender;

    float s_strength;
    float s_speed;
    float s_size;
    float s_vision;
    float s_camo;
    int s_age;
    float s_energy;
    float s_energy_consumption;
    Sex s_gender;

    bool canvas_enabled;

    bool geneticAlgorithm_UI;
    bool neuralNetwork_UI;


    // Start is called before the first frame update
    void Start()
    {
        canvas_enabled = true;
        time_stopped = false;
        
        a_gender = Sex.MALE;
        s_gender = Sex.MALE;

        EnableNeuralNetworkUI();
        ui_NeuralButton.Select();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (canvas_enabled)
            {
                canvas.GetComponent<Canvas>().enabled = false;
                canvas_enabled = false;
            }

            else
            {
                canvas.GetComponent<Canvas>().enabled = true;
                canvas_enabled = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (time_stopped)
            {
                Time.timeScale = time_multiplier;
                time_stopped = false;
            }

            else
            {
                Time.timeScale = 0f;
                time_stopped = true;
            }
        }

        UpdateSelectedAnimal();
        UpdateBestAnimal();

       time_multiplier = timeSlider.value;
       if(!time_stopped) Time.timeScale = time_multiplier;



        date = TimeController.Instance.world_calendar.number_of_day.ToString() + "/" 
             + TimeController.Instance.world_calendar.month.ToString()         + "/" 
             + TimeController.Instance.world_calendar.year.ToString();

        hour = TimeController.Instance.world_calendar.day.hour   + ":" +
               TimeController.Instance.world_calendar.day.minute + ":" +
               TimeController.Instance.world_calendar.day.second;

        switch (TimeController.Instance.world_calendar.season)
        {
            case 0:
                season = "Primavera";
                break;
            case 1:
                season = "Verano";
                break;
            case 2:
                season = "Otoño";
                break;
            case 3:
                season = "Invierno";
                break;
        }

        Date.text   = "Fecha Simulada: " + date + " - " + hour;
        Season.text = "Estación Simulada: " + season;
        MaxGen.text = "Generación mas avanzada: " + BestAnimalPicker.GetYoungestGeneration();

        TotalBorn.text = "Total animales Vivos: " + WorldController.Instance.get_animals().Count;
        TotalDead.text = "Total animales Muertos: " + WorldController.Instance.totalDeceases;

        AlphaStats.text =
            "-ESPECIE : " + a_species + "\n" +
            "-Fuerza: "   + a_strength.ToString("F2") + "\n" +
            "-Velocidad: "   + a_speed.ToString("F2") + "\n"+
            "-Tamaño: "      + a_size.ToString("F2") + "\n"+
            "-Visión: "      + a_vision.ToString("F2") + "\n"+
            "-Camuflaje: "   + a_camo.ToString("F2") + "\n\n"+

            "-EDAD: " + a_age  + "\n" +
            "-GENERACIÓN: "    + a_currentGeneration + "\n" +
            "-ENERGÍA: "       + a_energy.ToString("F2") + "\n" +
            "-CONSUMO / DÍA: " +(a_energy_consumption * 10.0f).ToString("F2") + "\n" +
            "-GÉNERO: "        + a_gender     +"\n";

        SelectedStats.text =
            "-Fuerza: "      + s_strength.ToString("F2")   + "\n" +
            "-Velocidad: "   + s_speed.ToString("F2") + "\n" +
            "-Tamaño: "      + s_size.ToString("F2") + "\n" +
            "-Visión: "      + s_vision.ToString("F2") + "\n" +
            "-Camuflaje: "   + s_camo.ToString("F2") + "\n\n" +

            "-EDAD: "          + s_age        + "\n" +
            "-ENERGÍA: "       + s_energy.ToString("F2") + "\n" +
            "-CONSUMO / DÍA: " + (s_energy_consumption*10.0f).ToString("F2") + "\n" +
            "-GÉNERO: "        + s_gender     + "\n";

        timeStep.text  = "X " + time_multiplier.ToString("F2");
    }

    void UpdateSelectedAnimal()
    {
        if (AnimalPicker.GetAnimal())
        {
            s_strength = AnimalPicker.GetAnimal().GetStats().strength;
            s_speed    = AnimalPicker.GetAnimal().GetStats().baseSpeed;
            s_size     = AnimalPicker.GetAnimal().GetStats().sizeScale;
            s_vision   = AnimalPicker.GetAnimal().GetStats().visionRadius;
            s_camo     = AnimalPicker.GetAnimal().GetStats().camouflage;
            s_age      = AnimalPicker.GetAnimal().GetDaysOld();
            s_energy   = AnimalPicker.GetAnimal().GetAnimalCurrentEnergy();
            s_gender   = AnimalPicker.GetAnimal().GetStats().sex;
            s_energy_consumption = AnimalPicker.GetAnimal().GetAnimalCurrentEnergyConsumption();
        }
    }

    void UpdateBestAnimal()
    {
        Animal bestAnimal = BestAnimalPicker.GetAnimal();
        if (BestAnimalPicker.GetAnimal())
        {
            a_species  =  bestAnimal.gameObject.tag == "prey" ? "Conejo" : "Zorro";
            a_strength =  bestAnimal.GetStats().strength;
            a_speed    =  bestAnimal.GetStats().baseSpeed;
            a_size     =  bestAnimal.GetStats().sizeScale;
            a_vision   =  bestAnimal.GetStats().visionRadius;
            a_camo     =  bestAnimal.GetStats().camouflage;
            a_age      =  bestAnimal.GetDaysOld();
            a_gender   =  bestAnimal.GetStats().sex;
            a_energy   =  bestAnimal.GetAnimalCurrentEnergy();
            a_currentGeneration  = bestAnimal.currentGeneration;
            a_energy_consumption = bestAnimal.GetAnimalCurrentEnergyConsumption();
        }
    }

    public void EnableNeuralNetworkUI()
    {
        geneticAlgorithm_UI = false;
        geneticUI.SetActive  (false);

        neuralUI.SetActive(true);
        neuralNetwork_UI = true;
    }

    public void EnableGeneticAlgorithmUI()
    {
        geneticAlgorithm_UI = true;
        geneticUI.SetActive  (true);

        neuralNetwork_UI = false;
        neuralUI.SetActive(false);
    }

    public void SpawnPrey()
    {
        WorldController.Instance.SpawnPreys(int.Parse(numberOfPreysToSpawn.text));
    }

    public void SpawnPredator()
    {
        WorldController.Instance.SpawnPredators(int.Parse(numberOfPredatorsToSpawn.text));
    }
}
