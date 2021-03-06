﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHConsult.Utils.Log4Net;
using WHConsult.Utils.Settings;
using WHConsult.Utils.Settings.IniFiles;

namespace FRA_IMP
{
    public class CurrentSettings
    {
        private ILogService m_LogService;
        private ISettingsManager m_SettingsManager;

        #region Singleton

        static readonly CurrentSettings m_Instance = new CurrentSettings();

        public static CurrentSettings Instance
        {
            get { return m_Instance; }
        }

        private CurrentSettings()
        {
            // logger
            m_LogService = new FileLogService(typeof(CurrentSettings));

            // Settings to be stored in ini file type
            m_SettingsManager = new IniSettingsManager("General", "Settings");
            m_SettingsManager.NewFileLoaded += M_SettingsManager_NewFileLoaded;     
        }

        #endregion

        #region Events

        private void M_SettingsManager_NewFileLoaded(object sender, WHConsult.Utils.Events.SettingChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        #endregion

        #region Current Settings

        public string PathLastMeasurementFile
        {
            get { return m_SettingsManager.GetStringValue("PathLastMeasurementFile", ""); }
            set { m_SettingsManager.SetStringValue("PathLastMeasurementFile", value); }
        }

        public string PathLastImageFile
        {
            get { return m_SettingsManager.GetStringValue("PathLastImageFile", ""); }
            set { m_SettingsManager.SetStringValue("PathLastImageFile", value); }
        }

        public string PathLastDataTableFile
        {
            get { return m_SettingsManager.GetStringValue("PathLastDataTableFile", ""); }
            set { m_SettingsManager.SetStringValue("PathLastDataTableFile", value); }
        }

        public bool SaveSettingsOnExit
        {
            get { return m_SettingsManager.GetBooleanValue("SaveSettingsOnExit", true); }
            set { m_SettingsManager.SetBooleanValue("SaveSettingsOnExit", value); }
        }

        public double ReferenceResistor
        {
            get { return m_SettingsManager.GetDoubleValue("ReferenceResistor", 47.0); }
            set { m_SettingsManager.SetDoubleValue("ReferenceResistor", value); }
        }

        public bool LogaritmicFrequencyAxis
        {
            get { return m_SettingsManager.GetBooleanValue("LogaritmicFrequencyAxis", true); }
            set { m_SettingsManager.SetBooleanValue("LogaritmicFrequencyAxis", value); }
        }

        public bool DisplayLegendInChart
        {
            get { return m_SettingsManager.GetBooleanValue("DisplayLegendInChart", true); }
            set { m_SettingsManager.SetBooleanValue("DisplayLegendInChart", value); }
        }

        public bool PlotAbsoluteReactanceValues
        {
            get { return m_SettingsManager.GetBooleanValue("PlotAbsoluteReactanceValues", true); }
            set { m_SettingsManager.SetBooleanValue("PlotAbsoluteReactanceValues", value); }
        }

        #endregion

        #region Settins File Management

        public void SaveSettingsAs(string fileName)
        {
            IniSettingsController.Instance.SafeFileAs("Settings", fileName);
        }

        public void SaveSettings()
        {
            IniSettingsController.Instance.SaveFile("Settings");
        }
        
        #endregion

    }

}
