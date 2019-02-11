using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRA_IMP
{
    public class FRAFileEventArgs
    {      
        public FRAFileEventArgs(FRAFile file, FRAFileChange change)
        {
            m_File = file;
            m_Change = change;
        }

        FRAFile m_File;
        public FRAFile File
        {
            get { return m_File; }
        }

        FRAFileChange m_Change;
        public FRAFileChange Change
        {
            get { return m_Change; }
        }
    }

    public enum FRAFileChange
    {
        FileDeteted = 1,
        FileAdded = 2,
        FileChanged = 3,
    }
}
