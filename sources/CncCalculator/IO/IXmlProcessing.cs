namespace As.Applications.IO
{
    interface IXmlProcessing
    {
        /// <summary>
        /// At root level, prepare the data structure tree for XML writing.
        /// </summary>
        void XmlWritePrepare();

        /// <summary>
        /// At root level, process the data structure tree after XML writing.
        /// </summary>
        void XmlWriteProcess();

        /// <summary>
        /// At root level, process the data structure tree after XML reading.
        /// </summary>
        void XmlReadProcess();
    }
}
