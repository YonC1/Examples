/**
 * -------------------------------------------------
 * File name: Lab07.java
 * Project Name: Lab07
 * -------------------------------------------------
 * Created By: Cora Yon
 * Email: cyon@stumail.northeaststate.edu
 * Course: CISP 1020
 * Creation date: 3/26/22
 * -------------------------------------------------
 */

package edu.northeaststate.cs2.labs.lab07.ui;

import edu.northeaststate.cs2.labs.lab07.comparators.CompareByCalories;
import edu.northeaststate.cs2.labs.lab07.comparators.CompareByCost;
import edu.northeaststate.cs2.labs.lab07.comparators.CompareByIsVegan;
import edu.northeaststate.cs2.labs.lab07.comparators.CompareByName;
import edu.northeaststate.cs2.labs.lab07.models.Sandwich;

import java.util.ArrayList;

/**
 * Class Name: Lab07
 * Purpose: An application that compares sandwiches
 */
public class Lab07 {

    /**
     * Method Name: printSandwiches
     * Method Description: prints each sandwich in the sandwiches arraylist
     * @param:
     *
     */
    public static void printSandwiches(ArrayList<Sandwich> sandwiches){
        for(Sandwich sandwich : sandwiches) {
            System.out.println(sandwich.toString());
        }
    }
    /**
     * Method Name: main
     * Method Description: Entry point for the application.
     * @param args command line arguments
     *
     */
    public static void main(String[] args) {
        //welcome message
        System.out.print("---------------------------------\n");
        System.out.print("Cora Yon - CISP 1020 R - Lab 7\n");
        System.out.print("---------------------------------\n");

        //Create an array list for the sandwiches to go into
        ArrayList<Sandwich> sandwiches = new ArrayList<>();

        //Add sandwiches to the list
        sandwiches.add(new Sandwich("Veggie Panini", 3.99, 350, true));
        sandwiches.add(new Sandwich("Big Pal", 3.49, 593, false));
        sandwiches.add(new Sandwich("Ultimate Avocado", 4.50, 400, true));
        sandwiches.add(new Sandwich("Chicken Salad", 4.25, 500, false));

        //Prints an unsorted list
        System.out.println("Unsorted list of sandwiches: ");
        printSandwiches(sandwiches);
        System.out.println("\n");

        //Sorted by name
        System.out.println("Sorted by name: ");
        sandwiches.sort(new CompareByName());
        printSandwiches(sandwiches);
        System.out.println("\n");

        //Sorted by calories
        System.out.println("Sorted by calories: ");
        sandwiches.sort(new CompareByCalories());
        printSandwiches(sandwiches);
        System.out.println("\n");

        //Sorted by cost
        System.out.println("Sorted by cost: ");
        sandwiches.sort(new CompareByCost());
        printSandwiches(sandwiches);
        System.out.println("\n");

        //Sorted by is vegan
        System.out.println("Sorted by is vegan: ");
        sandwiches.sort(new CompareByIsVegan());
        printSandwiches(sandwiches);
        System.out.println("\n");
    }
}
