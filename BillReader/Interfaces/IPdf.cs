namespace BillReader.Interfaces
{
    public interface IPdf
    {

        /// <summary>
        ///     Texto del fichero pdf donde cada índice del array corresponde a una página del fichero.
        /// </summary>
        string[] PagedText { get; }

        /// <summary>
        ///     Nombre del fichero pdf.
        /// </summary>
        string FileName { get; }

        void Read(string path);

    }
}
