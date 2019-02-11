//-----------------------------------------------------------------------------
//
//  IniSettingsManger.cs
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
using WHConsult.Utils.Log4Net;

namespace WHConsult.Utils.Settings.IniFiles
{
    public class IniSettingsManager : ISettingsManager
    {
        private ILogService m_LogService;

        private string m_Section;
        private string m_FileType;

        public IniSettingsManager(string section, string fileType)
        {
            // logger
            m_LogService = new FileLogService(typeof(IniSettingsManager));

            m_Section = section;
            m_FileType = fileType;
            IniSettingsController.Instance.FileLoaded += new EventHandler<Events.SettingChangedEventArgs>(Instance_FileLoaded);
        }

        #region Get Values
        
        public string GetStringValue(string settingName, string defaultValue)
        {
            m_LogService.Debug("GetStringValue: SettingName:" + settingName + " DefaultValue:" + defaultValue.ToString());
            string result = IniSettingsController.Instance.GetValue(m_FileType, m_Section, settingName);
            if (result != null) return result;
            else
            {
                m_LogService.Debug("New Default Setting Created: SettingName:" + settingName + " DefaultValue:" + defaultValue.ToString());
                SetStringValue(settingName, defaultValue); // create setting that did not exist;
                return defaultValue;
            }
        }

        public bool GetBooleanValue(string settingName, bool defaultValue)
        {
            m_LogService.Debug("GetBooleanValue: SettingName:" + settingName + " DefaultValue:" + defaultValue.ToString());
            string result = IniSettingsController.Instance.GetValue(m_FileType, m_Section, settingName);
            if (result != null) return Convert.ToBoolean(result);
            else
            {
                m_LogService.Debug("New Default Setting Created: SettingName:" + settingName + " DefaultValue:" + defaultValue.ToString());
                SetBooleanValue(settingName, defaultValue); // create setting that did not exist;
                return defaultValue;
            }
        }

        public byte GetByteValue(string settingName, byte defaultValue)
        {
            m_LogService.Debug("GetByteValue: SettingName:" + settingName + " DefaultValue:" + defaultValue.ToString());
            string result = IniSettingsController.Instance.GetValue(m_FileType, m_Section, settingName);
            if (result != null) return Convert.ToByte(result);
            else
            {
                m_LogService.Debug("New Default Setting Created: SettingName:" + settingName + " DefaultValue:" + defaultValue.ToString());
                SetByteValue(settingName, defaultValue); // create setting that did not exist;
                return defaultValue;
            }
        }

        public int GetIntValue(string settingName, int defaultValue)
        {
            m_LogService.Debug("GetIntValue: SettingName:" + settingName + " DefaultValue:" + defaultValue.ToString());
            string result = IniSettingsController.Instance.GetValue(m_FileType, m_Section, settingName);
            if (result != null) return Convert.ToInt32(result);
            else
            {
                m_LogService.Debug("New Default Setting Created: SettingName:" + settingName + " DefaultValue:" + defaultValue.ToString());
                SetIntValue(settingName, defaultValue); // create setting that did not exist;
                return defaultValue;
            }
        }

        public double GetDoubleValue(string settingName, double defaultValue)
        {
            m_LogService.Debug("GetDoubleValue: SettingName:" + settingName + " DefaultValue:" + defaultValue.ToString());
            string result = IniSettingsController.Instance.GetValue(m_FileType, m_Section, settingName);
            if (result != null) return Convert.ToDouble(result);
            else
            {
                m_LogService.Debug("New Default Setting Created: SettingName:" + settingName + " DefaultValue:" + defaultValue.ToString());
                SetDoubleValue(settingName, defaultValue); // create setting that did not exist;
                return defaultValue;
            }
        } 
       
        #endregion

        #region Set Values
        
        public void SetStringValue(string settingName, string value)
        {
            m_LogService.Debug("SetStringValue: SettingName:" + settingName + " Value:" + value);
            IniSettingsController.Instance.SetValue(m_FileType, m_Section, settingName, value);
        }

        public void SetBooleanValue(string settingName, bool value)
        {
            m_LogService.Debug("SetBooleanValue: SettingName:" + settingName + " Value:" + value.ToString());
            IniSettingsController.Instance.SetValue(m_FileType, m_Section, settingName, value.ToString());
        }

        public void SetByteValue(string settingName, byte value)
        {
            m_LogService.Debug("SetByteValue: SettingName:" + settingName + " Value:" + value.ToString());
            IniSettingsController.Instance.SetValue(m_FileType, m_Section, settingName, value.ToString());
        }

        public void SetIntValue(string settingName, int value)
        {
            m_LogService.Debug("SetIntValue: SettingName:" + settingName + " Value:" + value.ToString());
            IniSettingsController.Instance.SetValue(m_FileType, m_Section, settingName, value.ToString());
        }

        public void SetDoubleValue(string settingName, double value)
        {
            m_LogService.Debug("SetDoubleValue: SettingName:" + settingName + " Value:" + value.ToString());
            IniSettingsController.Instance.SetValue(m_FileType, m_Section, settingName, value.ToString());
        } 

        #endregion

        #region Save Values

        public void SaveValues()
        {
            m_LogService.Debug("SaveValues");
            IniSettingsController.Instance.SaveFile(m_FileType);
        }

        #endregion

        #region Events
        
        public event EventHandler<Events.SettingChangedEventArgs> NewFileLoaded;

        void Instance_FileLoaded(object sender, Events.SettingChangedEventArgs e)
        {
            m_LogService.Debug("NewFileLoadedEvent:" + (NewFileLoaded != null).ToString());
            if (NewFileLoaded != null) NewFileLoaded.Invoke(this, e);
        }

        #endregion
    }
}
