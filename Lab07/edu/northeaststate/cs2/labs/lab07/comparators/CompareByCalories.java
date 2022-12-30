/**
 * -------------------------------------------------
 * File name: CompareByCalories.java
 * Project Name: Lab07
 * -------------------------------------------------
 * Created By: Cora Yon
 * Email: cyon@stumail.northeaststate.edu
 * Course: CISP 1020
 * Creation date: 3/26/22
 * -------------------------------------------------
 */

package edu.northeaststate.cs2.labs.lab07.comparators;

import edu.northeaststate.cs2.labs.lab07.models.Sandwich;

import java.util.Comparator;

/**
 * Class Name: CompareByCalories
 * Purpose: Implements a Comparator interface to compare two sandwiches by calories
 */
public class CompareByCalories implements Comparator<Sandwich> {
    /**
     * Method Name: compare
     * Method Description: Implements the Comparator interface method, compare. Uses String class compare function to
     * return a comparison by calories
     * @param s1
     * @param s2
     * @return
     */

    public int compare(Sandwich s1, Sandwich s2){
        return Integer.compare(s1.getCalories(), s2.getCalories());
    }
}