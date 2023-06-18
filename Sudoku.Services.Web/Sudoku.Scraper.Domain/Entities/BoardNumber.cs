namespace Sudoku.Scraper.Domain.Entities
{
    public readonly struct BoardNumber : IEquatable<BoardNumber>
    {
        public BoardNumber(string id)
        {
            Id = id;
        }

        public string Id { get; init; }

        public override bool Equals(object? obj) => obj is BoardNumber other && this.Equals(other);

        public bool Equals(BoardNumber other)
        {
            return this.Id == other.Id;
        }

        public static bool operator ==(BoardNumber lhs, BoardNumber rhs) => lhs.Equals(rhs);
        public static bool operator !=(BoardNumber lhs, BoardNumber rhs) => !(lhs == rhs);

        public override int GetHashCode() => Id.GetHashCode();
    }
}
