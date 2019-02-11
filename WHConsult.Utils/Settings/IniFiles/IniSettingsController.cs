//-----------------------------------------------------------------------------
//
//  SettingsController.cs
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
using WHConsult.Utils.Files;
using System.IO;
using WHConsult.Utils.Events;
using WHConsult.Utils.Log4Net;

namespace WHConsult.Utils.Settings.IniFiles
{
    public class IniSettingsController
    {
        private ILogService m_LogService;

        #region Singleton

        static readonly IniSettingsController m_Instance = new IniSettingsController();

        public static IniSettingsController Instance
        {
            get { return m_Instance; }
        }

        private IniSettingsController()
        {
            // logger
            m_LogService = new FileLogService(typeof(IniSettingsController));

            IniFiles = new Dictionary<string, IniFile>();
            FilePaths = new Dictionary<string, string>();
        }

        #endregion

        private Dictionary<string, IniFile> IniFiles; // key= FileType, value = iniFile object for this file
        private Dictionary<string, string> FilePaths; // key= SettingsFileType, value = filepath for this file

        #region File Management

        public void CreateFile(string fileType, string filePath)
        {
            m_LogService.Debug("CreateFile fileType:" + fileType + " filePath:" + filePath);
            string dir = Path.GetDirectoryName(filePath);
            if (!System.IO.Directory.Exists(dir) && !dir.Equals("")) Directory.CreateDirectory(dir);
            File.WriteAllText(filePath, "");
            LoadFile(fileType, filePath);
        }

        public void LoadFile(string fileType, string filePath)
        {
            m_LogService.Debug("LoadFile fileType:" + fileType + " filePath:" + filePath);
            if (FilePaths.ContainsKey(fileType)) FilePaths[fileType] = filePath;
            else FilePaths.Add(fileType, filePath);

            IniFile file = new IniFile(filePath);
            if (IniFiles.ContainsKey(fileType)) IniFiles[fileType] = file;
            else IniFiles.Add(fileType, file);

            if (FileLoaded != null) FileLoaded.Invoke(this, new SettingChangedEventArgs(fileType, filePath));
        }

        public void SaveFile(string fileType)
        {
            m_LogService.Debug("SaveFile fileType:" + fileType);

            if (FilePaths.ContainsKey(fileType))
            {
                string filePath = FilePaths[fileType];
                this.SafeFileAs(fileType, filePath);
            }
            else
            {
                m_LogService.Warn("SaveFile fileType:" + fileType + " does not exists");
            }
        }

        public void SafeFileAs(string fileType, string filePath)
        {
            m_LogService.Debug("SafeFileAs fileType:" + fileType + " filePath:" + filePath);
            if (IniFiles.ContainsKey(fileType)) IniFiles[fileType].Save(filePath);
            if (FileSaved != null) FileSaved.Invoke(this, new SettingChangedEventArgs(fileType, filePath));
        }

        public string GetFilePath(string fileType)
        {
            m_LogService.Debug("GetFilePath fileType:" + fileType);
            if (FilePaths.ContainsKey(fileType)) return FilePaths[fileType];
            else return "";
        }

        public string GetFileContent(string fileType)
        {
            m_LogService.Debug("GetFileContent fileType:" + fileType);
            if (IniFiles.ContainsKey(fileType)) return IniFiles[fileType].GetText();
            else return "";
        }

        private IniFile GetInifile(string fileType)
        {
            m_LogService.Debug("GetInifile fileType:" + fileType);
            if (IniFiles.ContainsKey(fileType)) return IniFiles[fileType];
            else throw new ApplicationException("SettingsFileType '" + fileType + "' is not available in this settingsController. Please use load or create file first, before requesting settings stored in this fileType");
        }

        #endregion

        #region Value Management

        public string GetValue(string fileType, string section, string settingName)
        {
            m_LogService.Debug("GetValue fileType:" + fileType + " section:" + section + " settingName:" + settingName);
            IniFile file = GetInifile(fileType);
            return file.GetValue(section, settingName);
        }

        public void SetValue(string fileType, string section, string settingName, string value)
        {
            m_LogService.Debug("SetValue fileType:" + fileType + " section:" + section + " settingName:" + settingName + " value:" + value);
            IniFile file = GetInifile(fileType);
            file.SetValue(section, settingName, value);
        }

        #endregion

        #region Events

        public event EventHandler<SettingChangedEventArgs> FileLoaded;

        public event EventHandler<SettingChangedEventArgs> FileSaved;

        #endregion
    }
}
