using System.Collections.Generic;

namespace BillReader.Interfaces
{
    public interface IPdfInfo
    {

        IEnumerable<string> Pages { get; set; }
        string FileName { get; set; }

    }
}
