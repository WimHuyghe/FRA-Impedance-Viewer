//-----------------------------------------------------------------------------
//
//  SettingsChangedEventArgs.cs
//
//  Part of Utillities Library from Wim Huyghe
//
//  Copyright (C) 2012 Wim Huyghe
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
//  Web:    http://www.WHConsult.be
//  Email:  SpeakerReport@WHConsult.be
//
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WHConsult.Utils.Events
{
    public class SettingChangedEventArgs : EventArgs
    {
        public SettingChangedEventArgs(string fileType, string filePath)
        {
            FileType = fileType;
            FilePath = filePath;
        }

        private string m_FileType;
        public string FileType
        {
            get { return m_FileType; }
            private set { m_FileType = value; }
        }

        private string m_FilePath;
        public string FilePath
        {
            get { return m_FilePath; }
            private set { m_FilePath = value; }
        }
        
    }
}