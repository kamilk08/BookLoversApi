using System;

namespace BookLovers.Librarians.Domain.Librarians
{
    public class DecisionJustification : BookLovers.Base.Domain.ValueObject.ValueObject<DecisionJustification>
    {
        public string Content { get; private set; }

        public DateTime Date { get; private set; }

        private DecisionJustification()
        {
        }

        public DecisionJustification(string content, DateTime date)
        {
            this.Content = content;
            this.Date = date;
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.Content.GetHashCode();
            hash = (hash * 23) + this.Date.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(DecisionJustification obj)
        {
            return this.Content == obj.Content && this.Date == obj.Date;
        }
    }
}