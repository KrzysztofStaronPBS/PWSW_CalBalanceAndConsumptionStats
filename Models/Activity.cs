public class Activity : Entry
{
    public int Duration { get; set; }
    public double MET { get; set; }

    public override EntryType Type => EntryType.Activity;

    public void CalculateCalories(User user)
    {
        Calories = (int)(0.0175 * MET * user.Weight * Duration);
    }

    public override void Validate()
    {
        base.Validate();

        if (Duration <= 0)
            throw new InvalidEntryException("Duration must be > 0.");
        if (MET <= 0)
            throw new InvalidEntryException("MET must be > 0.");
    }
}
