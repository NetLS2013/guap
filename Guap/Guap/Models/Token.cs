namespace Guap.Models
{
    using Nethereum.Util;

    using SQLite;

    [Table("Token")]
    public class Token
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }

        public string Address { get; set; }

        public string Symbol { get; set; }

        public string Name { get; set; }

        public int Decimals { get; set; }

        [Ignore]
        public BigDecimal Balance { get; set; }

        [Ignore]
        public string BalanceString => Balance.ToString();
    }
}