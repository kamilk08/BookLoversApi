using System;

namespace BookLovers.Readers.Application.WriteModels.Readers
{
    public class ReaderDetailsWriteModel
    {
        public string Name { get; set; }

        public string SecondName { get; set; }

        public string FullName => Name + " " + SecondName;

        public string BirthPlace { get; set; }

        public DateTime BirthDate { get; set; }
    }
}