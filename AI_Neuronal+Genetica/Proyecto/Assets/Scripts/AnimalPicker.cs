using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalPicker : MonoBehaviour
{
    [SerializeField] Camera _camera;
    Animal animalPicked;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePickedObject();
    }

    void UpdatePickedObject()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Preys", "Predators")))
            {
                Transform objectHit = hit.transform;
                if(hit.collider.gameObject.GetComponent<Animal>())
                {
                    if (animalPicked) animalPicked.ToggleIndicator();

                    animalPicked = hit.collider.gameObject.GetComponent<Animal>();
                    animalPicked.ToggleIndicator();
                }
                
            }
        }
    }

    public Animal GetAnimal() { return animalPicked; }
}
