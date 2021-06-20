using System;
using System.Collections.Generic;
using System.Text;

namespace BillReader.Interfaces
{
    public interface IPdf
    {

        /// <summary>
        ///     Texto del fichero pdf donde cada índice del array corresponde a una página del fichero.
        /// </summary>
        string[] PagedText { get; }

        void Read(string path);

    }
}
