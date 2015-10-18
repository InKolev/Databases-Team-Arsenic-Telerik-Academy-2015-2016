namespace Toys.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Toys.Data.Contracts;

    public abstract class Command : ICommand
    {
        protected readonly IToysData Data;

        protected Command(IToysData data)
        {
            this.Data = data;
        }

        public abstract bool Execute();

        protected List<string[]> ImportFromTextFile(string filePath)
        {
            var path = filePath;
            var list = new List<string[]>();

            using (var reader = new StreamReader(path))
            {
                var line = reader.ReadLine();

                while (line != null)
                {
                    var fileData = line.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    list.Add(fileData);

                    line = reader.ReadLine();
                }
            }

            return list;
        }
    }
}