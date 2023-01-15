namespace SenteTask
{
    readonly struct Transfer
    {
        public readonly User origin;
        public readonly uint amount;
        public Transfer(User origin, uint amount)
        {
            this.origin = origin;
            this.amount = amount;
        }
    }
}