public sealed class Dice
{
    public int? Value { get; set; } = null;

    public Dice()
    {
        Value = 0;
    }

    public void Roll()
    {
        Value = new Random().Next(1, 7);
    }
}