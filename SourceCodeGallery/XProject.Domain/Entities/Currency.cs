using System;

namespace XProject.Domain.Entities
{
    public class Currency : EntityBase//, IEquatable<Currency>, IEquatable<string>
    {
        public Currency()
        {
            //Set default value
            Precision = 2;
        }

        public string Name { get; set; }
        public string Symbol { get; set; }
        public bool IsDefault { get; set; }
        public bool IsAppendSymbol { get; set; }
        public int Precision { get; set; }

        #region IEquatable<Currency> Members

        public bool Equals(Currency other)
        {
            if (ReferenceEquals(this, other))
                return true;

            return ID == other.ID;
        }

        #endregion

        #region IEquatable<string> Members

        public bool Equals(string otherName)
        {
            return Name.Equals(otherName, StringComparison.InvariantCultureIgnoreCase);
        }

        #endregion

        public override string ToString()
        {
            return Name;
        }

        public string Format(decimal value, bool withUnit = false)
        {
            Precision = 0;
            string format = "N" + Precision;
            if (withUnit)
            {
                if (IsAppendSymbol)
                {
                    return value.ToString(format) + " " + Symbol;
                }
                return Symbol + " " + value.ToString(format);
            }
            return value.ToString(format);
        }
    }
}
