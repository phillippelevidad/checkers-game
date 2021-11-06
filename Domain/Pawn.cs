using System.Collections.Generic;

namespace Domain
{
    public class Pawn : ValueObject
    {
        public Pawn(Player owner)
        {
            Owner = owner;
        }

        public Player Owner { get; }

        public bool IsCrowned { get; private set; }

        public void Crown()
        {
            IsCrowned = true;
        }

        public override string ToString()
        {
            return Owner.ToString();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Owner;
        }
    }
}
