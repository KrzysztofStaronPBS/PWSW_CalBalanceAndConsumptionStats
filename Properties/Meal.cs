using System;

public class Meal : Entry
{
    public double Quantity { get; set; }

    public override EntryType Type => EntryType.Meal;

    public override void Validate()
    {
        base.Validate();

        if (Quantity <= 0)
            throw new InvalidEntryException("Quantity must be > 0.");
    }
}
