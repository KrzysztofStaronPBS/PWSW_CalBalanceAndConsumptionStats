using System;

namespace PWSW_CalBalanceAndConsumptionStats.Models;

public enum Gender { K, M }

public class User
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty; // hashowane w serwisie
	public int Age { get; set; }
	public Gender Gender { get; set; }
	public double Weight { get; set; }
	public double Height { get; set; }
	public double BMI { get; private set; }
	public double ActivityLevel { get; set; } = 1.2;
	public int DailyGoalOffset { get; set; }
	public double CalculatedDailyGoal
	{
		get
		{
			if (Weight <= 0 || Height <= 0 || Age <= 0) return 2000;

			// BMR
			double bmr = (10 * Weight) + (6.25 * Height * 100) - (5 * Age);
			bmr = (Gender == Gender.M) ? bmr + 5 : bmr - 161;

			// TDEE (Całkowite zapotrzebowanie) + Cel (Offset)
			return (bmr * ActivityLevel) + DailyGoalOffset;
		}
	}
	public void CalculateBMI()
	{
		if (Height > 0)
			BMI = Math.Round(Weight / (Height * Height), 2);
	}

	public void Validate()
	{
		if (Age <= 0 || Age > 120) throw new ArgumentException("Niepoprawny wiek.");
		if (Weight <= 0 || Height <= 0) throw new ArgumentException("Waga i wzrost muszą być dodatnie.");
		if (ActivityLevel < 1.0) throw new ArgumentException("Niepoprawny poziom aktywności.");
	}
}