public class EmptyCell: DungeonCell
{
    public bool Equals(EmptyCell other)
    {
        return other == null || other.GetType() != typeof(EmptyCell);
    }
}