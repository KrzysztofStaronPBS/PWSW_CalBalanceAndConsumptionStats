using System;
using System.Collections.Generic;

namespace PWSW_CalBalanceAndConsumptionStats.Models;

public class User
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty; // hashowane w serwisie
	public int Age { get; set; }
	public string Gender { get; set; } = "M";
	public double Weight { get; set; }
	public double Height { get; set; }
	public double BMI { get; private set; }
	public int GoalCalories { get; set; }

	// TypeNameHandling, aby JSON wiedział czy to Meal czy Activity
	public List<Entry> Entries { get; set; } = new();

	public void CalculateBMI()
	{
		if (Height > 0) BMI = Weight / (Height * Height);
	}

	public void Validate()
	{
		if (Age <= 0 || Age > 120) throw new ArgumentException("Niepoprawny wiek.");
		if (Weight <= 0 || Height <= 0) throw new ArgumentException("Waga i wzrost muszą być dodatnie.");
	}
}