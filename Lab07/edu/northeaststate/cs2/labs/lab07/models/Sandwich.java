/**
 * -------------------------------------------------
 * File name: Sandwich.java
 * Project Name: Lab07
 * -------------------------------------------------
 * Created By: Cora Yon
 * Email: cyon@stumail.northeaststate.edu
 * Course: CISP 1020
 * Creation date: 3/26/22
 * -------------------------------------------------
 */


package edu.northeaststate.cs2.labs.lab07.models;

/**
 * Class Name: Sandwich
 * Purpose: Models a Sandwich object
 */
public class Sandwich {
    private String name;
    private double cost;
    private int calories;
    private boolean isVegan;


    /**
     * Method Name: Sandwich
     * Method Description: Parameterized constructor
     * @param name
     * @param cost
     * @param calories
     * @param isVegan
     */
    public Sandwich(String name, double cost, int calories, boolean isVegan){
        this.name = name;
        this.cost = cost;
        this.calories = calories;
        this.isVegan = isVegan;
    }

    /**
     * Method Name: getName
     * Method Description: Returns the name attribute for Sandwich
     * @return String
     */
    public String getName(){
        return this.name;
    }

    /**
     * Method Name: getCost
     * Method Description: Returns the cost attribute for Sandwich
     * @return double
     */
    public double getCost(){
        return this.cost;
    }

    /**
     * Method Name: getCalories
     * Method Description: Returns the calories attribute for Sandwich
     * @return int
     */
    public int getCalories(){
        return this.calories;
    }

    /**
     * Method Name: getIsVegan
     * Method Description: Returns true or false for if a Sandwich is vegan
     * @return boolean
     */
    public boolean getIsVegan(){
        return this.isVegan;
    }

    /**
     * Method Name: toString
     * Method description: Build a string that represents a Sandwich
     * @return String
     */
    public String toString(){
        return "Sandwich [name=" +
                this.name +
                ", cost=" +
                this.cost +
                ", calories=" +
                this.calories +
                ", is vegan=" +
                this.isVegan +
                "]";
    }
}
