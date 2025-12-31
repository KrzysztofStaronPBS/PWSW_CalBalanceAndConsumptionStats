using System;
using System.Collections.Generic;

public class User
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
	public int Age { get; set; }
	public string Gender { get; set; } = "M";
	public double Weight { get; set; }
	public double Height { get; set; }
	public double BMI { get; private set; }
	public int GoalCalories { get; set; }
	public List<Entry> Entries { get; set; } = new();

	public void Validate()
	{
		if (Id < 1)
			throw new ArgumentException("Id must be > 0.");
		if (Age <= 0 || Age >= 120)
			throw new ArgumentException("Age must be between 1 and 119.");
		if (Weight <= 0 || Weight > 250)
			throw new ArgumentException("Weight must be between 0 and 250.");
		if (Height <= 0 || Height > 3.0)
			throw new ArgumentException("Height must be between 0 and 3.0 meters.");
		if (Gender != "M" && Gender != "K")
			throw new ArgumentException("Gender must be 'M' or 'K'.");
	}

	public void CalculateBMI() => BMI = Weight / (Height * Height);

	public void UpdatePhysicalData(double weight, double height, int age, string gender)
	{
		Validate();

		Weight = weight;
		Height = height;
		Age = age;
		Gender = gender;
		CalculateBMI();
	}

	public DailySummary GetDailySummary(DateTime date)
	{
		var summary = new DailySummary { Date = date };
		summary.Calculate(this);
		return summary;
	}

	public Meal CreateCustomMeal(string name, int calories, double quantity, DateTime date)
	{
		var meal = new Meal { Name = name, Calories = calories, Quantity = quantity, Date = date };
		meal.Validate();
		Entries.Add(meal);
		return meal;
	}

	public Activity CreateCustomActivity(string name, int duration, double met, DateTime date)
	{
		var activity = new Activity { Name = name, Duration = duration, MET = met, Date = date };
		activity.Validate();
		activity.CalculateCalories(this);
		Entries.Add(activity);
		return activity;
	}
}

