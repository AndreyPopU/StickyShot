using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    // When you open inventory find what resources you have and load recipes
    // Scriptable object for recipe? and a list for recipes, string of ingridients and check if that string contains item name

    public Slider timeLeftSlider;
    public GameObject taskList;
    public float toggle;
    public bool canToggle;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    //public void CalculateCraftingPosibilities()
    //{
    //    // Loop through every item in inventory type = material and check if any of the recipes contain it
    //    possibleRecipes.Clear();

    //    for (int i = 0; i < inventory.slots.Length; i++)
    //    {
    //        if (inventory.slots[i] == null || inventory.slots[i].item == null || inventory.slots[i].item.type != Item.Type.material) continue;

    //        for (int j = 0; j < craftingRecipes.Length; j++)
    //        {
    //            for (int k = 0; k < craftingRecipes[j].requirements.Count; k++)
    //            {
    //                if (craftingRecipes[j].requirements[k] == inventory.slots[i].item)
    //                {
    //                    if (!possibleRecipes.Contains(craftingRecipes[j])) possibleRecipes.Add(craftingRecipes[j]);
    //                }
    //            }
    //        }
    //    }

    //    AddCraftingOption(possibleRecipes);
    //}

    //public void AddCraftingOption(List<CraftingRecipe> recipe)
    //{
    //    List<string> options = new List<string>();

    //    for (int i = 0; i < recipe.Count; i++)
    //    {
    //        options.Add(recipe[i].recipeName);
    //    }

    //    craftingDropdown.AddOptions(options);

    //    // Recipes you can craft to be on top, and the rest after them
    //}
}
