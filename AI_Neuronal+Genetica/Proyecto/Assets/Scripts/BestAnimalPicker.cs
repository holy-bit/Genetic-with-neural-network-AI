using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BestAnimalPicker : MonoBehaviour
{
    Animal oldestAnimal;
    int youngestGeneration;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("PickBestAnimal", 1.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PickBestAnimal()
    {
        int oldestAge;
        if (oldestAnimal)
            oldestAge = oldestAnimal.GetDaysOld();
        else
            oldestAge = 0;

        foreach(Animal animal in WorldController.Instance.get_animals())
        {
            int currentAge = animal.GetDaysOld();

            if (currentAge > oldestAge)
            {
                if(oldestAnimal)
                    oldestAnimal.ToggleBestIndicator();

                oldestAnimal = animal;
                oldestAge = currentAge;

                oldestAnimal.ToggleBestIndicator();
            }

            if (animal.currentGeneration > youngestGeneration) youngestGeneration = animal.currentGeneration;
        }
    }

    public Animal GetAnimal() { return oldestAnimal; }
    public int GetYoungestGeneration() { return youngestGeneration; }
}
